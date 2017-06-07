using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Axinom.Encryption
{
    public class AESEncryptor : IEncryptor
    {
        public byte[] Encrypt(string text, string key, string iv)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            var ivAes = Convert.FromBase64String(iv);
            var keyAes = Convert.FromBase64String(key);

            byte[] result;
            using (var aes = Aes.Create())
            {
                aes.Key = keyAes;
                aes.IV = ivAes;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var resultStream = new MemoryStream())
                {
                    using (var aesStream = new CryptoStream(resultStream, encryptor, CryptoStreamMode.Write))
                    using (var plainStream = new MemoryStream(buffer))
                    {
                        plainStream.CopyTo(aesStream);
                    }

                    result = resultStream.ToArray();
                }
            }
            return result;
        }
    }
}