using EmailAgent;
using EmailAgent.Entities.Caching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CryptoMail.Local.Serialization
{
    public static class MailSerializer
    {
        public static void SaveMessage(SerializableMessage message, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableMessage));

            using (TextWriter writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, message);

                writer.Close();
            }
        }

        public static void SaveMessages(SerializableMessages messages, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableMessages));

            using (TextWriter writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, messages);

                writer.Close();
            }
        }

        public static void SaveFolder(SerializableFolder folder, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableFolder));

            using (TextWriter writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, folder);

                writer.Close();
            }
        }
    }
}
