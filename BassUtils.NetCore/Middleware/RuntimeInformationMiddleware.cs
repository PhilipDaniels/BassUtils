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
                                    border: thin solid black;
                                    margin: 25px 0;
                                    font-size: 0.9em;
                                    font-family: sans-serif;
                                    min-width: 400px;
                                }
                                table thead tr {
                                    background-color: #444;
                                    color: white;
                                    text-align: left;
                                }
                                table th, table td {
                                    padding: 4px;
                                    border-left: thin solid black;
                                    border-right: thin solid black;
                                }
                                table tbody tr:nth-of-type(even) {
                                    background-color: lightgrey;
                                }
                                .left-table td:nth-child(1) {
                                    background-color: #444;
                                    color: white;
                                    text-align: right;
                                }
                              </style>
                            </head>");

            sb.AppendLine("<body>");
            AppendServerInfo(sb, info.ServerInfo);
            AppendProcessInfo(sb, info.ProcessInfo);
            AppendCurrentDomain(sb, info.CurrentDomainInfo);
            AppendEntryAssembly(sb, info.EntryAssemblyInfo);
            AppendOtherAssemblies(sb, info.OtherAssemblies);
            sb.AppendLine("</body>");

            return sb.ToString();
        }

        void AppendServerInfo(StringBuilder sb, ServerRuntimeInformation info)
        {
            if (info == null)
                return;

            sb.AppendLine("<div class=\"ri-server\">");
            sb.AppendFormat("<h1>Server</h1>\n");
            sb.AppendLine("</div>");

            sb.AppendLine("<table class=\"normal-table\">");
            sb.AppendLine("<thead>");
            sb.AppendLine("  <tr>");
            sb.AppendLine("    <th>Name</th>");
            sb.AppendLine("    <th>Time (UTC)</th>");
            sb.AppendLine("    <th>Time (Local)</th>");
            sb.AppendLine("    <th>OS Platform</th>");
            sb.AppendLine("    <th>OS Version</th>");
            sb.AppendLine("    <th>CLR Version</th>");
            sb.AppendLine("    <th>User</th>");
            sb.AppendLine("    <th>Processors</th>");
            sb.AppendLine("    <th>Command Line</th>");
            sb.AppendLine("  </tr>");
            sb.AppendLine("</thead>");

            sb.AppendLine("<tbody>");
            sb.AppendLine("  <tr>");
            sb.AppendFormat("    <td>{0}</td>\n", info.MachineName);
            sb.AppendFormat("    <td>{0:O}</td>\n", info.CurrentServerTimeUtc);
            sb.AppendFormat("    <td>{0:O}</td>\n", info.CurrentServerTimeLocal);
            sb.AppendFormat("    <td>{0}</td>\n", info.OsPlatform);
            sb.AppendFormat("    <td>{0}</td>\n", info.OsVersionString);
            sb.AppendFormat("    <td>{0}</td>\n", info.ClrVersion);
            sb.AppendFormat("    <td>{0}</td>\n", info.UserName);
            sb.AppendFormat("    <td>{0}</td>\n", info.ProcessorCount);
            sb.AppendFormat("    <td>{0}</td>\n", info.CommandLine);
            sb.AppendLine("  </tr>");
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
        }

        void AppendProcessInfo(StringBuilder sb, ProcessRuntimeInformation info)
        {
            if (info == null)
                return;

            sb.AppendLine("<div class=\"ri-process\">");
            sb.AppendLine("<h1>Process</h1>");
            sb.AppendLine("</div>");

            sb.AppendLine("<table class=\"left-table\">");
            sb.AppendLine("<tbody>");
            sb.AppendFormat("  <tr><td>Start Time</td><td>{0:O}</td></tr>\n", info.StartTime);
            sb.AppendFormat("  <tr><td>Priority Class</td><td>{0}</td></tr>\n", info.PriorityClass);
            sb.AppendFormat("  <tr><td>Base Priority</td><td>{0}</td></tr>\n", info.BasePriority);
            sb.AppendFormat("  <tr><td>Processor Time</td><td>User = {0}</br>System = {1}</br>Total = {2}</td></tr>\n",
                info.UserProcessorTime,
                info.PrivilegedProcessorTime,
                info.TotalProcessorTime);
            sb.AppendFormat("  <tr><td>Working Set / Peak</td><td>{0} / {1}</td></tr>\n",
                Humanizer.FormatBytes(info.WorkingSet64),
                Humanizer.FormatBytes(info.PeakWorkingSet64));
            sb.AppendFormat("  <tr><td>Virtual Memory / Peak</td><td>{0} / {1}</td></tr>\n",
                Humanizer.FormatBytes(info.VirtualMemorySize64),
                Humanizer.FormatBytes(info.PeakVirtualMemorySize64));
            sb.AppendFormat("  <tr><td>Paged Memory / Peak</td><td>{0} / {1}</td></tr>\n",
                Humanizer.FormatBytes(info.PagedMemorySize64),
                Humanizer.FormatBytes(info.PeakPagedMemorySize64));
            sb.AppendFormat("  <tr><td>System Memory - Paged / Non-Paged</td><td>{0} / {1}</td></tr>\n",
                Humanizer.FormatBytes(info.PagedSystemMemorySize64),
                Humanizer.FormatBytes(info.NonpagedSystemMemorySize64));
            sb.AppendFormat("  <tr><td>Private Memory</td><td>{0}</td></tr>\n",
                Humanizer.FormatBytes(info.PrivateMemorySize64));
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");

            sb.AppendLine("<div>");
            sb.AppendLine("See <a href=\"https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.process?view=net-6.0\">System.Diagnostics.Process</a>for the meaning of these fields");
            sb.AppendLine("</div>");
        }

        void AppendCurrentDomain(StringBuilder sb, AppDomainRuntimeInformation info)
        {
            if (info == null)
                return;

            sb.AppendLine("<div class=\"ri-domain\">");
            sb.AppendLine("<h1>App Domain</h1>");
            sb.AppendLine("</div>");

            sb.AppendLine("<table class=\"left-table\">");
            sb.AppendLine("<tbody>");
            sb.AppendFormat("  <tr><td>Friendly Name</td><td>{0}</td></tr>\n", info.FriendlyName);
            sb.AppendFormat("  <tr><td>Base Directory</td><td>{0}</td></tr>\n", info.BaseDirectory);
            sb.AppendFormat("  <tr><td>Dynamic Directory</td><td>{0}</td></tr>\n", info.DynamicDirectory);
            sb.AppendFormat("  <tr><td>Relative Search Path</td><td>{0}</td></tr>\n", info.RelativeSearchPath);
            sb.AppendFormat("  <tr><td>Fully Trusted</td><td>{0}</td></tr>\n", info.IsFullyTrusted);
            sb.AppendFormat("  <tr><td>Shadow Copy</td><td>{0}</td></tr>\n", info.ShadowCopyFiles);
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
        }

        void AppendEntryAssembly(StringBuilder sb, AssemblyRuntimeInformation info)
        {
            if (info == null)
                return;

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
            if (info == null)
                return;

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
