using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Axinom.ControlPanel.Features.Upload
{
    public class Unzipper : IUnzip
    {
        public async Task<string> Unzip(IFormFile file)
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
    

}