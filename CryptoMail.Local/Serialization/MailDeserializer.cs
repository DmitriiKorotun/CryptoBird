using EmailAgent;
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
    public static class MailDeserializer
    {
        public static SerializableMessage DeserializeMessage(string filename)
        {
            SerializableMessage message;

            XmlSerializer serializer = new XmlSerializer(typeof(SerializableMessage));

            using (StreamReader reader = new StreamReader(filename))
            {
                message = (SerializableMessage)serializer.Deserialize(reader);

                reader.Close();
            }

            return message;
        }

        public static SerializableMessages DeserializeMessages(string filename)
        {
            SerializableMessages messages;

            XmlSerializer serializer = new XmlSerializer(typeof(SerializableMessages));

            using (StreamReader reader = new StreamReader(filename))
            {
                messages = (SerializableMessages)serializer.Deserialize(reader);

                reader.Close();
            }

            return messages;
        }
    }
}
