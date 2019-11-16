using System;
using System.Security.Cryptography;

namespace Cryptography
{
    public class RSA
    {
        public byte[] Encrypt(byte[] dataToEncrypt, out string rsaXmlString)
        {
            byte[] encryptedData = null;

            try
            {
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    encryptedData = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);

                    rsaXmlString = RSA.ToXmlString(true);

                    return encryptedData;
                }
            }
            catch (ArgumentNullException)
            {
                throw new Exception("Encryption failed.");
            }
        }

        public byte[] Encrypt(byte[] dataToEncrypt, string rsaXmlPublicKey)
        {
            byte[] encryptedData = null;

            try
            {
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.FromXmlString(rsaXmlPublicKey);

                    encryptedData = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);

                    return encryptedData;
                }
            }
            catch (ArgumentNullException)
            {
                throw new Exception("Encryption failed.");
            }
        }

        public byte[] Decrypt(byte[] dataToDecrypt, string rsaXmlParams)
        {
            byte[] decryptedData = null;

            try
            {
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.FromXmlString(rsaXmlParams);
                    //Pass the data to DECRYPT, the private key information 
                    //(using RSACryptoServiceProvider.ExportParameters(true),
                    //and a boolean flag specifying no OAEP padding.
                    decryptedData = RSADecrypt(dataToDecrypt, RSA.ExportParameters(true), false);

                    return decryptedData;
                }
            }
            catch (ArgumentNullException)
            {
                throw new Exception("Decryption failed.");
            }
        }

        private byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This only needs
                    //toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);
                    //Encrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }

        }

        private byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }

        //private string GetPublicKey()
        //{
        //    RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

        //    return RSA.ToXmlString(false);
        //}

        //private string GetPrivateKey()
        //{
        //    RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

        //    return RSA.ToXmlString(true);
        //}

        public string[] GenerateKeyPair()
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            string[] keyPair = new string[] { RSA.ToXmlString(false), RSA.ToXmlString(true) };

            return keyPair;
        }

        private RSACryptoServiceProvider GenKey_SaveInContainer(string ContainerName)
        {
            // Create the CspParameters object and set the key container   
            // name used to store the RSA key pair.  
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = ContainerName;

            // Create a new instance of RSACryptoServiceProvider that accesses  
            // the key container MyKeyContainerName.  
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp);

            return rsa;
        }

        private RSACryptoServiceProvider GetKeyFromContainer(string ContainerName)
        {
            // Create the CspParameters object and set the key container   
            // name used to store the RSA key pair.  
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = ContainerName;

            // Create a new instance of RSACryptoServiceProvider that accesses  
            // the key container MyKeyContainerName.  
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp);

            return rsa;
        }

        private void DeleteKeyFromContainer(string ContainerName)
        {
            // Create the CspParameters object and set the key container   
            // name used to store the RSA key pair.  
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = ContainerName;

            // Create a new instance of RSACryptoServiceProvider that accesses  
            // the key container.  
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp);

            // Delete the key entry in the container.  
            rsa.PersistKeyInCsp = false;

            // Call Clear to release resources and delete the key from the container.  
            rsa.Clear();

            Console.WriteLine("Key deleted.");
        }

    }
}

