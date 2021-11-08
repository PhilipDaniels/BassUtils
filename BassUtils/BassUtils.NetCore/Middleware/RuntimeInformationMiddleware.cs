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
    /// See https://stevetalkscode.co.uk/middleware-styles
    /// </summary>
    public class RuntimeInformationMiddleware
    {
        /// <summary>
        /// URL to check against. This must be set before you register the middleware!
        /// In other words, it is a one-time initialisation.
        /// </summary>
        public static string Url { get; set; }

        readonly RequestDelegate nextRequestDelegate;

        /// <summary>
        /// Construct a new instance of the middleware.
        /// </summary>
        /// <param name="nextRequestDelegate">Middleware delegate - the next in the chain.</param>
        public RuntimeInformationMiddleware(RequestDelegate nextRequestDelegate)
        {
            this.nextRequestDelegate = nextRequestDelegate;
        }

        /// <summary>
        /// Invoke the middleware.
        /// </summary>
        /// <param name="httpContext">Http context to invoke within.</param>
        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;

            if (httpContext.Request.Path == new PathString(Url))
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
            await nextRequestDelegate.Invoke(httpContext);
        }
    }
}
