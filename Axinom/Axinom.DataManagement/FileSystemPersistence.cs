using System;
using System.IO;
using System.Threading.Tasks;

namespace Axinom.DataManagement
{
    public class FileSystemPersistence : IPersistence {
        private readonly string _root;

        public FileSystemPersistence(string root)
        {
            _root = root;
        }

        public Task Save(string key, string filename, string data)
        {
            var fullPath = $"{_root}/{key}/{filename}.json";
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            using (StreamWriter outputFile = File.CreateText(fullPath))
            {
                return outputFile.WriteAsync(data);
            }
        }
    }
}