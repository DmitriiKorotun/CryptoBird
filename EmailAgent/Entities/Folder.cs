using EmailAgent.Entities.Caching;
using MailKit;
using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailAgent.Entities
{
    public class Folder
    {
        public IFolderCache FolderCache { get; set; }

        public MailSpecialFolder FolderType { get; private set; }

        public Folder(MailSpecialFolder folderType)
        {
            FolderCache = new FolderCache(0, folderType.ToString());

            FolderType = folderType;
        }

        public Folder(MailSpecialFolder folderType, IFolderCache folderCache)
        {
            FolderCache = folderCache;

            FolderType = folderType;
        }

        public List<IEmailMessage> GetMessages()
        {
            return FolderCache.GetMessagesList();
        }

        public void UpdateFolder(string host, int port, string login, string password)
        {
            using (var client = new ImapClient())
            {
                // For demo-purposes, accept all SSL certificates
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(host, port, true);

                client.Authenticate(login, password);

                var mailFolder = GetMailFolder(client, FolderType);

                Sync(FolderCache, mailFolder, client);

                client.Disconnect(true);

            }
        }

        private void Sync(IFolderCache cache, IMailFolder folder, ImapClient client)
        {
            uint uidValidity = cache.GetUidValidity();
            var known = cache.GetKnownUids();
            UniqueIdSet missing;

            folder.MessageFlagsChanged += OnMessageFlagsChanged;

            if (client.Capabilities.HasFlag(ImapCapabilities.QuickResync))
                missing = GetMissingWithQResync(cache, folder);
            else
                missing = GetMissingWithCondstore(cache, folder, client.Capabilities);

            // fetch the summary information for the messages we are missing
            var fields = MessageSummaryItems.Full | MessageSummaryItems.UniqueId;

            if (client.Capabilities.HasFlag(ImapCapabilities.CondStore))
                fields |= MessageSummaryItems.ModSeq;

            var newMessages = folder.Fetch(missing, fields);
            foreach (var message in newMessages)
                cache.Add(message);

            cache.SetUidValidity(folder.UidValidity);
        }

        private UniqueIdSet GetMissingWithQResync(IFolderCache cache, IMailFolder folder)
        {
            uint uidValidity = cache.GetUidValidity();
            var known = cache.GetKnownUids();
            UniqueIdSet missing;

            var highestModSeq = cache.GetHighestKnownModSeq();

            folder.MessagesVanished += OnMessagesVanished;

            // This version of the Open() method will emit MessagesVanished and MessageFlagsChanged
            // for all messages that have been expunged or have changed since the last session.
            folder.Open(FolderAccess.ReadWrite, uidValidity, highestModSeq, known);

            if (folder.UidValidity != uidValidity)
            {
                // our cache is no longer valid, we'll need to start over from scratch
                cache.Clear();

                missing = new UniqueIdSet(folder.Search(MailKit.Search.SearchQuery.All));
            }
            else
            {
                // figure out which messages we are missing in our cache
                missing = new UniqueIdSet(MailKit.Search.SortOrder.Ascending);
                var all = folder.Search(MailKit.Search.SearchQuery.All);
                foreach (var uid in all)
                {
                    if (!known.Contains(uid))
                        missing.Add(uid);
                }
            }

            return missing;
        }

        private UniqueIdSet GetMissingWithCondstore(IFolderCache cache, IMailFolder folder, ImapCapabilities capabilities)
        {
            uint uidValidity = cache.GetUidValidity();
            var known = cache.GetKnownUids();
            UniqueIdSet missing;

            folder.MessageExpunged += OnMessageExpunged;
            folder.Open(FolderAccess.ReadWrite);

            if (folder.UidValidity != uidValidity)
            {
                // our cache is no longer valid, we'll need to start over from scratch
                cache.Clear();

                missing = new UniqueIdSet(folder.Search(MailKit.Search.SearchQuery.All));
            }
            else
            {
                var all = folder.Search(MailKit.Search.SearchQuery.All);

                // purge messages from our cache that have been purged on the remote IMAP server
                foreach (var uid in known)
                {
                    if (!all.Contains(uid))
                        cache.Remove(uid);
                }

                // sync flag changes since our last session
                known = cache.GetKnownUids();
                if (known.Count > 0)
                {
                    IList<IMessageSummary> changed;
                    if (capabilities.HasFlag(ImapCapabilities.CondStore))
                    {
                        changed = folder.Fetch(known, cache.GetHighestKnownModSeq(), MessageSummaryItems.Flags | MessageSummaryItems.ModSeq | MessageSummaryItems.UniqueId);
                    }
                    else
                    {
                        changed = folder.Fetch(known, MessageSummaryItems.Flags | MessageSummaryItems.UniqueId);
                    }

                    foreach (var item in changed)
                    {
                        // update the cache for this message
                        cache.Update(item);
                    }
                }

                // figure out which messages we are missing in our cache
                missing = new UniqueIdSet(MailKit.Search.SortOrder.Ascending);
                foreach (var uid in all)
                {
                    if (!known.Contains(uid))
                        missing.Add(uid);
                }
            }

            return missing;
        }

        void OnMessageFlagsChanged(object sender, MessageFlagsChangedEventArgs e)
        {
            FolderCache.Update(e.Index, e.Flags, e.ModSeq);
        }

        void OnMessageExpunged(object sender, MessageEventArgs e)
        {
            FolderCache.Remove(e.Index);
        }

        void OnMessagesVanished(object sender, MessagesVanishedEventArgs e)
        {
            FolderCache.RemoveRange(e.UniqueIds);
        }

        private static IMailFolder GetMailFolder(ImapClient client, MailSpecialFolder folderToFind)
        {
            IMailFolder folder;

            switch (folderToFind)
            {
                case MailSpecialFolder.Inbox:
                    folder = client.Inbox;
                    break;
                case MailSpecialFolder.Sent:
                    folder = client.GetFolder(SpecialFolder.Sent);
                    break;
                case MailSpecialFolder.Drafts:
                    folder = client.GetFolder(SpecialFolder.Drafts);
                    break;
                case MailSpecialFolder.Trash:
                    folder = client.GetFolder(SpecialFolder.Trash);
                    break;
                default:
                    folder = null;
                    break;
            }

            return folder;
        }
    }
}
