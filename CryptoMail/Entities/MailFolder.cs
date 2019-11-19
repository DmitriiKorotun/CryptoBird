using EmailAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Entities
{
    public class MailFolder
    {
        public string Name { get; private set; }
        public MailSpecialFolder FolderType { get; private set; }

        public MailFolder(string name, MailSpecialFolder folderType)
        {
            Name = name;
            FolderType = folderType;
        }
    }
}
