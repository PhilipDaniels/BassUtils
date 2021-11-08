using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BassUtils.Runtime;
using Microsoft.AspNetCore.Http;

namespace BassUtils.NetCore.Middleware
{
    /// <summary>
    /// A middleware to serve information about the current domain and loaded assemblies
    /// at the '/runtimeinfo' endpoint. It can serve as HTML or JSON.
    /// </summary>
    public class RuntimeInformationMiddleware
    {
        static readonly PathString _pathString = new PathString("/runtimeinfo");
        readonly RequestDelegate _next;

        public RuntimeInformationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;

            if (httpContext.Request.Path == _pathString)
            {
                var info = new RuntimeInformation();

                if (httpContext.Request.Headers["Content-Type"] == "application/json")
                {
                    httpContext.Response.ContentType = "application/json";
                    var json = JsonSerializer.Serialize(info);
                    await httpContext.Response.WriteAsync(json);
                }
                else
                {
                    httpContext.Response.ContentType = "text/html";
                    await httpContext.Response.WriteAsync("this is a text response");
                }

                return;
            }

            // Not us, pass request to next in chain.
            await _next.Invoke(httpContext);
        }
    }
}
