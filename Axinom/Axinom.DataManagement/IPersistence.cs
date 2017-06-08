using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Axinom.DataManagement
{
    public interface IPersistence
    {
        Task Save(string key, string filename, string data);
    }
}
