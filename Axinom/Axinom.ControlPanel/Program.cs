﻿using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Axinom.ControlPanel
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
                .UseUrls("http://localhost:5001")
                .Build();

            host.Run();
        }
    }
}