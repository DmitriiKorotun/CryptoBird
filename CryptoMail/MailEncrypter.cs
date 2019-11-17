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

        public string DecryptMessage(string body, string privateKeys)
        {           
            string[] privateKeysArr = SplitKeys(privateKeys);

            byte[] encryptedBlob = Convert.FromBase64String(body);

            CryptoBlob cryptoBlob = ParseEncryptedData(encryptedBlob);

            DataBlob dataBlob = DecryptCryptoBlob(cryptoBlob, privateKeysArr[0], privateKeysArr[1]);

            return Encoding.UTF8.GetString(dataBlob.Body);
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

        private CryptoBlob ParseEncryptedData(byte[] encryptedData)
        {
            CryptoBlob cryptoBlob = new CryptoBlob();

            int offset = 0;

            int keyLength = BitConverter.ToInt32(ReadEncryptedBlob(encryptedData, ref offset, 4), 0),
                IVLength = BitConverter.ToInt32(ReadEncryptedBlob(encryptedData, ref offset, 4), 0),
                hashLength = BitConverter.ToInt32(ReadEncryptedBlob(encryptedData, ref offset, 4), 0);

            cryptoBlob.EncryptedKey = ReadEncryptedBlob(encryptedData, ref offset, keyLength);
            cryptoBlob.IV = ReadEncryptedBlob(encryptedData, ref offset, IVLength);
            cryptoBlob.EncryptedHash = ReadEncryptedBlob(encryptedData, ref offset, hashLength);
            cryptoBlob.EncryptedData = ReadEncryptedBlob(encryptedData, ref offset, encryptedData.Length - offset);

            return cryptoBlob;
        }

        private DataBlob DecryptCryptoBlob(CryptoBlob cryptoBlob, string firstPrivateKey, string secondPrivateKey)
        {
            var dataBlob = new DataBlob(cryptoBlob)
            {
                Key = Cryptography.RSA.Decrypt(cryptoBlob.EncryptedKey, firstPrivateKey),
                Hash = Cryptography.RSA.Decrypt(cryptoBlob.EncryptedHash, secondPrivateKey)
            };
            dataBlob.Body = AES.DecryptStringFromBytes_Aes(cryptoBlob.EncryptedData, dataBlob.Key, dataBlob.IV);

            return dataBlob;
        }

        private byte[] ReadEncryptedBlob(byte[] dataEncrypted, ref int offset, int arrLength)
        {
            byte[] arr = new byte[arrLength];

            Array.Copy(dataEncrypted, offset, arr, 0, arr.Length);

            offset += arr.Length;

            return arr;
        }
    }
}
