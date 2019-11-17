using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Entities
{
    abstract class Blob
    {
        public abstract byte[] GetBytes();

        protected byte[] Append(byte[] origin, byte[] toAppend)
        {
            var result = new byte[origin.Length + toAppend.Length];

            origin.CopyTo(result, 0);
            toAppend.CopyTo(result, origin.Length);

            return result;
        }
    }
}
