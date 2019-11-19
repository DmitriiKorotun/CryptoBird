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

namespace EmailAgent
{
    public class MailGetter
    {
        public List<MimeMessage> GetAllMessagesTest()
        {
            return GetAllMessages("imap.gmail.com", 993, UserData.Login, UserData.Password);
        }

        //public List<MailMessage> GetAllMessages(string host, int port, string login, string password)
        //{
        //    List<MailMessage> messages;

        //    using (var client = new ImapClient())
        //    {
        //        // For demo-purposes, accept all SSL certificates
        //        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

        //        client.Connect(host, port, true);

        //        client.Authenticate(login, password);

        //        // The Inbox folder is always available on all IMAP servers...
        //        var inbox = client.Inbox;
        //        inbox.Open(FolderAccess.ReadOnly);

        //        messages = new List<MailMessage>(inbox.Count);

        //        Console.WriteLine("Total messages: {0}", inbox.Count);
        //        Console.WriteLine("Recent messages: {0}", inbox.Recent);

        //        for (int i = 0; i < inbox.Count; i++)
        //        {
        //            var message = inbox.GetMessage(i);

        //            Console.WriteLine("Subject: {0}", message.Subject);
        //            var kek = (MimeKit.MailboxAddress)message.From[0];
        //            var kek2 = (MimeKit.MailboxAddress)message.To[0];

        //            var ls = new MailMessage(kek.Address, kek2.Address, message.Subject, message.HtmlBody);

        //            for (int j = 0; j < message.Attachments.Count(); ++j)
        //            {
        //                var attachment = message.Attachments.ElementAt(j);

        //                attachment.WriteTo()
        //                using (var stream = new MemoryStream())
        //                {
        //                    message.WriteTo(stream);

        //                    ls.Attachments.Add(new Attachment(stream, j.ToString()));

        //                    stream.Close();
        //                }
        //            }

        //            messages.Add(ls);
        //        }

        //        client.Disconnect(true);
        //    }

        //    return messages;
        //}

        public List<MimeMessage> GetAllMessages(string host, int port, string login, string password)
        {
            List<MimeMessage> messages;

            using (var client = new ImapClient())
            {
                // For demo-purposes, accept all SSL certificates
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(host, port, true);

                client.Authenticate(login, password);

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                messages = new List<MimeMessage>(inbox.Count);

                Console.WriteLine("Total messages: {0}", inbox.Count);
                Console.WriteLine("Recent messages: {0}", inbox.Recent);

                for (int i = 0; i < inbox.Count; i++)
                {
                    var message = inbox.GetMessage(i);

                    Console.WriteLine("Subject: {0}", message.Subject);

                    messages.Add(message);
                }

                client.Disconnect(true);

                return messages;
            }
        }

        public MimeMessage GetMessage(string host, int port, string login, string password, int messageIndex)
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

        public List<MailMessage> CastToMailMessage(List<MimeMessage> mimeMessages)
        {
            List<MailMessage> messages = new List<MailMessage>(mimeMessages.Count);

            foreach (MimeMessage mimeMessage in mimeMessages)
            {
                    var from = (MailboxAddress)mimeMessage.From[0];
                    var to = (MailboxAddress)mimeMessage.To[0];

                    var mailMessage = new MailMessage(from.Address, to.Address, mimeMessage.Subject, mimeMessage.HtmlBody);

                    messages.Add(mailMessage);
            }

            return messages;
        }
    }
}
