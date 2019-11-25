using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailAgent.Entities
{
    public interface IEmailMessage
    {
        int Index { get; set; }

        string To { get; set; }
        string From { get; set; }
        string Subject { get; set; }
        string Date { get; set; }

        bool HasAttachments { get; set; }
        bool IsRead { get; set; }

        uint UniqueId { get; set; }
        ulong? ModSeq { get; set; }
    }
}
