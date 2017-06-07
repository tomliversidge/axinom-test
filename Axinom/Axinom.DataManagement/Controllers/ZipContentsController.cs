using System.IO;
using System.Threading.Tasks;
using Axinom.DataManagement.Configuration;
using Axinom.Encryption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Axinom.DataManagement.Controllers
{
    [Route("api/[controller]")]
    public class ZipContentsController : Controller
    {
        private readonly IOptions<AESKey> _aesKey;
        private readonly IDecryptor _decryptor;
        private readonly IPersistence _persistence;

        public ZipContentsController(IOptions<AESKey> aesKey, IDecryptor decryptor, IPersistence persistence)
        {
            _aesKey = aesKey;
            _decryptor = decryptor;
            _persistence = persistence;
        }
        
        [HttpPost("{id}/{filename}")]
        public async Task<string> Post(string id, string filename)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                await Request.Body.CopyToAsync(ms);
                bytes = ms.ToArray();
            }
           
            var decr = _decryptor.Decrypt(bytes, _aesKey.Value.Key, _aesKey.Value.IV);
            await _persistence.Save(id, filename, decr);
            return decr;
        }
    }
}