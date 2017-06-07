using System;
using System.Text;
using System.Threading.Tasks;
using Axinom.DataManagement.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Axinom.DataManagement
{
    public class BasicAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<DataManagementCredentials> _credentials;

        public BasicAuthenticationMiddleware(RequestDelegate next, IOptions<DataManagementCredentials> credentials)
        {
            _next = next;
            _credentials = credentials;
        }

        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                //Extract credentials
                var encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                var encoding = Encoding.GetEncoding("iso-8859-1");
                var usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                var credentials = usernamePassword.Split(':');

                var username = credentials[0];
                var password = credentials[1];

                if(username == _credentials.Value.Username && password == _credentials.Value.Password)
                {
                    await _next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = 401; //Unauthorized
                    return;
                }
            }
            else
            {
                // no authorization header
                context.Response.StatusCode = 401; //Unauthorized
                return;
            }
        }
    }
}