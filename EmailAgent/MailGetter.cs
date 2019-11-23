using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using EmailAgent.Entities.Caching;

namespace EmailAgent
{
    public enum MailSpecialFolder
    {
        Inbox,
        Sent,
        Drafts,
        Trash
    }

    public static class MailGetter
    {
        public static List<MimeMessage> GetAllMessages(string host, int port, string login, string password, MailSpecialFolder folder)
        {
            List<MimeMessage> messages;

            using (var client = new ImapClient())
            {
                //// For demo-purposes, accept all SSL certificates
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(host, port, true);

                client.Authenticate(login, password);

                var mailFolder = GetMailFolder(client, folder);
                mailFolder.Open(FolderAccess.ReadOnly);

                messages = new List<MimeMessage>(mailFolder.Count);

                //Console.WriteLine("Total messages: {0}", mailFolder.Count);
                //Console.WriteLine("Recent messages: {0}", mailFolder.Recent);

                //for (int i = 0; i < mailFolder.Count; i++)
                //{
                //    var message = mailFolder.GetMessage(i);

                //    Console.WriteLine("Subject: {0}", message.Subject);

                //    messages.Add(message);
                //}

                //client.Disconnect(true);

                return messages;
            }
        }

        private static IMailFolder GetMailFolder(ImapClient client, MailSpecialFolder folderToFind)
        {
            IMailFolder folder;

            switch(folderToFind)
            {
                case MailSpecialFolder.Inbox:
                    folder = client.Inbox;
                    break;
                case MailSpecialFolder.Sent:
                    folder = client.GetFolder(SpecialFolder.Sent);
                    break;
                case MailSpecialFolder.Drafts:
                    folder = client.GetFolder(SpecialFolder.Drafts);
                    break;
                case MailSpecialFolder.Trash:
                    folder = client.GetFolder(SpecialFolder.Trash);
                    break;
                default:
                    folder = null;
                    break;
            }

            return folder;
        }

        public static MimeMessage GetMessage(string host, int port, string login, string password, int messageIndex)
        {
            MimeMessage message;

            using (var client = new ImapClient())
            {
                // For demo-purposes, accept all SSL certificates
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(host, port, true);

                client.Authenticate(login, password);

                client.Inbox.Open(FolderAccess.ReadOnly);

                message = client.Inbox.GetMessage(messageIndex);

                client.Disconnect(true);

                return message;
            }
        }

        public static bool DeleteMessage(string host, int port, string login, string password, int messageId)
        {
            try
            {
                using (var client = new ImapClient())
                {
                    // For demo-purposes, accept all SSL certificates
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect(host, port, true);

                    client.Authenticate(login, password);

                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadWrite);

                    inbox.AddFlags(new int[] { messageId }, MessageFlags.Deleted, true);

                    client.Disconnect(true);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static List<MailMessage> CastToMailMessage(List<MimeMessage> mimeMessages)
        {
            List<MailMessage> messages = new List<MailMessage>(mimeMessages.Count);

            foreach (MimeMessage mimeMessage in mimeMessages)
            {
                var from = (MailboxAddress)mimeMessage.From[0];
                var to = (MailboxAddress)mimeMessage.To[0];

                var mailMessage = new MailMessage(from.Address, to.Address, mimeMessage.Subject, mimeMessage.HtmlBody);

                SetMessageHeaders(mailMessage, mimeMessage);

                messages.Add(mailMessage);
            }

            return messages;
        }

        private static void SetMessageHeaders(MailMessage mailMessage, MimeMessage mimeMessage)
        {
            SetMessageHeader(mailMessage, "Uid", mimeMessage.Headers[HeaderId.MessageId]);
            SetMessageHeader(mailMessage, "Date", mimeMessage.Headers[HeaderId.Date]);
        }

        private static void SetMessageHeader(MailMessage mailMessage, string mailMessageHeaderName, string mimeMessageHeader)
        {
            if (!string.IsNullOrEmpty(mimeMessageHeader))
                mailMessage.Headers[mailMessageHeaderName] = mimeMessageHeader;
        }
    }
}
