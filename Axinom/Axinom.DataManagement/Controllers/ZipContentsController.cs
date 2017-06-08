using System.IO;
using System.Threading.Tasks;
using Axinom.Encryption;
using Microsoft.AspNetCore.Mvc;

namespace Axinom.DataManagement.Controllers
{
    [Route("api/[controller]")]
    public class ZipContentsController : Controller
    {
        private readonly IDecrypt _decryptor;
        private readonly IPersist _persistor;

        public ZipContentsController(IDecrypt decryptor, IPersist persistor)
        {
            _decryptor = decryptor;
            _persistor = persistor;
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
           
            var decr = _decryptor.Decrypt(bytes);
            await _persistor.Save(id, filename, decr);
            return decr;
        }
    }
}