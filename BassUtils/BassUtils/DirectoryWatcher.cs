using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace BassUtils
{
    /// <summary>
    /// Wrapper around <code>FileSystemWatcher</code> that tries to uniqueify events,
    /// because <code>FileSystemWatcher</code> raises lots of duplicates.
    /// </summary>
    public sealed class DirectoryWatcher : IDisposable
    {
        /// <summary>
        /// The directory being watched.
        /// </summary>
        public string Directory { get; private set; }

        /// <summary>
        /// The period in milliseconds at which the <code>DirectoryWatcher</code>
        /// raises the <code>ChangedFiles</code> event.
        /// </summary>
        public int TimerPeriodMilliseconds { get; private set; }

        /// <summary>
        /// The <code>ChangedFiles event is raised periodically whenever there</code>
        /// are file system events.
        /// </summary>
        public event EventHandler<DirectoryWatcherEventArgs> ChangedFiles;

        /// <summary>
        /// The list of files to ignore that the <code>DirectoryWatcher</code>
        /// was configured with. Can be null.
        /// </summary>
        public IEnumerable<string> FilesToIgnore
        {
            get { return _FilesToIgnore; }
        }

        /// <summary>
        /// The list of directories to ignore that the <code>DirectoryWatcher</code>
        /// was configured with. Can be null.
        /// </summary>
        public IEnumerable<string> DirectoriesToIgnore
        {
            get { return _DirectoriesToIgnore; }
        }


        public static int DefaultTimerPeriodMilliseconds { get { return 100; } }
        HashSet<string> _FilesToIgnore;
        List<string> _DirectoriesToIgnore;
        readonly FileSystemWatcher Watcher;
        readonly List<FileSystemEventArgs> NotifiedEvents;
        readonly Timer Timer;
        bool Disposed;

        /// <summary>
        /// Create a new <code>DirectoryWatcher</code> on <paramref name="directory"/>
        /// using the default timeout. Does not ignore any files or directories.
        /// </summary>
        /// <param name="directory">The directory to watch.</param>
        public DirectoryWatcher(string directory)
            : this(directory, DefaultTimerPeriodMilliseconds, null, null)
        {
        }

        /// <summary>
        /// Create a new <code>DirectoryWatcher</code> on <paramref name="directory"/>
        /// using the timeout of <paramref name="timerPeriodMilliseconds"/>. Does not ignore any files or directories.
        /// </summary>
        /// <param name="directory">The directory to watch.</param>
        /// <param name="timerPeriodMilliseconds">The timeout. Must be a postive number.</param>
        public DirectoryWatcher(string directory, int timerPeriodMilliseconds)
            : this(directory, timerPeriodMilliseconds, null, null)
        {
        }

        /// <summary>
        /// Create a new <code>DirectoryWatcher</code> on <paramref name="directory"/>
        /// using the timeout of <paramref name="timerPeriodMilliseconds"/>. Optionally
        /// ignore specified files or directories.
        /// </summary>
        /// <param name="directory">The directory to watch.</param>
        /// <param name="timerPeriodMilliseconds">The timeout. Must be a postive number.</param>
        /// <param name="filesToIgnore">List of files to ignore. Can be null or empty.</param>
        /// <param name="directoriesToIgnore">List of directories to ignore. Can be null or empty.</param>
        public DirectoryWatcher
            (
            string directory,
            int timerPeriodMilliseconds,
            IEnumerable<string> filesToIgnore,
            IEnumerable<string> directoriesToIgnore
            )
        {
            Directory = directory.ThrowIfDirectoryDoesNotExist("directory");
            TimerPeriodMilliseconds = timerPeriodMilliseconds.ThrowIfLessThan(1, "timerPeriodMilliseconds");

            Watcher = new FileSystemWatcher();
            NotifiedEvents = new List<FileSystemEventArgs>();
            Timer = new Timer(OnTimeout, null, TimerPeriodMilliseconds, TimerPeriodMilliseconds);

            if (filesToIgnore != null && filesToIgnore.Any())
            {
                _FilesToIgnore = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var file in filesToIgnore)
                    _FilesToIgnore.Add(file);
            }

            if (directoriesToIgnore != null && directoriesToIgnore.Any())
            {
                _DirectoriesToIgnore = new List<string>();
                _DirectoriesToIgnore.AddRange(directoriesToIgnore);
            }
        }


        public void Start()
        {
            ThrowIfDisposed();

            Watcher.Path = Directory;
            Watcher.IncludeSubdirectories = true;
            Watcher.Created += Watcher_Fired;
            Watcher.Changed += Watcher_Fired;
            Watcher.Deleted += Watcher_Fired;
            Watcher.Renamed += Watcher_Fired;

            Watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            ThrowIfDisposed();

            Watcher.EnableRaisingEvents = false;
            Timer.Dispose();
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                Watcher.Dispose();
                Timer.Dispose();
                Disposed = true;
            }
        }

        bool ShouldIgnore(string filename)
        {
            return (_FilesToIgnore != null && _FilesToIgnore.Contains(filename)) ||
                   (_DirectoriesToIgnore != null && _DirectoriesToIgnore.Any(d => filename.StartsWith(d, StringComparison.OrdinalIgnoreCase)));
        }

        void Watcher_Fired(object sender, FileSystemEventArgs e)
        {
            if (!ShouldIgnore(e.FullPath))
            { 
                lock (NotifiedEvents)
                { 
                    NotifiedEvents.Add(e);
                }
            }
        }

        void OnTimeout(object state)
        {
            ThrowIfDisposed();

            var notifications = new List<FileSystemEventArgs>();

            lock (NotifiedEvents)
            { 
                if (NotifiedEvents.Count == 0)
                    return;

                // When the timer fires, get all pending notifications from the queue,
                // simplify/uniqueify them, and yield them as events. This eliminates
                // duplicate events that the FileSystemWatcher raises.
                // Do this even if there is nobody to receive the events, because it
                // prevents the queue from growing forever.
                notifications.AddRange(NotifiedEvents);
                NotifiedEvents.Clear();
            }

            if (notifications.Count == 0)
                return;

            var evt = ChangedFiles;
            if (evt != null)
            {
                var args = new DirectoryWatcherEventArgs(notifications.Distinct());
                evt(this, args);
            }
        }

        void ThrowIfDisposed()
        {
            if (Disposed)
                throw new InvalidOperationException("The DirectoryWatcher is already disposed.");
        }
    }
}
