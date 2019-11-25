using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Local.Serialization
{
    [Serializable]
    public class SerializableMessageSummary
    {
        public DateTimeOffset Date { get; set; }

        public string From { get; set; }

        public string Subject { get; set; }

        public int Index { get; set; }

        public UniqueId UniqueId { get; set; }

        public ulong? ModSeq { get; set; }

        public SerializableMessageSummary() { }

        public static SerializableMessageSummary CreateFromIMessageSummary(IMessageSummary message)
        {
            SerializableMessageSummary serializableMessageSummary = new SerializableMessageSummary();

            if (message.Date != null)
                serializableMessageSummary.Date = message.Date;

            if (message.Envelope.From[0] is MailboxAddress mailboxAdress)
                serializableMessageSummary.From = mailboxAdress.Address;

            if (!string.IsNullOrEmpty(message.Envelope.Subject))
                serializableMessageSummary.Subject = message.Envelope.Subject;

            if (message.UniqueId != null)
                serializableMessageSummary.UniqueId = message.UniqueId;

            if (message.ModSeq is ulong modSeq)
                serializableMessageSummary.ModSeq = modSeq;

            serializableMessageSummary.Index = message.Index;

            return serializableMessageSummary;
        }
    }
}
