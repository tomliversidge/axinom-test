﻿using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Axinom.DataManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5002")
                .Build();

            host.Run();
        }
    }
}