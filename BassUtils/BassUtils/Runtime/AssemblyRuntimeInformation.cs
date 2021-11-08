using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

namespace BassUtils.Runtime
{
    /// <summary>
    /// Specifies information about assemblies loaded at runtime.
    /// </summary>
    [DebuggerDisplay("{FullName}")]
    public class AssemblyRuntimeInformation
    {
        /// <summary>
        /// The fullname of the assembly.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// The location of the assembly.
        /// </summary>
        public string Location { get; private set; }

        /// <summary>
        /// The version of the assembly.
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// The file version of the assembly.
        /// </summary>
        public string FileVersion { get; private set; }

        /// <summary>
        /// The value of the <c>AssemblyInformationalVersionAttribute</c>.
        /// </summary>
        public string InformationalVersion { get; private set; }

        /// <summary>
        /// The configuration (Debug, etc.) that the assembly was built under.
        /// </summary>
        public string Configuration { get; private set; }

        /// <summary>
        /// The target framework name.
        /// </summary>
        public string FrameworkName { get; private set; }

        /// <summary>
        /// Information extracted from any <c>AssemblyMetadataAttributes</c>
        /// that are present.
        /// </summary>
        public IList<string> MetaData { get; private set; }

        internal AssemblyRuntimeInformation(Assembly asm)
        {
            try
            {
                FullName = asm.FullName;
                Location = asm.Location;

                var ava = asm.GetCustomAttribute<AssemblyVersionAttribute>();
                if (ava != null)
                {
                    Version = ava.Version;
                }
                else
                {
                    if (asm.GetName().Version != null)
                    {
                        Version = asm.GetName().Version.ToString();
                    }
                }

                var afva = asm.GetCustomAttribute<AssemblyFileVersionAttribute>();
                if (afva != null)
                {
                    FileVersion = afva.Version;
                }

                var aiva = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                if (aiva != null)
                {
                    InformationalVersion = aiva.InformationalVersion;
                }

                var aca = asm.GetCustomAttribute<AssemblyConfigurationAttribute>();
                if (aca != null)
                {
                    Configuration = aca.Configuration;
                }

                var tfa = asm.GetCustomAttribute<TargetFrameworkAttribute>();
                if (tfa != null)
                {
                    FrameworkName = tfa.FrameworkName;
                }

                MetaData = asm.GetCustomAttributes<AssemblyMetadataAttribute>()
                    .Select(ma => $"{ma.Key}={ma.Value}")
                    .OrderBy(s => s)
                    .ToList();
            }
            catch { }
        }
    }
}
