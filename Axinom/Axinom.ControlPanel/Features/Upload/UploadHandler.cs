using System.Net.Http;
using System.Threading.Tasks;
using Axinom.Encryption;
using MediatR;

namespace Axinom.ControlPanel.Features.Upload
{
    public class UploadHandler : IAsyncRequestHandler<UploadModel, HttpResponseMessage>
    {
        private readonly ISaveFiles _fileSaver;
        private readonly IEncrypt _encryptor;
        private readonly IUnzip _unzipper;
        private readonly IMakeWebRequests _webRequester;

        public UploadHandler(ISaveFiles fileSaver, IEncrypt encryptor, IUnzip unzipper, IMakeWebRequests webRequester)
        {
            _fileSaver = fileSaver;
            _encryptor = encryptor;
            _unzipper = unzipper;
            _webRequester = webRequester;
        }
        
        public async Task<HttpResponseMessage> Handle(UploadModel message)
        {
            await _fileSaver.SaveFile(message.File, message.Username);

            var unzipped = await _unzipper.Unzip(message.File);

            var encrypted = _encryptor.Encrypt(unzipped);
            
            var response = await _webRequester.Send(new EncryptedContent(encrypted, message.File.FileName), message.Username, message.Password);
            return response;
        }
    }
}