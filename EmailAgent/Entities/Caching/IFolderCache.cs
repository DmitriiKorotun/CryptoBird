using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailAgent.Entities.Caching
{
    public interface IFolderCache
    {
        void Add(IMessageSummary item);
        void Update(IMessageSummary item);
        void Update(int index, MessageFlags flags, ulong? modSeq);
        void Clear();
        void Remove(UniqueId uid);
        void Remove(int index);
        void RemoveRange(IList<UniqueId> uids);
        ulong GetHighestKnownModSeq();
        uint GetUidValidity();
        void SetUidValidity(uint uidValidity);
        IList<UniqueId> GetKnownUids();
    }
}
