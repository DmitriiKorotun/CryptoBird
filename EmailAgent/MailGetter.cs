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
        public List<MailMessage> GetAllMessagesTest()
        {
            var messagesTask = GetAllMessagesAsync("imap.gmail.com", 993, UserData.Login, UserData.Password);

            return messagesTask.Result;
            //return GetAllMessages("imap.gmail.com", 993, UserData.Login, UserData.Password);
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

        //        var result = GetAllMessagesAsync(inbox); // new List<MailMessage>(inbox.Count);

        //        Console.WriteLine("Total messages: {0}", inbox.Count);
        //        Console.WriteLine("Recent messages: {0}", inbox.Recent);

        //        for (int i = 0; i < inbox.Count; i++)
        //        {
        //            var message = inbox.GetMessage(i);
        //            Console.WriteLine("Subject: {0}", message.Subject);
        //            var kek = (MimeKit.MailboxAddress)message.From[0];
        //            var kek2 = (MimeKit.MailboxAddress)message.To[0];
        //            messages.Add(new MailMessage(kek.Address, kek2.Address, message.Subject, message.HtmlBody));
        //        }
        //        messages = result.Result;
        //        client.Disconnect(true);
        //    }

        //    return messages;
        //}

        public async Task<List<MailMessage>> GetAllMessagesAsync(string host, int port, string login, string password)
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

                messages = await GetAllMessagesAsync(inbox); // new List<MailMessage>(inbox.Count);

                client.Disconnect(true);
            }

            return messages;
        }

        private async Task<List<MailMessage>> GetAllMessagesAsync(IMailFolder folder)
        {
            List<MailMessage> messages = new List<MailMessage>(folder.Count);

            Console.WriteLine("Total messages: {0}", folder.Count);
            Console.WriteLine("Recent messages: {0}", folder.Recent);

            await Task.Run(() =>
            {
                for (int i = 0; i < folder.Count; i++)
                {
                    var message = folder.GetMessage(i);
                    Console.WriteLine("Subject: {0}", message.Subject);
                    var kek = (MimeKit.MailboxAddress)message.From[0];
                    var kek2 = (MimeKit.MailboxAddress)message.To[0];
                    messages.Add(new MailMessage(kek.Address, kek2.Address, message.Subject, message.HtmlBody));
                }
            });

            return messages;
        }
    }
}
