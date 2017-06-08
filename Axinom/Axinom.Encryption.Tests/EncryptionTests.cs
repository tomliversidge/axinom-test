using System;
using Shouldly;
using Xunit;

namespace Axinom.Encryption.Tests
{
    public class EncryptionTests
    {
        [Fact]
        public void can_encrypt_and_decrypt_using_aes()
        {
            var encryptor = new AESEncryptor("SBcvpEo21MnyWamdiPxf1O+kBKk53s5GWRnrv3JoUVQ=", "vLWsT81pAOlk7hKd4cyz5A==");
            var encr = encryptor.Encrypt("some string");

            var stringy = Convert.ToBase64String(encr);
            var decryptor = new AESDecryptor("SBcvpEo21MnyWamdiPxf1O+kBKk53s5GWRnrv3JoUVQ=", "vLWsT81pAOlk7hKd4cyz5A==");
            var decr = decryptor.Decrypt(encr);
            decr.ShouldBe("some string");
        }
    }
}