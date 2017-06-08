using System.Net.Http;
using System.Threading.Tasks;

namespace Axinom.ControlPanel.Features.Upload
{
    public interface IMakeWebRequests
    {
        Task<HttpResponseMessage> Send(EncryptedContent content, string username, string password);
    }
}