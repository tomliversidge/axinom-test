using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Axinom.ControlPanel.Features.Upload
{
    public class DataManagementWebRequester : IMakeWebRequests
    {
        private readonly string _baseUrl;

        public DataManagementWebRequester(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<HttpResponseMessage> Send(EncryptedContent content, string username, string password)
        {
            using (var client = new HttpClient())
            {
                var byteArray = Encoding.GetEncoding("iso-8859-1").GetBytes($"{username}:{password}");
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                return await client.PostAsync($"{_baseUrl}/{username}/{content.Filename}", new ByteArrayContent(content.Data));
            }
        }
    }
}