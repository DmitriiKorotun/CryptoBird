using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Network.Infrastructure
{
    class IO
    {
        public static void WriteAllBytes(string filename, byte[] bytes)
        {
            System.IO.File.WriteAllBytes(filename, bytes);
        }

        public static void WriteAllText(string filename, string text)
        {
            System.IO.File.WriteAllText(filename, text);
        }
    }
}
