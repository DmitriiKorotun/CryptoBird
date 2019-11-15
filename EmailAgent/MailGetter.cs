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
        public List<MailMessage> kek(string host, int port, string login, string password)
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
                    var kek = (MimeKit.MailboxAddress)message.From[0];
                    var kek2 = (MimeKit.MailboxAddress)message.To[0];
                    messages.Add(new MailMessage(kek.Address, kek2.Address, message.Subject, message.HtmlBody));
                }

                client.Disconnect(true);
            }

            return messages;
        }
    }
}
