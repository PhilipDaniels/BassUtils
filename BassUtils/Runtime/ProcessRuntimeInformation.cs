using System;
using System.Diagnostics;

namespace BassUtils.Runtime
{
    /// <summary>
    /// Information about the running process.
    /// </summary>
    public class ProcessRuntimeInformation
    {
        /// <summary>
        /// How long the process has been alive.
        /// </summary>
        public TimeSpan UpTime { get; private set; }

        /// <summary>
        /// Gets the base priority of the associated process.
        /// </summary>
        public int BasePriority { get; private set; }

        /// <summary>
        /// The amount of system memory, in bytes, allocated for the associated process that cannot be written to the virtual memory paging file.
        /// </summary>
        public long NonpagedSystemMemorySize64 { get; private set; }

        /// <summary>
        /// Gets the amount of paged memory, in bytes, allocated for the associated process.
        /// </summary>
        public long PagedMemorySize64 { get; private set; }

        /// <summary>
        /// Gets the amount of pageable system memory, in bytes, allocated for the associated process.
        /// </summary>
        public long PagedSystemMemorySize64 { get; private set; }

        /// <summary>
        /// Gets the maximum amount of memory in the virtual memory paging file, in bytes, used by the associated process.
        /// </summary>
        public long PeakPagedMemorySize64 { get; private set; }

        /// <summary>
        /// Gets the maximum amount of virtual memory, in bytes, used by the associated process.
        /// </summary>
        public long PeakVirtualMemorySize64 { get; private set; }

        /// <summary>
        /// Gets the maximum amount of physical memory, in bytes, used by the associated process.
        /// </summary>
        public long PeakWorkingSet64 { get; private set; }

        /// <summary>
        /// Gets the overall priority category for the associated process.
        /// </summary>
        public ProcessPriorityClass PriorityClass { get; private set; }

        /// <summary>
        /// Gets the amount of private memory, in bytes, allocated for the associated process.
        /// </summary>
        public long PrivateMemorySize64 { get; private set; }

        /// <summary>
        /// Gets the privileged processor time for this process.
        /// </summary>
        public TimeSpan PrivilegedProcessorTime { get; private set; }

        /// <summary>
        /// The time the process started.
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// Gets the total processor time for this process.
        /// </summary>
        public TimeSpan TotalProcessorTime { get; private set; }

        /// <summary>
        /// Gets the user processor time for this process.
        /// </summary>
        public TimeSpan UserProcessorTime { get; private set; }

        /// <summary>
        /// Gets the amount of the virtual memory, in bytes, allocated for the associated process.
        /// </summary>
        public long VirtualMemorySize64 { get; private set; }

        /// <summary>
        /// Gets the amount of physical memory, in bytes, allocated for the associated process.
        /// </summary>
        public long WorkingSet64 { get; private set; }

        /// <summary>
        /// Create a new instance of the <seealso cref="ProcessRuntimeInformation"/>.
        /// </summary>
        public ProcessRuntimeInformation()
        {
            using var p = Process.GetCurrentProcess();

            try
            {
                BasePriority = p.BasePriority;
            }
            catch { }

            try
            {
                NonpagedSystemMemorySize64 = p.NonpagedSystemMemorySize64;
            }
            catch { }

            try
            {
                PagedMemorySize64 = p.PagedMemorySize64;
            }
            catch { }

            try
            {
                PagedSystemMemorySize64 = p.PagedSystemMemorySize64;
            }
            catch { }

            try
            {
                PeakPagedMemorySize64 = p.PeakPagedMemorySize64;
            }
            catch { }

            try
            {
                PeakVirtualMemorySize64 = p.PeakVirtualMemorySize64;
            }
            catch { }

            try
            {
                PeakWorkingSet64 = p.PeakWorkingSet64;
            }
            catch { }

            try
            {
                PriorityClass = p.PriorityClass;
            }
            catch { }

            try
            {
                PrivateMemorySize64 = p.PrivateMemorySize64;
            }
            catch { }

            try
            {
                PrivilegedProcessorTime = p.PrivilegedProcessorTime;
            }
            catch { }

            try
            {
                StartTime = p.StartTime;
                UpTime = DateTime.Now - StartTime;
            }
            catch { }

            try
            {
                TotalProcessorTime = p.TotalProcessorTime;
            }
            catch { }

            try
            {
                UserProcessorTime = p.UserProcessorTime;
            }
            catch { }

            try
            {
                VirtualMemorySize64 = p.VirtualMemorySize64;
            }
            catch { }

            try
            {
                WorkingSet64 = p.WorkingSet64;
            }
            catch { }
        }
    }
}
