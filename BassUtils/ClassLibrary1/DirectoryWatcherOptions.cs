using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Dawn;

namespace ClassLibrary1
{
    /// <summary>
    /// An options object for the <seealso cref="DirectoryWatcher"/> class.
    /// </summary>
    public class DirectoryWatcherOptions
    {
        /// <summary>
        /// Returns the default number of milliseconds between event raisings, if there are
        /// notifications to be returned.
        /// </summary>
        public static int DefaultTimerPeriodMilliseconds { get { return 100; } }

        private int timerPeriodMilliseconds;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryWatcherOptions"/> class. This
        /// will perform a recursive watch of all files on the default timer period using a
        /// <seealso cref="StringComparison.OrdinalIgnoreCase"/> comparison type for files and
        /// directories to ignore.
        /// </summary>
        public DirectoryWatcherOptions()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryWatcherOptions"/> class. This
        /// will perform a recursive watch of all files on the default timer period using a
        /// <seealso cref="StringComparison.OrdinalIgnoreCase"/> comparison type for files and
        /// directories to ignore.
        /// </summary>
        /// <param name="directory">Directory to watch.</param>
        public DirectoryWatcherOptions(string directory)
        {
            Directory = directory;
            TimerPeriodMilliseconds = DefaultTimerPeriodMilliseconds;
            IncludeSubDirectories = true;
            FileAndDirectoryComparisonType = StringComparison.OrdinalIgnoreCase;
        }

        /// <summary>
        /// The directory being watched.
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// Whether sub-directories of <see cref="Directory"/> are also being watched.
        /// </summary>
        /// <value>
        /// <c>true</c> if watching sub directories; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeSubDirectories { get; set; }

        /// <summary>
        /// The period in milliseconds at which the <code>DirectoryWatcher</code>
        /// raises the <code>ChangedFiles</code> event.
        /// </summary>
        public int TimerPeriodMilliseconds
        {
            get
            {
                return timerPeriodMilliseconds;
            }

            set
            {
                timerPeriodMilliseconds = Guard.Argument(value, nameof(value)).NotZero().NotNegative().Value;
            }
        }

        /// <summary>
        /// The list of files to ignore that the <code>DirectoryWatcher</code> was configured with.
        /// Can be null. Entries in this list must be exact matches to the filename of the files
        /// that you are monitoring (just the filename, excluding the path, for example "foo.txt".
        /// To perform exclusions based on patterns, see <see cref="PatternsToIgnore"/>.
        /// </summary>
        public List<string> FilesToIgnore { get; set; }

        /// <summary>
        /// The list of directories to ignore that the <code>DirectoryWatcher</code> was configured
        /// with. Can be null. Entries in this list must be exact matches to the full path of the
        /// files that you are monitoring. To perform exclusions based on patterns, see
        /// <see cref="PatternsToIgnore"/>.
        /// </summary>
        public List<string> DirectoriesToIgnore { get; set; }

        /// <summary>
        /// The list of patterns to ignore that the <code>DirectoryWatcher</code> was configured
        /// with. Can be null. Entries in this list are checked against the full path of the file
        /// that generated the event; if there is a match then the <code>DirectoryWatcher</code>
        /// will ignore that file.
        /// </summary>
        public IEnumerable<Regex> PatternsToIgnore { get; set; }

        /// <summary>
        /// Gets or sets the type of the string comparisong to be done when checking the
        /// <see cref="FilesToIgnore"/> and <see cref="DirectoriesToIgnore"/>. Not used by
        /// <see cref="PatternsToIgnore"/>, because you can specify the comparison via the
        /// <seealso cref="RegexOptions"/>.
        /// </summary>
        /// <value>
        /// The type of the file and directory comparison.
        /// </value>
        public StringComparison FileAndDirectoryComparisonType {get; set; }

        /// <summary>
        /// A function, which if set, will be called by the <see cref="DirectoryWatcher"/> to
        /// see if it should ignore a path. Not normally needed, most ignore operations can be
        /// specified using <see cref="FilesToIgnore"/>, <see cref="DirectoriesToIgnore"/>
        /// or <see cref="PatternsToIgnore"/>.
        /// </summary>
        /// <value>
        /// The ignore callback.
        /// </value>
        public Func<string, bool> IgnoreCallback { get; set; }
    }
}
