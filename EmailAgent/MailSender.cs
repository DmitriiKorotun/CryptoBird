using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailAgent
{
    public class MailSender
    {
        SmtpClient Smtp { get; set; }
        public string Host {
            get { return Smtp.Host; }
            set { Smtp.Host = value; }
        }
        public int Port
        {
            get { return Smtp.Port; }
            set { Smtp.Port = value; }
        }

        public MailSender(string host, int port) => Smtp = new SmtpClient(host, port);

        public void Send(string from, string to, string body, string subject, string login, string password, string host, int port, bool isBodyHtml = true)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress adressFrom = new MailAddress(from);
            // кому отправляем
            MailAddress adressTo = new MailAddress(to);
            // создаем объект сообщения
            MailMessage m = new MailMessage(adressFrom, adressTo)
            {
                // тема письма
                Subject = subject,
                // текст письма
                Body = body,
                IsBodyHtml = isBodyHtml
            };
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient(host, port)
            {
                // логин и пароль
                Credentials = new NetworkCredential(login, password),
                EnableSsl = true
            };
            smtp.Send(m);
        }

        public void Send(MailMessage message, string login, string password, string host, int port)
        {
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient(host, port)
            {
                // логин и пароль
                Credentials = new NetworkCredential(login, password),
                EnableSsl = true
            };
            smtp.Send(message);
        }

        private void SetCredentials(string login, string password)
        {
            Smtp.Credentials = new NetworkCredential(login, password);
        }

        private void SendEmail(string from, string to, string body, string subject, string login, string password)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress adressFrom = new MailAddress(from);
            // кому отправляем
            MailAddress adressTo = new MailAddress(to);

            MailMessage m = new MailMessage(from, to)
            {
                // тема письма
                Subject = "Тест",
                // текст письма
                Body = "<h2>Письмо-тест работы smtp-клиента</h2>"
            };

            SetCredentials(login, password);

            Smtp.Send(m);
        }
    }
}
