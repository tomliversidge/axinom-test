using System.Threading.Tasks;

namespace Axinom.DataManagement
{
    public interface IPersist
    {
        Task Save(string key, string filename, string data);
    }
}
