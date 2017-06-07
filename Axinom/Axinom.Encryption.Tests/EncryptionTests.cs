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
            var encryptor = new AESEncryptor();
            var encr = encryptor.Encrypt("some string", "SBcvpEo21MnyWamdiPxf1O+kBKk53s5GWRnrv3JoUVQ=", "vLWsT81pAOlk7hKd4cyz5A==");

            var stringy = Convert.ToBase64String(encr);
            var decryptor = new AESDecryptor();
            var decr = decryptor.Decrypt(encr, "SBcvpEo21MnyWamdiPxf1O+kBKk53s5GWRnrv3JoUVQ=", "vLWsT81pAOlk7hKd4cyz5A==");
            decr.ShouldBe("some string");

        }
    }
}