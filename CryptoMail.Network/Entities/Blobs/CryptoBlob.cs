using CryptoMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Network.Entities.Blobs
{
    class CryptoBlob : Blob
    {
        private const int keyLengthSize = 4;
        private const int IVLengthSize = 4;
        private const int hashLengthSize = 4;

        public byte[] EncryptedData { get; set; }
        public byte[] EncryptedHash { get; set; }
        public byte[] EncryptedKey { get; set; }
        public byte[] IV { get; set; }

        public CryptoBlob() { }
        public CryptoBlob(byte[] iv)
        {
            IV = iv;
        }

        public CryptoBlob(DataBlob dataBlob)
        {
            IV = dataBlob.IV;
        }

        public override byte[] GetBytes()
        {
            //byte[] cryptoBlob = new byte[EncryptedKey.Length + IV.Length + EncryptedHash.Length + EncryptedData.Length];
            byte[] cryptoBlob = Append(EncryptedHash, EncryptedData);
            cryptoBlob = Append(IV, cryptoBlob);
            cryptoBlob = Append(EncryptedKey, cryptoBlob);
            cryptoBlob = Append(Helper.IntToBytes(EncryptedHash.Length), cryptoBlob);
            cryptoBlob = Append(Helper.IntToBytes(IV.Length), cryptoBlob);
            cryptoBlob = Append(Helper.IntToBytes(EncryptedKey.Length), cryptoBlob);

            return cryptoBlob;
        }
    }
}
