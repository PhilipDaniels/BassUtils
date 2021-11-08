using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BassUtils.Runtime
{
    /// <summary>
    /// Contains information about the assemblies loaded at runtime into
    /// the app.
    /// </summary>
    public class RuntimeInformation
    {
        /// <summary>
        /// Current UTC server time.
        /// </summary>
        public DateTime CurrentServerTimeUtc { get; private set; }

        /// <summary>
        /// Current server local time.
        /// </summary>
        public DateTime CurrentServerTimeLocal { get; private set; }

        /// <summary>
        /// Current domain info.
        /// </summary>
        public AppDomainRuntimeInformation CurrentDomainInfo { get; private set; }

        /// <summary>
        /// Information about the entry assembly.
        /// </summary>
        public AssemblyRuntimeInformation EntryAssemblyInfo { get; private set; }

        /// <summary>
        /// Information about all other assemblies.
        /// </summary>
        public IList<AssemblyRuntimeInformation> OtherAssemblies { get; private set; }

        /// <summary>
        /// Initialises a new instance based on current server time, current domain,
        /// and currently loaded assemblies.
        /// </summary>
        public RuntimeInformation()
        {
            CurrentServerTimeUtc = DateTime.UtcNow;
            CurrentServerTimeLocal = DateTime.Now;
            CurrentDomainInfo = new AppDomainRuntimeInformation();

            var entryAsm = Assembly.GetEntryAssembly();
            EntryAssemblyInfo = new AssemblyRuntimeInformation(entryAsm);

            OtherAssemblies = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(asm => asm != entryAsm)
                .OrderBy(asm => asm.FullName)
                .Select(asm => new AssemblyRuntimeInformation(asm))
                .ToList();
        }
    }
}
