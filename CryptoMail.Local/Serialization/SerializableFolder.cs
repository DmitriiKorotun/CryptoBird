using EmailAgent;
using EmailAgent.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Local.Serialization
{
    [Serializable]
    public class SerializableFolder
    {
        public SerializableFolderCache FolderCache { get; set; }

        public MailSpecialFolder FolderType { get; set; }

        public SerializableFolder() { }

        public static SerializableFolder CreateFromFolder(Folder folder)
        {
            SerializableFolder serialazableFolder = new SerializableFolder
            {
                FolderCache = SerializableFolderCache.CreateFromFolderCache(folder.FolderCache),

                FolderType = folder.FolderType
            };

            return serialazableFolder;
        }
    }
}
