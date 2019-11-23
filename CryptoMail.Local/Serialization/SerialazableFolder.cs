using EmailAgent.Entities.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Local.Serialization
{
    [Serializable]
    public class SerialazableFolder
    {
        public List<KeyValuePair<string, object>> Messages { get; set; }
        public ulong HighestKnownModSeq { get; set; }
        public uint UidValidity { get; set; }

        public SerialazableFolder() { }

        public SerialazableFolder(List<KeyValuePair<string, object>> messages)
        {
            Messages = messages;
        }

        public SerialazableFolder(List<KeyValuePair<string, object>> messages, uint uidValidity)
        {
            Messages = messages;

            UidValidity = uidValidity;
        }

        public SerialazableFolder(List<KeyValuePair<string, object>> messages, uint uidValidity, ulong highestKnownModSeq)
        {
            Messages = messages;

            UidValidity = uidValidity;

            HighestKnownModSeq = highestKnownModSeq;
        }

        public static SerialazableFolder CreateFromFolderCache(FolderCache folderCache)
        {
            List<KeyValuePair<string, object>> messages = folderCache.GetAllMessages();

            ulong highestKnownModSeq = folderCache.GetHighestKnownModSeq();

            uint uidValidity = folderCache.GetUidValidity();

            return new SerialazableFolder(messages, uidValidity, highestKnownModSeq);
        }
    }
}
