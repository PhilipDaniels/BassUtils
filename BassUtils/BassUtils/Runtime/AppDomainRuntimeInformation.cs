using System;

namespace BassUtils.Runtime
{
    /// <summary>
    /// Collects runtime information about an entire app domain.
    /// </summary>
    public class AppDomainRuntimeInformation
    {
        /// <summary>
        /// The friendly name of the domain.
        /// </summary>
        public string FriendlyName { get; private set; }

        /// <summary>
        /// The base directory of the domain.
        /// </summary>
        public string BaseDirectory { get; private set; }

        /// <summary>
        /// The dynamic directory of the domain.
        /// </summary>
        public string DynamicDirectory { get; private set; }

        /// <summary>
        /// The relative search path of the domain.
        /// </summary>
        public string RelativeSearchPath { get; private set; }

        /// <summary>
        /// Whether the domain is fully trusted.
        /// </summary>
        public bool IsFullyTrusted { get; private set; }

        /// <summary>
        /// Whether shadow copying is in operation for the domain.
        /// </summary>
        public bool ShadowCopyFiles { get; private set; }

        /// <summary>
        /// Construct a new instance of the app domain information
        /// based on information from the current domain.
        /// </summary>
        public AppDomainRuntimeInformation()
            : this(AppDomain.CurrentDomain)
        {
        }

        /// <summary>
        /// Construct a new object with the fields initialized from
        /// the <paramref name="domain"/>.
        /// </summary>
        /// <param name="domain">The domain to read the information from.</param>
        public AppDomainRuntimeInformation(AppDomain domain)
        {
            FriendlyName = domain.FriendlyName;
            BaseDirectory = domain.BaseDirectory;
            DynamicDirectory = domain.DynamicDirectory;
            RelativeSearchPath = domain.RelativeSearchPath;
            IsFullyTrusted = domain.IsFullyTrusted;
            ShadowCopyFiles = domain.ShadowCopyFiles;
        }
    }
}
