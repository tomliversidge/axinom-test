using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Axinom.Encryption
{
    public class AESEncryptor : IEncrypt
    {
        private readonly string _key;
        private readonly string _iv;

        public AESEncryptor(string key, string iv)
        {
            _key = key;
            _iv = iv;
        }
        public byte[] Encrypt(string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            var ivAes = Convert.FromBase64String(_iv);
            var keyAes = Convert.FromBase64String(_key);

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