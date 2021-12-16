using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BassUtils.Runtime
{
    /// <summary>
    /// Contains information about the assemblies loaded into
    /// the app and other interesting runtime information.
    /// </summary>
    public class RuntimeInformation
    {
        /// <summary>
        /// Information about the server and environment.
        /// </summary>
        public ServerRuntimeInformation ServerInfo { get; private set; }

        /// <summary>
        /// Information about the running process.
        /// </summary>
        public ProcessRuntimeInformation ProcessInfo { get; private set; }

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
            ServerInfo = new ServerRuntimeInformation();
            ProcessInfo = new ProcessRuntimeInformation();
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
