using System;
using System.IO;
using System.Security.Cryptography;

namespace Axinom.Encryption
{
    public class AESDecryptor : IDecryptor
    {
        public string Decrypt(byte[] data, string key, string iv)
        {
            var ivAes = Convert.FromBase64String(iv);
            var keyAes = Convert.FromBase64String(key);
            string result;
            using (var aes = Aes.Create())
            {
                aes.Key = keyAes;
                aes.IV = ivAes;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var resultStream = new MemoryStream(data))
                {
                    using (var aesStream = new CryptoStream(resultStream, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(aesStream))
                    {
                        result = srDecrypt.ReadToEnd();
                    }
                }
            }
            return result;
        }
    }
}