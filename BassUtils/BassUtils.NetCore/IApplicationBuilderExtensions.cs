using BassUtils.NetCore.Middleware;
using Microsoft.AspNetCore.Builder;

namespace BassUtils.NetCore
{
    /// <summary>
    /// Extensions for <c>IApplicationBuilder</c>.
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Installs the runtime info middleware, which makes a diagnostics report
        /// available in HTML and JSON. The path defaults to "/runtimeinfo".
        /// </summary>
        /// <param name="app">The app builder.</param>
        /// <param name="path">URL to expose the runtime information on.</param>
        public static IApplicationBuilder UseRuntimeInfo(this IApplicationBuilder app, string path = "/runtimeinfo")
        {
            return app.UseMiddleware<RuntimeInformationMiddleware>(path);
        }
    }
}
