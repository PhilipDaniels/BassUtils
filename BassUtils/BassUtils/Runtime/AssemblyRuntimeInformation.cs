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
    [DebuggerDisplay("{ShortName}")]
    public class AssemblyRuntimeInformation
    {
        /// <summary>
        /// Shortname of the assembly.
        /// </summary>
        public string ShortName { get; private set; }

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
                ShortName = asm.GetName().Name;
            }
            catch { }

            try
            {
                FullName = asm.FullName;
            }
            catch { }

            try
            {
                Location = asm.Location;
            }
            catch { }

            try
            {
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
            }
            catch { }

            try
            {
                var afva = asm.GetCustomAttribute<AssemblyFileVersionAttribute>();
                if (afva != null)
                {
                    FileVersion = afva.Version;
                }
            }
            catch { }

            try
            {
                var aiva = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                if (aiva != null)
                {
                    InformationalVersion = aiva.InformationalVersion;
                }
            }
            catch { }

            try
            {
                var aca = asm.GetCustomAttribute<AssemblyConfigurationAttribute>();
                if (aca != null)
                {
                    Configuration = aca.Configuration;
                }
            }
            catch { }

            try
            {
                var tfa = asm.GetCustomAttribute<TargetFrameworkAttribute>();
                if (tfa != null)
                {
                    FrameworkName = tfa.FrameworkName;
                }
            }
            catch { }

            try
            {
                MetaData = asm.GetCustomAttributes<AssemblyMetadataAttribute>()
                    .OrderBy(mda => mda.Key)
                    .Select(mda => $"{mda.Key}={mda.Value}")
                    .ToList();
            }
            catch { }
        }
    }
}
