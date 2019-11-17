using MailKit;
using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailAgent
{
    public class MailGetter
    {
        public static List<MailMessage> GetAllMessagesTest()
        {
            return GetAllMessages("imap.gmail.com", 993, UserData.Login, UserData.Password);
        }

        public static List<MailMessage> GetAllMessages(string host, int port, string login, string password)
        {
            List<MailMessage> messages;

            using (var client = new ImapClient())
            {
                // For demo-purposes, accept all SSL certificates
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(host, port, true);

                client.Authenticate(login, password);

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                messages = new List<MailMessage>(inbox.Count);

                Console.WriteLine("Total messages: {0}", inbox.Count);
                Console.WriteLine("Recent messages: {0}", inbox.Recent);

                for (int i = 0; i < inbox.Count; i++)
                {
                    var message = inbox.GetMessage(i);
                    Console.WriteLine("Subject: {0}", message.Subject);
                    var from = (MimeKit.MailboxAddress)message.From[0];
                    var to = (MimeKit.MailboxAddress)message.To[0];
                    messages.Add(new MailMessage(from.Address, to.Address, message.Subject, message.HtmlBody));
                }

                client.Disconnect(true);
            }

            return messages;
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

    }
}
