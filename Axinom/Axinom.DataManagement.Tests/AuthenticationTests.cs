using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Axinom.Encryption;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Shouldly;
using Xunit;

namespace Axinom.DataManagement.Tests
{
    public class AuthenticationTests
    {
        [Fact]
        public async void can_send_data()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .UseEnvironment("Development"));

            var bytes = new AESEncryptor().Encrypt("some string here", "SBcvpEo21MnyWamdiPxf1O+kBKk53s5GWRnrv3JoUVQ=",
                "vLWsT81pAOlk7hKd4cyz5A==");
           
            ByteArrayContent byteContent = new ByteArrayContent(bytes);
            
            using (var client = server.CreateClient())
            {
                var byteArray = Encoding.GetEncoding("iso-8859-1").GetBytes($"foo:bar");
                client.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
               
                var response = await client.PostAsync("/api/zipcontents/tom/file1.zip", byteContent);
                var result = await response.Content.ReadAsStringAsync();
                result.ShouldBe("some string here");
            }
        }
        
        [Fact]
        public async void returns_401_when_unauthenticated()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .UseEnvironment("Development"));
            
            using (var client = server.CreateClient())
            {
                var byteArray = Encoding.GetEncoding("iso-8859-1").GetBytes($"not:correct");
                client.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
               
                var response = await client.PostAsync("/api/zipcontents", new ByteArrayContent(Encoding.UTF8.GetBytes("whatever")));
                response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
            }
        }
    }
}