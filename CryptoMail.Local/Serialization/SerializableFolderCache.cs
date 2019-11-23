using EmailAgent.Entities.Caching;
using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CryptoMail.Local.Serialization
{
    [Serializable]
    public class SerializableFolderCache
    {
        [Serializable]
        [XmlType(TypeName = "SerializableKeyValuePair")]
        public struct SerializableKeyValuePair<K, V>
        {
            public K Key
            { get; set; }

            public V Value
            { get; set; }

            public SerializableKeyValuePair(K key, V value)
            {
                Key = key;

                Value = value;
            }
        }

        public List<SerializableKeyValuePair<string, SerializableMessageSummary>> Messages { get; set; }
        public ulong HighestKnownModSeq { get; set; }
        public uint UidValidity { get; set; }

        public SerializableFolderCache() { }

        public SerializableFolderCache(List<SerializableKeyValuePair<string, SerializableMessageSummary>> messages)
        {
            Messages = messages;
        }

        public SerializableFolderCache(List<SerializableKeyValuePair<string, SerializableMessageSummary>> messages, uint uidValidity)
        {
            Messages = messages;

            UidValidity = uidValidity;
        }

        public SerializableFolderCache(List<SerializableKeyValuePair<string, SerializableMessageSummary>> messages, uint uidValidity, ulong highestKnownModSeq)
        {
            Messages = messages;

            UidValidity = uidValidity;

            HighestKnownModSeq = highestKnownModSeq;
        }

        public static SerializableFolderCache CreateFromFolderCache(IFolderCache folderCache)
        {           
            List<KeyValuePair<string, object>> messages = folderCache.GetAllMessages();

            var serializableMessages = new List<SerializableKeyValuePair<string, SerializableMessageSummary>>(messages.Count);

            foreach (KeyValuePair<string, object> entry in messages)
            {
                if (entry.Value is IMessageSummary message)
                {
                    var serializableMessage = SerializableMessageSummary.CreateFromIMessageSummary(message);

                    serializableMessages.Add(new SerializableKeyValuePair<string, SerializableMessageSummary>(entry.Key, serializableMessage));
                }
            }

            ulong highestKnownModSeq = folderCache.GetHighestKnownModSeq();

            uint uidValidity = folderCache.GetUidValidity();

            return new SerializableFolderCache(serializableMessages, uidValidity, highestKnownModSeq);
        }
    }
}
