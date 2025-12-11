using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_EF_Core
{
    /// <summary>
    /// Provides helper methods for securely generating salts, hashing values, and verifying hashed values.
    /// </summary>
    public class HashingHelper
    {
        /// <summary>
        /// Generates a cryptographically secure random salt.
        /// </summary>
        /// <param name="size">The number of random bytes to generate. Default is 16 bytes (128 bits).</param>
        /// <returns>A Base64-encoded string containing the generated salt.</returns>
        public static string GenerateSalt(int size = 16)
        {
            var saltBytes = RandomNumberGenerator.GetBytes(size);
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Hashes a value with the given salt.
        /// </summary>
        /// <param name="value">The plaintext value to hash (in this instance a personnummer).</param>
        /// <param name="base64Salt">The Base64-encoded salt to use for hashing.</param>
        /// <param name="iterations">The number of PBKDF2 iterations. Higher means more secure but slower.</param>
        /// <param name="hashLength">The length of the generated hash in bytes. Default is 32 bytes (256 bits).</param>
        /// <returns>A Base64-encoded string containing the resulting hashed value.</returns>
        public static string HashWithSalt(string value, string base64Salt, int iterations = 100_000, int hashLength = 32)
        {
            var saltBytes = Convert.FromBase64String(base64Salt);
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password: value,
                salt: saltBytes,
                iterations: iterations,
                hashAlgorithm: HashAlgorithmName.SHA256);

            var hashBytes = pbkdf2.GetBytes(hashLength);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Verifies that a plaintext value matches a previously stored salt + hash.
        /// </summary>
        /// <param name="value">The value to test (the customers personnummer).</param>
        /// <param name="base64Salt">The Base64-encoded salt stored for the user.</param>
        /// <param name="expectedBase64Hash">The Base64-encoded hash stored for the user.</param>
        /// <returns>True if the value, when hashed with the same salt, matches the stored hash; otherwise false.</returns>
        public static bool Verify(string value, string base64Salt, string expectedBase64Hash)
        {
            var computedHash = HashWithSalt(value, base64Salt);
            return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(expectedBase64Hash),
                Convert.FromBase64String(computedHash));
        }
    }
}
