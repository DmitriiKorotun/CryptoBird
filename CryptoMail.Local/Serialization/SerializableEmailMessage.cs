using EmailAgent.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Local.Serialization
{
    public class SerializableEmailMessage : IEmailMessage
    {
        public int Index { get; set; }
        public string To { get; set; }
        public string From { get; set; } 
        public string Subject { get; set; } 
        public string Date { get; set; } 
        public bool HasAttachments { get; set; } 
        public bool IsRead { get; set; } 
        public uint UniqueId { get; set; } 
        public ulong? ModSeq { get; set; } 

        public SerializableEmailMessage() { }

        public SerializableEmailMessage(int index)
        {
            Index = index;
        }

        public SerializableEmailMessage(IEmailMessage message)
        {
            Index = message.Index;
            To = message.To;
            From = message.From;
            Subject = message.Subject;
            Date = message.Date;
            HasAttachments = message.HasAttachments;
            IsRead = message.HasAttachments;
            UniqueId = message.UniqueId;
            ModSeq = message.ModSeq;
        }
    }
}
