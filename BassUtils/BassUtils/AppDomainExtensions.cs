using System;
using System.Linq;
using System.Reflection;

namespace BassUtils
{
    /// <summary>
    /// Extensions for the AppDomain class.
    /// </summary>
    public static class AppDomainExtensions
    {
        /// <summary>
        /// Checks to see if an assembly is already loaded. The comparison is based on the FullName
        /// property of the assembly.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="path">Path to the file containing the assembly to check for.</param>
        /// <returns>True if the assembly is already loaded, false otherwise.</returns>
        public static bool IsLoaded(this AppDomain domain, string path)
        {
            domain.ThrowIfNull("domain");
            path.ThrowIfFileDoesNotExist("path");

            var loadedAssemblies = (from asm in domain.GetAssemblies()
                                   select asm.FullName
                                   ).ToHashSet();

            var assemblyName = AssemblyName.GetAssemblyName(path);

            return loadedAssemblies.Contains(assemblyName.FullName);
        }
    }
}
