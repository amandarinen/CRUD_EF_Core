using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_EF_Core
{
    public class EncryptionHelper
    {
        private const byte Key = 0x42;

        public static string Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            // Using System.Text
            var bytes = Encoding.UTF8.GetBytes(text);

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ Key);
            }

            return Convert.ToBase64String(bytes);
        }

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