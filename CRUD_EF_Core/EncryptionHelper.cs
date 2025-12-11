using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_EF_Core
{
    /// <summary>
    /// Provides methods for encrypting and decrypting text using a simple XOR-based algorithm.
    /// </summary>
    /// <remarks>This class uses a fixed key to perform XOR encryption and decryption. The encrypted text is
    /// encoded as a Base64 string.</remarks>
    public class EncryptionHelper
    {
        private const byte Key = 0x42;

        /// <summary>
        /// Encrypts plain text using XOR + Base64.
        /// </summary>
        public static string Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            var bytes = Encoding.UTF8.GetBytes(text);

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ Key);
            }

            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Attempts to decrypt Base64-encoded XOR text.
        /// If the string is not valid Base64, the original string is returned.
        /// </summary>
        public static string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
            { 
                return encryptedText; 
            }

            try
            {
                var bytes = Convert.FromBase64String(encryptedText);

                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = (byte)(bytes[i] ^ Key);
                }

                return Encoding.UTF8.GetString(bytes);
            }

            catch (FormatException)
            {
                return encryptedText;
            }
        }
    }
}