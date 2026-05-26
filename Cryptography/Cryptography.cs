using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Cryptography
{
    public sealed class Cryptography(string masterKey)
    {
        private static readonly UTF8Encoding Utf8 = new(false);

        private readonly byte[] _key1 = DeriveKey(masterKey, "1");
        private readonly byte[] _key2 = DeriveKey(masterKey, "2");

        // =========================
        // KEY DERIVATION
        // =========================
        private static byte[] DeriveKey(string masterKey, string dynamicPart)
        {
            var combined = $"{masterKey}:{dynamicPart}";
            return SHA256.HashData(Utf8.GetBytes(combined));
        }

        // =========================
        // AES ENCRYPT
        // =========================
        private static string Encrypt(string plainText, byte[] key)
        {
            using var aes = Aes.Create();

            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();

            byte[] plainBytes = Utf8.GetBytes(plainText);

            byte[] cipherBytes =
                encryptor.TransformFinalBlock(
                    plainBytes,
                    0,
                    plainBytes.Length);

            byte[] result = new byte[aes.IV.Length + cipherBytes.Length];

            Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);

            Buffer.BlockCopy(
                cipherBytes,
                0,
                result,
                aes.IV.Length,
                cipherBytes.Length);

            return Convert.ToBase64String(result);
        }

        // =========================
        // AES DECRYPT
        // =========================
        private static string Decrypt(string encryptedText, byte[] key)
        {
            byte[] fullBytes =
                Convert.FromBase64String(encryptedText);

            byte[] iv = new byte[16];

            int cipherLength = fullBytes.Length - 16;

            byte[] cipher = new byte[cipherLength];

            Buffer.BlockCopy(fullBytes, 0, iv, 0, 16);

            Buffer.BlockCopy(fullBytes, 16, cipher, 0, cipherLength);

            using var aes = Aes.Create();

            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();

            byte[] plainBytes =
                decryptor.TransformFinalBlock(
                    cipher,
                    0,
                    cipher.Length);

            return Utf8.GetString(plainBytes);
        }

        // =========================
        // DOUBLE ENCRYPT
        // =========================
        public string DoubleEncrypt<T>(T data)
        {
            string json =
                data is string str
                    ? str
                    : JsonSerializer.Serialize(data);

            string first =
                Encrypt(json, _key1);

            return Encrypt(first, _key2);
        }

        // =========================
        // DOUBLE DECRYPT
        // =========================
        public T DoubleDecrypt<T>(string encryptedText)
        {
            string first =
                Decrypt(encryptedText, _key2);

            string second =
                Decrypt(first, _key1);

            if (typeof(T) == typeof(string))
            {
                return (T)(object)second;
            }

            return JsonSerializer.Deserialize<T>(second)!;
        }
    }
}