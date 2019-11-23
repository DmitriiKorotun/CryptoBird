using CryptoMail.Local.Serialization;
using EmailAgent.Entities;
using EmailAgent.Entities.Caching;
using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static CryptoMail.Local.Serialization.SerializableFolderCache;

namespace CryptoMail.Local
{
    class MessageIO : IMessageIO
    {
        public MailMessage LoadMessage(string filename)
        {
            MailMessage message;

            var deserializedMessage = MailDeserializer.DeserializeMessage(filename);

            message = CastToMailMessage(deserializedMessage);

            return message;
        }

        public List<MailMessage> LoadMessages(string filename)
        {
            List<MailMessage> messages;

            var deserializedMessages = MailDeserializer.DeserializeMessages(filename);

            messages = CastToMailMessages(deserializedMessages);

            return messages;
        }

        public Folder LoadFolder(string filename)
        {
            Folder folder;

            var deserializedFolder = MailDeserializer.DeserializeFolder(filename);

            folder = CastToFolder(deserializedFolder);

            return folder;
        }

        public void SaveMessage(MailMessage message, string filename)
        {
            var serializableMessage = SerializableMessage.CreateFromMailMessage(message);

            MailSerializer.SaveMessage(serializableMessage, filename);
        }

        public void SaveMessages(List<MailMessage> messages, string filename)
        {
            var serializableMessages = SerializableMessages.CreateFromMailMessages(messages);

            MailSerializer.SaveMessages(serializableMessages, filename);
        }

        public void SaveFolder(Folder folder, string filename)
        {
            var serializableFolder = SerializableFolder.CreateFromFolder(folder);

            MailSerializer.SaveFolder(serializableFolder, filename);
        }

        private MailMessage CastToMailMessage(SerializableMessage serializableMessage)
        {
            MailMessage message = new MailMessage(serializableMessage.From, serializableMessage.To[0], serializableMessage.Subject, serializableMessage.Body);

            if (!string.IsNullOrEmpty(serializableMessage.Uid))
                message.Headers["Uid"] = serializableMessage.Uid;

            if (!string.IsNullOrEmpty(serializableMessage.Date))
                message.Headers["Date"] = serializableMessage.Date;

            if (serializableMessage.HasAttachments)
                message.Attachments.Add(new Attachment("null"));

            return message;
        }

        private List<MailMessage> CastToMailMessages(SerializableMessages serializableMessages)
        {
            List<MailMessage> messages = new List<MailMessage>(serializableMessages.Messages.Count);

            foreach (SerializableMessage serializableMessage in serializableMessages.Messages)
            {
                var message = CastToMailMessage(serializableMessage);

                messages.Add(message);
            }

            return messages;
        }

        private Folder CastToFolder(SerializableFolder serializableFolder)
        {
            Folder folder;

            IFolderCache folderCache = CastToFolderCache(serializableFolder.FolderCache);

            folder = new Folder(serializableFolder.FolderType, folderCache);

            return folder;
        }

        private IFolderCache CastToFolderCache(SerializableFolderCache serializableFolder)
        {
            FolderCache folder = new FolderCache(
                CastToIMessageListDictionary(serializableFolder.Messages), serializableFolder.UidValidity, serializableFolder.HighestKnownModSeq
                );

            return folder;
        }

        private List<KeyValuePair<string, object>> CastToIMessageListDictionary(List<SerializableKeyValuePair<string, SerializableMessageSummary>> serializableMessages)
        {
            List<KeyValuePair<string, object>> messagesSummary = new List<KeyValuePair<string, object>>(serializableMessages.Count);

            foreach(SerializableKeyValuePair<string, SerializableMessageSummary> serializableMessage in serializableMessages)
            {
                var messageSummary = CastToIMessageSummary(serializableMessage.Value);

                messagesSummary.Add(new KeyValuePair<string, object>(serializableMessage.Key, messageSummary));
            }

            return messagesSummary;
        }

        private IMessageSummary CastToIMessageSummary(SerializableMessageSummary serializableMessage)
        {
            IMessageSummary iMessageSummary = new MessageSummary(serializableMessage.Index);

            if (iMessageSummary is MessageSummary messageSummary)
            {
                messageSummary.Envelope = new Envelope
                {
                    Subject = serializableMessage.Subject
                };

                messageSummary.UniqueId = serializableMessage.UniqueId;
            }

            return iMessageSummary;
        }
    }
}
