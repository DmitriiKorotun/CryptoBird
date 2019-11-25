using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CryptoMail.Local.Serialization
{
    [Serializable]
    public class SerializableMessage
    {
        public string From { get; set; }
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Uid { get; set; }
        public string Date { get; set; }
        public bool HasAttachments { get; set; }

        public SerializableMessage() { }
        public SerializableMessage(string from, MailAddressCollection to)
        {
            From = from;
            To = EjectAdresses(to);
        }

        public SerializableMessage(MailAddress from, MailAddressCollection to)
        {
            From = from.Address;
            To = EjectAdresses(to);
        }

        public SerializableMessage(string from, MailAddressCollection to, string subject, string body)
        {
            From = from;
            To = EjectAdresses(to);
            Subject = subject;
            Body = body;
        }

        public SerializableMessage(MailAddress from, MailAddressCollection to, string subject, string body)
        {
            From = from.Address;
            To = EjectAdresses(to);
            Subject = subject;
            Body = body;
        }

        public SerializableMessage(string from, MailAddressCollection to, string subject, string body, bool hasAttachments)
        {
            From = from;
            To = EjectAdresses(to);
            Subject = subject;
            Body = body;
            HasAttachments = hasAttachments;
        }

        public SerializableMessage(MailAddress from, MailAddressCollection to, string subject, string body, bool hasAttachments)
        {
            From = from.Address;
            To = EjectAdresses(to);
            Subject = subject;
            Body = body;
            HasAttachments = hasAttachments;
        }

        public static SerializableMessage CreateFromMailMessage(MailMessage message)
        {
            var serializableMessage = new SerializableMessage(message.From, message.To, message.Subject, message.Body);

            serializableMessage.CopyMessageHeaders(message.Headers);

            if (message.Attachments.Count > 0)
                serializableMessage.HasAttachments = true;

            return serializableMessage;
        }

        private List<string> EjectAdresses(MailAddressCollection addressesCollection)
        {
            List<string> addresses = new List<string>(addressesCollection.Count);

            foreach (MailAddress address in addressesCollection)
            {
                addresses.Add(address.Address);
            }

            return addresses;
        }

        private void CopyMessageHeaders(System.Collections.Specialized.NameValueCollection headers)
        {
            if (!string.IsNullOrEmpty(headers["Uid"]))
                this.Uid = headers["Uid"];

            if (!string.IsNullOrEmpty(headers["Uid"]))
                this.Date = headers["Date"];
        }
    }
}