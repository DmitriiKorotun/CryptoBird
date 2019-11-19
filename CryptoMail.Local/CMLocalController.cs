using EmailAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Local
{
    public static class CMLocalController
    {
        public static void SaveMessages(List<MailMessage> messages, MailSpecialFolder folder, string currentUser)
        {
            string dirname = CreateMessagesDir(folder, currentUser),
                filename = GenerateFilename("Messages", dirname);

            IMessageIO messageIO = new MessageIO();

            messageIO.SaveMessages(messages, filename);
        }

        public static List<MailMessage> LoadMessages(MailSpecialFolder folder, string currentUser)
        {
            string dirname = CreateMessagesDir(folder, currentUser),
                filename = GenerateFilename("Messages", dirname);

            IMessageIO messageIO = new MessageIO();

            return messageIO.LoadMessages(filename);
        }


        private static string GenerateFilename(MailMessage message, string dirName)
        {
            string filename = dirName + "/" + message.Headers["Uid"];

            if (filename.Contains('<'))
                filename = filename.Replace('<', '-');
            if (filename.Contains('>'))
                filename = filename.Replace('>', '-');

            return filename;
        }

        private static string GenerateFilename(string filename, string dirName)
        {
            string complexName = dirName + "/" + filename;

            return complexName;
        }

        private static string CreateMessagesDir(MailMessage message, string currentUser)
        {
            string directory;

            directory = currentUser;
            System.IO.Directory.CreateDirectory(directory);

            directory += '/' + message.From.Address;
            System.IO.Directory.CreateDirectory(directory);

            return directory;
        }

        private static string CreateMessagesDir(MailSpecialFolder folder, string currentUser)
        {
            string directory;

            directory = currentUser;
            System.IO.Directory.CreateDirectory(directory);

            directory += '/' + folder.ToString();
            System.IO.Directory.CreateDirectory(directory);

            return directory;
        }
    }
}
