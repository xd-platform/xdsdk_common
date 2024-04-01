using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace XD.SDK.Common.Internal {
    public static class EncryptionUtils {
        private const int IvSize = 16; // AES block size in bytes

        public static string Encrypt(string plainText, string key) {
            byte[] iv = new byte[IvSize];
            using (var random = RandomNumberGenerator.Create()) {
                random.GetBytes(iv); // Generate a random IV
            }

            byte[] encrypted;

            using (Aes aes = Aes.Create()) {
                aes.Key = GetAesKey(key);
                aes.Mode = CipherMode.CBC;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream()) {
                    // Prepend the IV to the ciphertext
                    memoryStream.Write(iv, 0, iv.Length);

                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)) {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream)) {
                            streamWriter.Write(plainText);
                        }
                    }

                    encrypted = memoryStream.ToArray();
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string cipherTextWithIv, string key) {
            byte[] buffer = Convert.FromBase64String(cipherTextWithIv);

            using (Aes aes = Aes.Create()) {
                aes.Key = GetAesKey(key);
                aes.Mode = CipherMode.CBC;

                // Extract the IV from the beginning of the ciphertext
                byte[] iv = new byte[IvSize];
                Array.Copy(buffer, 0, iv, 0, iv.Length);

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, iv);

                using (MemoryStream memoryStream = new MemoryStream(buffer, IvSize, buffer.Length - IvSize)) {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)) {
                        using (StreamReader streamReader = new StreamReader(cryptoStream)) {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        private static byte[] GetAesKey(string key) {
            // Use SHA-256 hash algorithm to generate a 256-bit key from any length input
            using (SHA256 sha256Hasher = SHA256.Create()) {
                return sha256Hasher.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }
    }
}