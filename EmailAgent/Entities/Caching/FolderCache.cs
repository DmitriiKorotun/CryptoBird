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

        public FolderCache(uint uidValidity)
        {
            Cache = MemoryCache.Default;

            HighestKnownModSeq = 0;

            UidValidity = uidValidity;
        }

        public FolderCache(List<KeyValuePair<string, object>> messages, uint uidValidity, ulong highestKnownModSeq)
        {
            Cache = MemoryCache.Default;

            Cache.Concat(messages);

            HighestKnownModSeq = highestKnownModSeq;

            UidValidity = uidValidity;
        }

        public void Add(IMessageSummary item)
        {
            var cacheItem = new CacheItem(item.UniqueId.ToString(), item);

            CacheItemPolicy policy = new CacheItemPolicy();

            Cache.Add(cacheItem, policy);

            UpdateHighestKnownModSeq(item.ModSeq);
        }

        private void UpdateHighestKnownModSeq(ulong? modSeq)
        {
            var messageModSeq = (ulong)modSeq;

            HighestKnownModSeq = HighestKnownModSeq < messageModSeq ? messageModSeq : HighestKnownModSeq;
        }

        public void Update(IMessageSummary item)
        {
            var key = item.UniqueId.ToString();

            if (Cache.Contains(key))
                Cache[key] = item;

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
            Cache.Remove(uid.ToString());
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
            Cache = MemoryCache.Default;
        }

        public IList<UniqueId> GetKnownUids()
        {
            var idList = new List<UniqueId>(Cache.Count());

            foreach (KeyValuePair<string, object> entry in Cache)
            {
                if (entry.Value is IMessageSummary message)
                    idList.Add(message.UniqueId);
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
