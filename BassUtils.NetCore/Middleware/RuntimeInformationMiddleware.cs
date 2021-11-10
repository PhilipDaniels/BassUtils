using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
        //public static string Url { get; set; }

        readonly RequestDelegate nextRequestDelegate;
        readonly PathString path;

        /// <summary>
        /// Construct a new instance of the middleware.
        /// </summary>
        /// <param name="nextRequestDelegate">Middleware delegate - the next in the chain.</param>
        /// <param name="path">URL path to expose the runtime information on.</param>
        public RuntimeInformationMiddleware(RequestDelegate nextRequestDelegate, string path)
        {
            this.nextRequestDelegate = nextRequestDelegate;
            this.path = new PathString(path);
        }

        /// <summary>
        /// Invoke the middleware.
        /// </summary>
        /// <param name="httpContext">Http context to invoke within.</param>
        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;

            if (httpContext.Request.Path == this.path)
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
                    await httpContext.Response.WriteAsync(GetHtml(info));
                }

                return;
            }

            // Not us, pass request to next in chain.
            await nextRequestDelegate.Invoke(httpContext);
        }

        string GetHtml(RuntimeInformation info)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<!doctype html>");
            sb.AppendLine("<html lang=\"en\">");

            sb.AppendLine(@"<head>
                            <meta charset=""utf-8"">
                            <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                            <title>Runtime Information</title>
                            <meta name=""description"" content=""Runtime Information for this site"">
                            <style>
                                body {
                                    font-family: ""Arial"", ""Helvetica"", Sans-serif;
                                    font-size: 0.875em;
                                }
                                span.time {
                                    font-weight: bold;
                                }
                                h1 {
                                    color: DodgerBlue;
                                }
                                table {
                                    border-collapse: collapse;
                                    margin: 25px 0;
                                    font-size: 0.9em;
                                    font-family: sans-serif;
                                    min-width: 400px;
                                    box-shadow: 0 0 20px rgba(0, 0, 0, 0.15);
                                }
                                table thead tr {
                                    background-color: black;
                                    color: white;
                                    text-align: left;
                                }
                                table th, table td {
                                    padding: 3px;
                                    border-left: thin solid black;
                                    border-right: thin solid black;
                                }
                                table tbody tr:nth-of-type(even) {
                                    background-color: lightgrey;
                                }
                                .left-table td:nth-child(1) {
                                    background-color: black;
                                    color: white;
                                    text-align: right;
                                }
                              </style>
                            </head>");

            sb.AppendLine("<body>");
            AppendHeader(sb, info);
            AppendCurrentDomain(sb, info.CurrentDomainInfo);
            AppendEntryAssembly(sb, info.EntryAssemblyInfo);
            AppendOtherAssemblies(sb, info.OtherAssemblies);
            sb.AppendLine("</body>");

            return sb.ToString();
        }

        void AppendHeader(StringBuilder sb, RuntimeInformation info)
        {
            sb.AppendLine("<div class=\"ri-header\">");
            sb.AppendFormat("<h1>Server: {0}</h1>\n", info.ServerName);
            sb.AppendFormat("<p>Current server UTC time is <span class=\"time\">{0:O}</span> and current server local time is <span class=\"time\">{1:O}</span></p>", info.CurrentServerTimeUtc, info.CurrentServerTimeLocal);
            sb.AppendLine("</div>");
        }

        void AppendCurrentDomain(StringBuilder sb, AppDomainRuntimeInformation domain)
        {
            if (domain == null)
            {
                return;
            }

            sb.AppendLine("<div class=\"ri-domain\">");
            sb.AppendLine("<h1>App Domain</h1>");
            sb.AppendLine("</div>");

            sb.AppendLine("<table class=\"left-table\">");
            sb.AppendLine("<tbody>");
            sb.AppendFormat("  <tr><td>Friendly Name</td><td>{0}</td></tr>\n", domain.FriendlyName);
            sb.AppendFormat("  <tr><td>Base Directory</td><td>{0}</td></tr>\n", domain.BaseDirectory);
            sb.AppendFormat("  <tr><td>Dynamic Directory</td><td>{0}</td></tr>\n", domain.DynamicDirectory);
            sb.AppendFormat("  <tr><td>Relative Search Path</td><td>{0}</td></tr>\n", domain.RelativeSearchPath);
            sb.AppendFormat("  <tr><td>Fully Trusted</td><td>{0}</td></tr>\n", domain.IsFullyTrusted);
            sb.AppendFormat("  <tr><td>Shadow Copy</td><td>{0}</td></tr>\n", domain.ShadowCopyFiles);
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
        }

        void AppendEntryAssembly(StringBuilder sb, AssemblyRuntimeInformation info)
        {
            if (info == null)
            {
                return;
            }

            sb.AppendLine("<div class=\"ri-entry\">");
            sb.AppendLine("<h1>Entry Assembly</h1>");
            sb.AppendLine("</div>");

            sb.AppendLine("<table class=\"normal-table\">");
            AppendAssemblyTableHeader(sb);
            sb.AppendLine("<tbody>");
            AppendAssemblyTableRow(sb, info);
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
        }

        void AppendOtherAssemblies(StringBuilder sb, IEnumerable<AssemblyRuntimeInformation> assemblies)
        {
            if (assemblies == null || !assemblies.Any())
            {
                return;
            }

            sb.AppendLine("<div class=\"ri-others\">");
            sb.AppendLine("<h1>Other Assemblies</h1>");
            sb.AppendLine("</div>");

            sb.AppendLine("<table class=\"normal-table\">");
            AppendAssemblyTableHeader(sb);
            sb.AppendLine("<tbody>");
            foreach (var asm in assemblies.OrderBy(a => a.FullName))
            {
                AppendAssemblyTableRow(sb, asm);
            }
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
        }

        void AppendAssemblyTableHeader(StringBuilder sb)
        {
            sb.AppendLine("<thead>");
            sb.AppendLine("  <tr>");
            sb.AppendLine("    <th>Name</th>");
            sb.AppendLine("    <th>Version</th>");
            sb.AppendLine("    <th>File Version</th>");
            sb.AppendLine("    <th>Informational Version</th>");
            sb.AppendLine("    <th>Configuration</th>");
            sb.AppendLine("    <th>Framework Name</th>");
            sb.AppendLine("    <th>Location </th>");
            sb.AppendLine("    <th>Meta Data</th>");
            sb.AppendLine("    <th>Full Name</th>");
            sb.AppendLine("  </tr>");
            sb.AppendLine("</thead>");
        }

        void AppendAssemblyTableRow(StringBuilder sb, AssemblyRuntimeInformation info)
        {
            sb.AppendLine("  <tr>");
            sb.AppendFormat("    <td>{0}</td>\n", info.ShortName);
            sb.AppendFormat("    <td>{0}</td>\n", info.Version);
            sb.AppendFormat("    <td>{0}</td>\n", info.FileVersion);
            sb.AppendFormat("    <td>{0}</td>\n", info.InformationalVersion);
            sb.AppendFormat("    <td>{0}</td>\n", info.Configuration);
            sb.AppendFormat("    <td>{0}</td>\n", info.FrameworkName);
            sb.AppendFormat("    <td>{0}</td>\n", info.Location);
            sb.AppendFormat("    <td>{0}</td>\n", string.Join(" ", info.MetaData));
            sb.AppendFormat("    <td>{0}</td>\n", info.FullName);
            sb.AppendLine("  </tr>");
        }
    }
}
