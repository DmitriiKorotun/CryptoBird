using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Entities
{
    class DataBlob : Blob
    {
        public byte[] Body { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }

        public DataBlob() { }
        public DataBlob(byte[] body)
        {
            Body = body;
        }

        public DataBlob(byte[] body, byte[] hash)
        {
            Body = body;
            Hash = hash;
        }

        public DataBlob(byte[] body, byte[] key, byte[] iv)
        {
            Body = body;
            Key = key;
            IV = iv;
        }

        public DataBlob(byte[] body, byte[] hash, byte[] key, byte[] iv)
        {
            Body = body;
            Hash = hash;
            Key = key;
            IV = iv;
        }

        public override byte[] GetBytes()
        {
            throw new NotImplementedException();
        }
    }
}
