using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Axinom.ControlPanel.Features.Upload
{
    public interface ISaveFiles
    {
        Task SaveFile(IFormFile messageFile, string folder);
    }
}