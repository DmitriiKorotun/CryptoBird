using CryptoMail.Local.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Local
{
    class MessageIO : IMessageIO
    {
        public MailMessage LoadMessage(string filename)
        {
            MailMessage message;

            var deserializedMessage = MailDeserializer.DeserializeMessage(filename);

            message = CastToMailMessage(deserializedMessage);

            return message;
        }

        public List<MailMessage> LoadMessages(string filename)
        {
            List<MailMessage> messages;

            var deserializedMessages = MailDeserializer.DeserializeMessages(filename);

            messages = CastToMailMessages(deserializedMessages);

            return messages;
        }

        public void SaveMessage(MailMessage message, string filename)
        {
            var serializableMessage = SerializableMessage.CreateFromMailMessage(message);

            MailSerializer.SaveMessage(serializableMessage, filename);
        }

        public void SaveMessages(List<MailMessage> messages, string filename)
        {
            var serializableMessages = SerializableMessages.CreateFromMailMessages(messages);

            MailSerializer.SaveMessages(serializableMessages, filename);
        }

        private MailMessage CastToMailMessage(SerializableMessage serializableMessage)
        {
            MailMessage message = new MailMessage(serializableMessage.From, serializableMessage.To[0], serializableMessage.Subject, serializableMessage.Body);

            if (!string.IsNullOrEmpty(serializableMessage.Uid))
                message.Headers["Uid"] = serializableMessage.Uid;

            if (!string.IsNullOrEmpty(serializableMessage.Date))
                message.Headers["Date"] = serializableMessage.Date;

            if (serializableMessage.HasAttachments)
                message.Attachments.Add(new Attachment("null"));

            return message;
        }

        private List<MailMessage> CastToMailMessages(SerializableMessages serializableMessages)
        {
            List<MailMessage> messages = new List<MailMessage>(serializableMessages.Messages.Count);

            foreach (SerializableMessage serializableMessage in serializableMessages.Messages)
            {
                var message = CastToMailMessage(serializableMessage);

                messages.Add(message);
            }

            return messages;
        }
    }
}
