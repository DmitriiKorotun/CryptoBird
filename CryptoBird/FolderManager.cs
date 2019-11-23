using CryptoMail.Local;
using EmailAgent;
using EmailAgent.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoBird
{
    static class FolderManager
    {
        public static Folder CreateFolder(MailSpecialFolder folderType)
        {
            var folder = new Folder(folderType);

            return folder;
        }

        public static void UpdateFolder(string login, string password, string host, int port, Folder folder)
        {
            folder.UpdateFolder(host, port, login, password);
        }

        public static Folder LoadFolder(MailSpecialFolder folderType, string currentUser)
        {
            var folder = CMLocalController.LoadFolder(folderType, currentUser);

            return folder;
        }

        public static void SaveFolder(Folder folder, string currentUser)
        {
            CMLocalController.SaveFolder(folder, folder.FolderType, currentUser);
        }
    }
}
