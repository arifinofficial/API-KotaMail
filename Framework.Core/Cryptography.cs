using System.Security.Cryptography;
using System.Text;

namespace Framework.Core
{
    public static class Cryptography
    {
        public static string Encrypt(string plainText, string base64Key, string base64Iv) 
        {
            var (key, iv) = AesKeyIvGenerator(base64Key, base64Iv);

            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (StreamWriter swEncrypt = new(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }
            byte[] encryptedBytes = msEncrypt.ToArray();
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string cipherTextBase64, string base64Key, string base64Iv)
        {
            byte[] cipherText = Convert.FromBase64String(cipherTextBase64);
            var (key, iv) = AesKeyIvGenerator(base64Key, base64Iv);

            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream msDecrypt = new MemoryStream(cipherText);
            using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }

        private static (byte[] Key, byte[] IV) AesKeyIvGenerator(string key, string iv)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] hashKey = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
            byte[] hashIv = sha256.ComputeHash(Encoding.UTF8.GetBytes(iv));

            return (hashKey[..24], hashIv[..16]);
        }
    }
}
