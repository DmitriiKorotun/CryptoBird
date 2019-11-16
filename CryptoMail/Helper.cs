using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail
{
    public class Helper
    {
        public static byte[] IntToBytes(int integer)
        {
            var bytes = new byte[4];

            bytes[3] = (byte)(integer >> 24);
            bytes[2] = (byte)(integer >> 16);
            bytes[1] = (byte)(integer >> 8);
            bytes[0] = (byte)integer;

            return bytes;
        }
    }
}
