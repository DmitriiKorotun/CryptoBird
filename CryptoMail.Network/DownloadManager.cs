using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Network
{
    public static class DownloadManager
    {
        public static void DownloadAttachments(MimeMessage message, string currentUser)
        {
            if (message.Attachments.Count() > 0)
            {
                string attachmentDir = GetNewDirName(((MailboxAddress)message.From[0]).Address, currentUser);

                System.IO.Directory.CreateDirectory(attachmentDir);

                foreach (var attachment in message.Attachments)
                {
                    string attachmentName = GetAttachmentName(attachmentDir, attachment.ContentDisposition.FileName);

                    SaveMailAttachment(attachment, attachmentName);
                }
            }
        }

        private static void SaveMailAttachment(MimeEntity attachment, string filename)
        {
            using (var stream = File.Create(filename))
            {
                if (attachment is MessagePart)
                {
                    var part = (MessagePart)attachment;

                    part.Message.WriteTo(stream);
                }
                else
                {
                    var part = (MimePart)attachment;

                    part.Content.DecodeTo(stream);
                }
            }
        }

        private static string GetNewDirName(string from, string currentUser)
        {
            return "Profiles/" + currentUser + "/Attachments-" + from + "-" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
        }

        private static string GetAttachmentName(string dir, string filename)
        {
            return dir + "/" + filename;
        }
    }
}
