namespace Axinom.Encryption
{
    public interface IDecryptor
    {
        string Decrypt(byte[] data, string key, string iv);
    }
}