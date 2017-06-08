using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Axinom.ControlPanel.Features.Upload
{
    public interface IUnzip
    {
        Task<string> Unzip(IFormFile file);
    }
}