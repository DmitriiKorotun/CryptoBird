using EmailAgent.Entities;
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

        public List<SerializableKeyValuePair<string, SerializableEmailMessage>> Messages { get; set; }
        public ulong HighestKnownModSeq { get; set; }
        public uint UidValidity { get; set; }
        public string CacheName { get; set; }

        public SerializableFolderCache() { }

        public SerializableFolderCache(List<SerializableKeyValuePair<string, SerializableEmailMessage>> messages)
        {
            Messages = messages;
        }

        public SerializableFolderCache(List<SerializableKeyValuePair<string, SerializableEmailMessage>> messages, uint uidValidity)
        {
            Messages = messages;

            UidValidity = uidValidity;
        }

        public SerializableFolderCache(List<SerializableKeyValuePair<string, SerializableEmailMessage>> messages, uint uidValidity, ulong highestKnownModSeq)
        {
            Messages = messages;

            UidValidity = uidValidity;

            HighestKnownModSeq = highestKnownModSeq;
        }

        public SerializableFolderCache(List<SerializableKeyValuePair<string, SerializableEmailMessage>> messages, uint uidValidity, ulong highestKnownModSeq, string cacheName)
        {
            Messages = messages;

            UidValidity = uidValidity;

            HighestKnownModSeq = highestKnownModSeq;

            CacheName = cacheName;
        }

        public static SerializableFolderCache CreateFromFolderCache(IFolderCache folderCache, string folderCacheName)
        {          
            List<KeyValuePair<string, object>> messages = folderCache.GetAllMessages();

            var serializableMessages = new List<SerializableKeyValuePair<string, SerializableEmailMessage>>(messages.Count);

            foreach (KeyValuePair<string, object> entry in messages)
            {
                if (entry.Value is IEmailMessage message)
                {
                    //var serializableMessage = IEmailMessage.CreateFromIMessageSummary(message);

                    //serializableMessages.Add(new SerializableKeyValuePair<string, IEmailMessage>(entry.Key, serializableMessage));
                    serializableMessages.Add(new SerializableKeyValuePair<string, SerializableEmailMessage>(entry.Key, new SerializableEmailMessage(message)));
                }
            }

            ulong highestKnownModSeq = folderCache.GetHighestKnownModSeq();

            uint uidValidity = folderCache.GetUidValidity();

            string cacheName = folderCacheName;

            return new SerializableFolderCache(serializableMessages, uidValidity, highestKnownModSeq, cacheName);
        }
    }
}
