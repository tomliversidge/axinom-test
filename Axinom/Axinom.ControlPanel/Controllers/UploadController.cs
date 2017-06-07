using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Axinom.ControlPanel.Configuration;
using Axinom.Encryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Axinom.ControlPanel.Controllers
{
    public class UploadZipModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public IFormFile File { get; set; }
    }
    
    public class UploadController : Controller
    {
        private readonly IOptions<AESKey> _aesKey;
        private readonly IOptions<DataManagement> _dataManagementCredentials;
        private readonly IEncryptor _encrypter;

        public UploadController(IOptions<AESKey> aesKey, IOptions<DataManagement> dataManagementCredentials, IEncryptor encrypter)
        {
            _aesKey = aesKey;
            _dataManagementCredentials = dataManagementCredentials;
            _encrypter = encrypter;
        }
        
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(UploadZipModel model)
        {
            Console.WriteLine(model.Username);
            var unzipped = await UnzipFile(model.File);
            
            var encrypted = _encrypter.Encrypt(unzipped, _aesKey.Value.Key, _aesKey.Value.IV);

            var response = await SendToDataManagement(encrypted, model.File.FileName, _dataManagementCredentials.Value.Url, model.Username, model.Password);

            if (response.IsSuccessStatusCode)
                return Ok(response.StatusCode);
            
            // TODO log errors
            // TODO better error handling
            return StatusCode((int)response.StatusCode);
        }

        private static async Task<string> UnzipFile(IFormFile file)
        {
            var folder = new Folder();

            if (file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (ZipArchive archive = new ZipArchive(stream))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries.Where(
                            e => !e.FullName.StartsWith("__MACOSX") && !e.FullName.EndsWith(".DS_Store")))
                        {
                            folder.Add(entry.FullName);
                        }
                    }
                }
            }
            
            return JsonConvert.SerializeObject(folder);
        }

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
}
