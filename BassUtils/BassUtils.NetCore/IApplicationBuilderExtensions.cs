using BassUtils.NetCore.Middleware;
using Microsoft.AspNetCore.Builder;

namespace BassUtils.NetCore
{
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Installs the runtime info middleware, which makes a diagnostics report
        /// available in HTML and JSON at "/runtimeinfo".
        /// </summary>
        public static IApplicationBuilder UseRuntimeInfo(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RuntimeInformationMiddleware>();
        }
    }
}
