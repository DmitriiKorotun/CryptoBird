using CryptoMail.Network.Entities;
using EmailAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Network.Infrastructure
{
    class Postman
    {
        public static void DeliverMessage(EmailMessageSimplified message, MailTechnicalPassport mailPassport)
        {
            var messageSender = new MailSender("smtp.gmail.com", 587);

            messageSender.Send(
                message.From, message.To, message.Body, message.Subject,
                mailPassport.UserCreditentials.Login, mailPassport.UserCreditentials.Password,
                mailPassport.Provider.Host, mailPassport.Provider.Port,
                message.IsHtml);
        }

        public static void DeliverMessage(MailMessage message, MailTechnicalPassport mailPassport)
        {
            var messageSender = new MailSender("smtp.gmail.com", 587);

            messageSender.Send(
                message,
                mailPassport.UserCreditentials.Login, mailPassport.UserCreditentials.Password,
                mailPassport.Provider.Host, mailPassport.Provider.Port);
        }
    }
}
