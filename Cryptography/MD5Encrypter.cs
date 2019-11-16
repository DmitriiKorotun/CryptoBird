using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Cryptography
{
    public static class MD5Encrypter
    {
        public static byte[] MD5Encrypt(string data)
        {
            try
            {
                using (MD5 md5 = MD5.Create())
                {
                    var hash = md5.ComputeHash(Encoding.Default.GetBytes(data));

                    return hash;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);

                throw e;
            }
        }

        public static byte[] MD5Encrypt(byte[] data)
        {
            try
            {
                using (MD5 md5 = MD5.Create())
                {
                    var hash = md5.ComputeHash(data);

                    return hash;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);

                throw e;
            }
        }
    }
}

