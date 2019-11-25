using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailAgent.Entities
{
    [Serializable]
    public class EmailMessage : IEmailMessage
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

        public EmailMessage() { }

        public EmailMessage(IMessageSummary messageSummary)
        {
            Index = messageSummary.Index;

            Subject = messageSummary.Envelope.Subject;

            Date = messageSummary.Date.ToLocalTime().ToString();

            UniqueId = messageSummary.UniqueId.Id;

            ModSeq = messageSummary.ModSeq;

            SetTo(messageSummary.Envelope.To);
            SetFrom(messageSummary.Envelope.From);

            foreach (BodyPartBasic bodyPart in messageSummary.BodyParts)
            {
                if (bodyPart.ContentType.MediaType != "TEXT")
                {
                    HasAttachments = true;
                    break;
                }
            }
            
        }

        private void SetTo(InternetAddressList internetAddresses)
        {
            foreach (InternetAddress internetAddress in internetAddresses)
            {
                if (internetAddress is MailboxAddress mailboxAdress)
                    To += mailboxAdress.Address + " ";
            }
        }

        private void SetFrom(InternetAddressList internetAddresses)
        {
            foreach (InternetAddress internetAddress in internetAddresses)
            {
                if (internetAddress is MailboxAddress mailboxAdress)
                    From += mailboxAdress.Address + " ";
            }
        }
    }
}
