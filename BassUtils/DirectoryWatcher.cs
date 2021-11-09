using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Dawn;

namespace BassUtils
{
    /// <summary>
    /// Wrapper around <code>FileSystemWatcher</code> that tries to uniqueify events,
    /// because <code>FileSystemWatcher</code> raises lots of duplicates.
    /// </summary>
    public sealed class DirectoryWatcher : IDisposable
    {
        /// <summary>
        /// The <code>ChangedFiles event is raised periodically whenever there</code>
        /// are file system events.
        /// </summary>
        public event EventHandler<DirectoryWatcherEventArgs> ChangedFiles;

        /// <summary>
        /// The options that the watcher was created with.
        /// </summary>
        /// <value>
        /// The options that the watcher was created with.
        /// </value>
        public DirectoryWatcherOptions Options { get; private set; }

        //private HashSet<string> filesToIgnore;
        //private List<string> directoriesToIgnore;
        //private List<Regex> patternsToIgnore;
        private readonly FileSystemWatcher watcher;
        private readonly List<FileSystemEventArgs> notifiedEvents;
        private readonly Timer timer;
        private bool disposed;

        /// <summary>
        /// Create a new <code>DirectoryWatcher</code> on <paramref name="directory"/>
        /// using the default options.
        /// </summary>
        /// <param name="directory">The directory to watch.</param>
        public DirectoryWatcher(string directory)
            : this(new DirectoryWatcherOptions(directory))
        {
        }

        /// <summary>
        /// Create a new <code>DirectoryWatcher</code> based on the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">Options used to configure the watcher.</param>
        public DirectoryWatcher(DirectoryWatcherOptions options)
        {
            Options = Guard.Argument(options, nameof(options)).NotNull().Value;

            watcher = new FileSystemWatcher();
            notifiedEvents = new List<FileSystemEventArgs>();
            timer = new Timer(OnTimeout, null, Options.TimerPeriodMilliseconds, Options.TimerPeriodMilliseconds);
        }

        /// <summary>
        /// Starts the watcher.
        /// </summary>
        public void Start()
        {
            ThrowIfDisposed();

            watcher.Path = Options.Directory;
            watcher.IncludeSubdirectories = Options.IncludeSubDirectories;
            watcher.Created += Watcher_Fired;
            watcher.Changed += Watcher_Fired;
            watcher.Deleted += Watcher_Fired;
            watcher.Renamed += Watcher_Fired;

            watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Stops the watcher.
        /// </summary>
        public void Stop()
        {
            ThrowIfDisposed();

            watcher.EnableRaisingEvents = false;
            timer.Dispose();
        }

        /// <summary>
        /// Disposes the watcher.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                watcher.Dispose();
                timer.Dispose();
                disposed = true;
            }
        }

        private bool ShouldIgnore(string filename)
        {
            if (Options.FilesToIgnore != null &&
                Options.FilesToIgnore.Any(s => s.Equals(Path.GetFileName(filename), Options.FileAndDirectoryComparisonType)))
            {
                return true;
            }

            if (Options.DirectoriesToIgnore != null &&
                Options.DirectoriesToIgnore.Any(d => filename.StartsWith(d, Options.FileAndDirectoryComparisonType)))
            {
                return true;
            }

            if (Options.PatternsToIgnore != null &&
                Options.PatternsToIgnore.Any(r => r.IsMatch(filename)))
            {
                return true;
            }

            if (Options.IgnoreCallback != null && Options.IgnoreCallback(filename))
            {
                return true;
            }

            return false;
        }

        private void Watcher_Fired(object sender, FileSystemEventArgs e)
        {
            if (!ShouldIgnore(e.FullPath))
            {
                lock (notifiedEvents)
                {
                    notifiedEvents.Add(e);
                }
            }
        }

        private void OnTimeout(object state)
        {
            ThrowIfDisposed();

            var notifications = new List<FileSystemEventArgs>();

            if (notifiedEvents.Count == 0)
            {
                return;
            }

            lock (notifiedEvents)
            {
                if (notifiedEvents.Count == 0)
                {
                    return;
                }

                // When the timer fires, get all pending notifications from the queue,
                // simplify/uniqueify them, and yield them as events. This eliminates
                // duplicate events that the FileSystemWatcher raises.
                // Do this even if there is nobody to receive the events, because it
                // prevents the queue from growing forever.
                notifications.AddRange(notifiedEvents);
                notifiedEvents.Clear();
            }

            var evt = ChangedFiles;
            if (evt != null)
            {
                var args = new DirectoryWatcherEventArgs(notifications.Distinct());
                evt(this, args);
            }
        }

        private void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new InvalidOperationException("The DirectoryWatcher is already disposed.");
            }
        }
    }
}
