using Cryptography;
using CryptoMail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail
{
    public class MailEncrypter
    {
        public string EncryptMessage(string body, string publicKeys)
        {
            string[] publicKeysArr = SplitKeys(publicKeys);

            DataBlob dataBlob = CreateDataBlob(body);

            CryptoBlob cryptoBlob = EncryptDataBlob(dataBlob, publicKeysArr[0], publicKeysArr[1]);

            byte[] cryptoBlobBytes = cryptoBlob.GetBytes();

            return Convert.ToBase64String(cryptoBlobBytes);
        }

        private string[] SplitKeys(string publicKeys)
        {
            const string rsaKeyEnding = "</RSAKeyValue>";

            string[] publicKeysArr = new string[2];

            int firstKeyLength = publicKeys.IndexOf(rsaKeyEnding) + rsaKeyEnding.Length;

            publicKeysArr[0] = publicKeys.Substring(0, firstKeyLength);

            publicKeysArr[1] = publicKeys.Substring(firstKeyLength, publicKeys.Length - firstKeyLength);

            return publicKeysArr;
        }

        private DataBlob CreateDataBlob(string body)
        {
            var dataBlob = new DataBlob
            {
                Body = Encoding.UTF8.GetBytes(body)
            };

            dataBlob.Hash = MD5Encrypter.MD5Encrypt(dataBlob.Body);

            using(Rijndael rijndael = Rijndael.Create())
            {
                dataBlob.Key = rijndael.Key;
                dataBlob.IV = rijndael.IV;
            }

            return dataBlob;
        }

        private CryptoBlob EncryptDataBlob(DataBlob dataBlob, string firstPublicKey, string secondPublicKey)
        {
            var cryptoBlob = new CryptoBlob(dataBlob)
            {
                EncryptedData = AES.EncryptStringToBytes_Aes(dataBlob.Body, dataBlob.Key, dataBlob.IV),
                EncryptedKey = Cryptography.RSA.Encrypt(dataBlob.Key, firstPublicKey),
                EncryptedHash = Cryptography.RSA.Encrypt(dataBlob.Hash, secondPublicKey)
            };

            return cryptoBlob;
        }
    }
}
