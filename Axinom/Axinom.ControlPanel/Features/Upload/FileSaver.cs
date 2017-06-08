using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Axinom.ControlPanel.Features.Upload
{
    public class FileSaver : ISaveFiles
    {
        private readonly string _rootDirectory;

        public FileSaver(string rootDirectory)
        {
            _rootDirectory = rootDirectory;
        }

        public async Task SaveFile(IFormFile file, string folder)
        {
            var fullPath = $"{_rootDirectory}/{folder}/{file.FileName}";
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fileStream);
            }
        }
    }
}