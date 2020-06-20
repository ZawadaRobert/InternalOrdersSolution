using System;
using System.Collections.Generic;
using System.Text;

namespace InternalOrdersContextLib {
    class PasswordHasher {

        private const int SaltSize = 8;
        private const int HashSize = 24;
        private const int Iterations = 1000;
        public const string ExtraInfo = "$AZHASH$V1$";

        public static string Hash(string password) {
            // Create salt
            byte[] salt;
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // Create hash
            var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, Iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            // Combine salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            // Format hash with extra information
            return string.Format("{0}{1}${2}", ExtraInfo, Iterations, base64Hash);
        }

        // Checks if hash is supported.
        public static bool IsHashSupported(string hashString) {
            return hashString.Contains(ExtraInfo);
        }

        // Verifies a password against a hash.
        public static bool Verify(string password, string hashedPassword) {
            // Check hash
            if (!IsHashSupported(hashedPassword))
                throw new NotSupportedException("The hashtype is not supported");

            // Extract iteration and Base64 string
            var splittedHashString = hashedPassword.Replace(ExtraInfo, "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            // Get hash bytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            // Get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Create hash with given salt
            var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Get result
            for (var i = 0; i < HashSize; i++) {
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            }
            return true;
        }
    }
}
