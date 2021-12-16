using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BassUtils.Runtime;

namespace BassUtils.ConsoleTestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            var ri = new RuntimeInformation();

            var options = new DirectoryWatcherOptions()
            {
                Directory = @"C:\temp",
                IncludeSubDirectories = true,
                FilesToIgnore = new List<string>() { "ignore.txt" },
                DirectoriesToIgnore = new List<string>() { @"C:\temp\ignoreme" },
                FileAndDirectoryComparisonType = StringComparison.Ordinal,
                PatternsToIgnore = new List<Regex>()
                {
                    new Regex(@".*\.md")
                },
                IgnoreCallback = (f) => { return f.EndsWith(".got"); }
            };

            var watcher = new DirectoryWatcher(options);

            watcher.ChangedFiles += Watcher_ChangedFiles;
            watcher.Start();
            Console.ReadKey();
        }

        private static void Watcher_ChangedFiles(object sender, DirectoryWatcherEventArgs e)
        {
            Console.WriteLine("Got {0} events.", e.FileSystemEvents.Count());
            foreach (var evt in e.FileSystemEvents)
            {
                Console.WriteLine("    CT={0} {1}", evt.ChangeType, evt.FullPath);
            }

            Console.Out.Flush();
        }
    }
}
