using System;
using System.Collections.Generic;
using System.Text;

namespace BassUtils.Runtime
{
    /// <summary>
    /// Information about the server and environment.
    /// </summary>
    public class ServerRuntimeInformation
    {
        /// <summary>
        /// Machine name.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Current UTC server time.
        /// </summary>
        public DateTime CurrentServerTimeUtc { get; private set; }

        /// <summary>
        /// Current server local time.
        /// </summary>
        public DateTime CurrentServerTimeLocal { get; private set; }

        /// <summary>
        /// Gets the name of the user associated with the current thread.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the CLR version.
        /// </summary>
        public string ClrVersion { get; private set; }

        /// <summary>
        /// Gets the number of processors.
        /// </summary>
        public int ProcessorCount { get; private set; }

        /// <summary>
        /// Gets the command line for this process.
        /// </summary>
        public string CommandLine { get; private set; }

        /// <summary>
        /// Returns the OS platform (Windows, Linux etc).
        /// </summary>
        public string OsPlatform { get; private set; }

        /// <summary>
        /// Returns a string describing the OS version.
        /// </summary>
        public string OsVersionString { get; private set; }

        /// <summary>
        /// Create a new instance of the <seealso cref="ServerRuntimeInformation"/>.
        /// </summary>
        public ServerRuntimeInformation()
        {
            MachineName = Environment.MachineName;
            CurrentServerTimeUtc = DateTime.UtcNow;
            CurrentServerTimeLocal = DateTime.Now;
            UserName = Environment.UserName;
            ClrVersion = Environment.Version.ToString();
            ProcessorCount = Environment.ProcessorCount;
            CommandLine = Environment.CommandLine;
            OsPlatform = Environment.OSVersion.Platform.ToString();
            OsVersionString = Environment.OSVersion.VersionString;
        }
    }
}
