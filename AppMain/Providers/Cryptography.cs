using System;
using System.Security.Cryptography;
using System.Text;

namespace AppMain.Providers
{
    /// <summary>
    /// Provides encryption and decryption funtionalities
    /// </summary>
    public static class Cryptography
    {
        private const string passphrase = "dr0w$$ap";

        /// <summary>
        /// Encrypts plain text to cipher text
        /// </summary>
        /// <param name="plainText">Plain text to encrypt</param>
        /// <returns>Cipher text</returns>
        public static string Encrypt(this string plainText)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5 
            // We use the MD5 hash generator as the result is a 128 bit byte array 
            // which is a valid length for the TripleDES encoder we use below 

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(Encoding.Unicode.GetBytes(passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object 
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder 
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[] 
            byte[] DataToEncrypt = UTF8.GetBytes(plainText);

            // Step 5. Attempt to encrypt the string 
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information 
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string 
            return Convert.ToBase64String(Results);
        }


        /// <summary>
        /// Decrypts a cipher text to plain text
        /// </summary>
        /// <param name="cipherText">Cipher text to decrypt</param>
        /// <returns>Plain text</returns>
        public static string Decrypt(this string cipherText)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5 
            // We use the MD5 hash generator as the result is a 128 bit byte array 
            // which is a valid length for the TripleDES encoder we use below 

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(Encoding.Unicode.GetBytes(passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object 
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder 
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[] 
            byte[] DataToDecrypt = Convert.FromBase64String(cipherText);

            // Step 5. Attempt to decrypt the string 
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information 
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format 
            return UTF8.GetString(Results);
        }

        /// <summary>
        /// Compares a cipher text with a plain text
        /// </summary>
        /// <param name="plainText">Plain text to check</param>
        /// <param name="cipherText">Cipher text</param>
        /// <returns>Whether or not the plain text is equal to the cipher text</returns>
        public static bool IsEqual(this string cipherText, string plainText)
        {
            string encryptedPlainText = plainText.Encrypt();
            return cipherText.Equals(encryptedPlainText);
        }
    }
}
