﻿using Axinom.DataManagement.Configuration;
using Axinom.Encryption;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Axinom.DataManagement
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.Configure<DataManagementCredentials>(Configuration.GetSection("Secrets:DataManagementCredentials"));
            services.Configure<AESKey>(Configuration.GetSection("Secrets:AESKey"));
            services.AddSingleton<IDecrypt>(new AESDecryptor(Configuration["Secrets:AESKey:Key"], Configuration["Secrets:AESKey:IV"]));
            services.AddSingleton<IPersist>(new FileSystemPersistence(Configuration["FileSystem:Root"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            app.UseMiddleware<BasicAuthenticationMiddleware>();
            app.UseMvc();
        }
    }
}