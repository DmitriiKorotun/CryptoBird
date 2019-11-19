using CryptoMail.Entities;
using EmailAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Infrastructure
{
    public class CMController
    {
        public void SendMessage(EmailMessageSimplified message, MailTechnicalPassport mailPassport)
        {
            Postman.DeliverMessage(message, mailPassport);
        }

        public void SendMessage(MailMessage message, MailTechnicalPassport mailPassport)
        {
            Postman.DeliverMessage(message, mailPassport);
        }

        public void SendMessage(string from, string to, string body, string subject, MailTechnicalPassport mailPassport)
        {
            var message = new EmailMessageSimplified(body, subject, from, to);

            Postman.DeliverMessage(message, mailPassport);
        }

        public List<MailMessage> GetAllMessages(string login, string password, string host, int port)
        {
            MailTechnicalPassport mailPassport = CreateMailPassport(login, password, host, port);

            var messages = MailGetter.CastToMailMessage(MailGetter.GetAllMessagesTest());

            for (int i = 0; i < messages.Count; ++i)
            {
                var token = new MailParser().SearchForToken(messages[i]);

                //if (token != MailParser.Token.NONE)
                //{
                //    PerformRequest(token, messages[i], mailPassport);

                //    DeleteMessage(mailPassport, i);

                //    messages.RemoveAt(i);

                //    --i;
                //}
            }

            return messages;
        }

        private MailTechnicalPassport CreateMailPassport(string login, string password, string host, int port)
        {
            MailTechnicalPassport mailPassport = new MailTechnicalPassport();

            mailPassport.UserCreditentials.Login = login;
            mailPassport.UserCreditentials.Password = password;
            mailPassport.Provider.Host = host;
            mailPassport.Provider.Port = port;

            return mailPassport;
        }

        private void PerformRequest(MailParser.Token token, MailMessage message, MailTechnicalPassport mailPassport)
        {
            switch(token)
            {
                case MailParser.Token.SEND_PUBLIC_KEYS:
                    SendPublicKeys(message.To[0].Address, message.From.Address, mailPassport);
                    break;
                case MailParser.Token.RECEIVE_PUBLIC_KEYS:
                    break;
            }
        }

        private void SendPublicKeys(string from, string to, MailTechnicalPassport mailPassport)
        {
            var netRepresentative = new NetRepresentative();

            netRepresentative.SendPublicKeys(
                from, to, mailPassport.UserCreditentials.Login, mailPassport.UserCreditentials.Password,
                mailPassport.Provider.Host, mailPassport.Provider.Port
                );
        }

        private void ReceivePublicKeys(MailMessage message)
        {
            string keys = message.Body,
                adress = message.From.Address, filename = adress;

            int filenamePostfix = 0;

            while (System.IO.File.Exists(filename))
                filename = adress + filenamePostfix++.ToString();

            IO.WriteAllText(message.From.Address, keys);
        }

        private void DeleteMessage(MailTechnicalPassport mailPassport, int messageId)
        {
            MailGetter.DeleteMessage(
                mailPassport.Provider.Host, mailPassport.Provider.Port,
                mailPassport.UserCreditentials.Login, mailPassport.UserCreditentials.Password,
                messageId
                );
        }
    }
}
