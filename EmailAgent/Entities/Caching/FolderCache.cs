using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace EmailAgent.Entities.Caching
{
    public class FolderCache : IFolderCache
    {
        protected ObjectCache Cache { get; set; }
        protected ulong HighestKnownModSeq { get; set; }
        public uint UidValidity { get; set; }

        public FolderCache(uint uidValidity, string name)
        {
            Cache = new MemoryCache(name);

            HighestKnownModSeq = 0;

            UidValidity = uidValidity;
        }

        public FolderCache(List<KeyValuePair<string, object>> messages, uint uidValidity, ulong highestKnownModSeq, string name)
        {
            Cache = new MemoryCache(name);

            Cache.Concat(messages);

            HighestKnownModSeq = highestKnownModSeq;

            UidValidity = uidValidity;
        }

        public void Add(IMessageSummary item)
        {
            
            IEmailMessage emailMessage = new EmailMessage(item);

            var cacheItem = new CacheItem(item.UniqueId.Id.ToString(), emailMessage);

            CacheItemPolicy policy = new CacheItemPolicy();

            Cache.Add(cacheItem, policy);

            UpdateHighestKnownModSeq(emailMessage.ModSeq);
        }

        private void UpdateHighestKnownModSeq(ulong? modSeq)
        {
            var messageModSeq = (ulong)modSeq;

            HighestKnownModSeq = HighestKnownModSeq < messageModSeq ? messageModSeq : HighestKnownModSeq;
        }

        public void Update(IMessageSummary item)
        {
            var key = item.UniqueId.Id.ToString();

            if (Cache.Contains(key))
            {
                var emailMessage = new EmailMessage(item);

                Cache[key] = emailMessage;
            }            

            UpdateHighestKnownModSeq(item.ModSeq);
        }

        public void Update(int index, MessageFlags flags, ulong? modSeq)
        {
            var messageKey = FindKeyByMessageIndex(index);

            //if (messageObject is IMessageSummary message)
            //{
            //    message.In

            //}
            //    idList.Add(message.UniqueId);
        }

        public void Remove(UniqueId uid)
        {
            Cache.Remove(uid.Id.ToString());
        }

        public void Remove(int index)
        {
            var key = FindKeyByMessageIndex(index);

            if (!string.IsNullOrEmpty(key))
                Cache.Remove(key);
        }

        private string FindKeyByMessageIndex(int index)
        {
            string key = null;

            foreach (KeyValuePair<string, object> entry in Cache)
            {
                if (entry.Value is IMessageSummary message)
                {
                    if (message.Index == index)
                    {
                        key = message.UniqueId.ToString();

                        break;
                    }
                }
            }

            return key;
        } 

        public void RemoveRange(IList<UniqueId> uids)
        {
            foreach (UniqueId uid in uids)
                Remove(uid);
        }

        public void Clear()
        {
            foreach (KeyValuePair<string, object> entry in Cache)
                Cache.Remove(entry.Key);         
        }

        public IList<UniqueId> GetKnownUids()
        {
            var idList = new List<UniqueId>(Cache.Count());

            foreach (KeyValuePair<string, object> entry in Cache)
            {
                if (entry.Value is IEmailMessage message)
                {
                    idList.Add(new UniqueId(message.UniqueId));
                }
            }

            return idList;
        }

        public ulong GetHighestKnownModSeq()
        {
            return HighestKnownModSeq;
        }

        public uint GetUidValidity()
        {
            return UidValidity;
        }

        public void SetUidValidity(uint uidValidity)
        {
            UidValidity = uidValidity;
        }

        public List<KeyValuePair<string, object>> GetAllMessages()
        {
            return Cache.ToList();
        }

        public List<IEmailMessage> GetMessagesList()
        {
            List<IEmailMessage> messages = new List<IEmailMessage>(Cache.Count());

            foreach (var item in Cache)
            {
                if (item.Value is IEmailMessage message)
                    messages.Add(message);
            }

            return messages;
        }

        //private void btnGet_Click(object sender, EventArgs e)
        //{
        //    ObjectCache cache = MemoryCache.Default;
        //    string fileContents = cache["filecontents"] as string;

        //    if (fileContents == null)
        //    {
        //        CacheItemPolicy policy = new CacheItemPolicy();

        //        List<string> filePaths = new List<string>();
        //        filePaths.Add("c:\\cache\\example.txt");

        //        policy.ChangeMonitors.Add(new
        //        HostFileChangeMonitor(filePaths));

        //        // Fetch the file contents.  
        //        fileContents =
        //            File.ReadAllText("c:\\cache\\example.txt");

        //        cache.Set("filecontents", fileContents, policy);
        //    }

        //    Label1.Text = fileContents;
        //}
    }
}
