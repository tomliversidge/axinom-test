namespace Axinom.ControlPanel.Features.Upload
{
    public class EncryptedContent
    {
        public byte[] Data { get; }
        public string Filename { get; }

        public EncryptedContent(byte[] data, string filename)
        {
            Data = data;
            Filename = filename;
        }
    }
}