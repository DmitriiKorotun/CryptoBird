using Cryptography;
using CryptoMail.Network.Entities;
using EmailAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Network.Infrastructure
{
    class NetRepresentative
    {
        public void SendPublicKeys(string from, string to, string login, string password, string host, int port)
        {
            string[] firstKeyPair = RSA.GenerateKeyPair(),
                secondKeyPair = RSA.GenerateKeyPair();

            string publicKeys = firstKeyPair[0] + secondKeyPair[0],
                privateKeys = firstKeyPair[1] + secondKeyPair[1];

            IO.WriteAllText("test.xml", privateKeys);

            new MailSender(host, port).Send(from, to, publicKeys, "9B9817CC57EBA9DF0067A197FB7FBE9F", login, password, host, port, false);
        }
    }
}
