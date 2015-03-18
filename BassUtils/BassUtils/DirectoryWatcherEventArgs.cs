using System;
using System.Collections.Generic;
using System.IO;

namespace BassUtils
{
    /// <summary>
    /// The EventArgs type used by the <code>DirectoryWatcher</code> class.
    /// </summary>
    public class DirectoryWatcherEventArgs : EventArgs
    {
        /// <summary>
        /// The list of file system events raised by the <code>DirectoryWatcher</code>.
        /// </summary>
        public IEnumerable<FileSystemEventArgs> FileSystemEvents { get; private set; }

        /// <summary>
        /// Construct a new DirectoryWatcherEventArgs object.
        /// </summary>
        /// <param name="fileSystemEvents">List of file system event args. Cannot be null.</param>
        public DirectoryWatcherEventArgs(IEnumerable<FileSystemEventArgs> fileSystemEvents)
        {
            FileSystemEvents = fileSystemEvents.ThrowIfNull("fileSystemEvents");
        }
    }
}
