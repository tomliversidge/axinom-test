namespace Axinom.Encryption
{
    public interface IEncryptor
    {
        byte[] Encrypt(string text, string key, string iv);
    }
}