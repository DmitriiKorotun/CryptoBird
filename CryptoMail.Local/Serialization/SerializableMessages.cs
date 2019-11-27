using EmailAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Local.Serialization
{
    public class SerializableMessages
    {
        public List<SerializableMessage> Messages { get; set; }
        public MailSpecialFolder Folder { get; set; } 

        public SerializableMessages()
        {
            Messages = new List<SerializableMessage>();
        }

        public SerializableMessages(MailSpecialFolder folder)
        {
            Messages = new List<SerializableMessage>();

            Folder = folder;
        }

        public SerializableMessages(List<MailMessage> messages)
        {
            Messages = CastToSerializableMessages(messages);
        }

        public SerializableMessages(List<SerializableMessage> messages)
        {
            Messages = messages;
        }

        public SerializableMessages(List<MailMessage> messages, MailSpecialFolder folder)
        {
            Messages = CastToSerializableMessages(messages);

            Folder = folder;
        }

        private static List<SerializableMessage> CastToSerializableMessages(List<MailMessage> messages)
        {
            var serializableMessages = new List<SerializableMessage>(messages.Count);

            foreach (MailMessage mailMessage in messages)
            {
                serializableMessages.Add(SerializableMessage.CreateFromMailMessage(mailMessage));
            }

            return serializableMessages;
        }

        public static SerializableMessages CreateFromMailMessages(List<MailMessage> messages)
        {
            return new SerializableMessages(CastToSerializableMessages(messages));
        }
    }
}
