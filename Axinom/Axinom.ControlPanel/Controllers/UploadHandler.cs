using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Axinom.ControlPanel.Configuration;
using Axinom.Encryption;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Axinom.ControlPanel.Controllers
{
    public class UploadHandler : IAsyncRequestHandler<UploadModel, HttpResponseMessage>
    {
        private readonly IEncryptor _encryptor;
        private readonly IOptions<AESKey> _aesKey;
        private readonly IOptions<DataManagement> _dataManagement;

        public UploadHandler(IEncryptor encryptor, IOptions<AESKey> aesKey, IOptions<DataManagement> dataManagement)
        {
            _encryptor = encryptor;
            _aesKey = aesKey;
            _dataManagement = dataManagement;
        }
        
        public async Task<HttpResponseMessage> Handle(UploadModel message)
        {
            var unzipped = await ZipUtilities.Unzip(message.File);

            var encrypted = _encryptor.Encrypt(unzipped, _aesKey.Value.Key, _aesKey.Value.IV);
            
            var response = await DataManagementSystem.Send(encrypted, message.File.FileName, _dataManagement.Value.Url, message.Username, message.Password);
            return response;
        }

        private static bool FilterMacItems(ZipArchiveEntry e) => !e.FullName.StartsWith("__MACOSX") && !e.FullName.EndsWith(".DS_Store");

        private static async Task<HttpResponseMessage> SendToDataManagement(byte[] data, string filename, string url, string username, string password)
        {
            using (var client = new HttpClient())
            {
                var byteArray = Encoding.GetEncoding("iso-8859-1").GetBytes($"{username}:{password}");
                client.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                
                return await client.PostAsync($"{url}/{username}/{filename}", new ByteArrayContent(data));
            }
        }
    }
    
    public static class ZipUtilities
    {
        public static async Task<string> Unzip(IFormFile file)
        {
            var folder = new Folder();

            if (file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (ZipArchive archive = new ZipArchive(stream))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries.Where(FilterMacItems))
                        {
                            folder.Add(entry.FullName);
                        }
                    }
                }
            }
            return JsonConvert.SerializeObject(folder);
        }
        
        private static bool FilterMacItems(ZipArchiveEntry e) => !e.FullName.StartsWith("__MACOSX") && !e.FullName.EndsWith(".DS_Store");
        
    }
    
    
    public static class DataManagementSystem
    {
        public static async Task<HttpResponseMessage> Send(byte[] content, string filename, string url, string username, string password)
        {
            using (var client = new HttpClient())
            {
                var byteArray = Encoding.GetEncoding("iso-8859-1").GetBytes($"{username}:{password}");
                client.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                
                return await client.PostAsync($"{url}/{username}/{filename}", new ByteArrayContent(content));
            }
        }
    }
}