using System;
using System.IO;
using Dawn;

namespace ClassLibrary1
{
    public static class Paths
    {
        /// <summary>
        /// Creates a directory if it does not exist (and returns true).
        /// If the directory already exists, returns false.
        /// </summary>
        /// <param name="directory">The directory to create.</param>
        /// <returns>True if the directory was created, false if it already existed.</returns>
        public static bool EnsureDirectory(string directory)
        {
            Guard.Argument(directory, nameof(directory)).NotNull().NotWhiteSpace();

            if (Directory.Exists(directory))
            {
                return false;
            }
            else
            {
                Directory.CreateDirectory(directory);
                return true;
            }
        }

        /// <summary>
        /// Creates the parent directory of a file if it does not exist (and returns true).
        /// If the directory already exists, returns false.
        /// </summary>
        /// <param name="fileName">The filename whose parent directory is to be created.</param>
        /// <returns>True if the directory was created, false if it already existed.</returns>
        public static bool EnsureParentDirectory(string fileName)
        {
            Guard.Argument(fileName, nameof(fileName)).NotNull().NotWhiteSpace();

            string parentDir = Path.GetDirectoryName(fileName);
            return EnsureDirectory(parentDir);
        }

        /// <summary>
        /// Extracts a "clean" extension from a filename. By default, Path.GetExtension
        /// returns extensions with leading "." characters. This method removes them.
        /// </summary>
        /// <param name="fileName">Filename to get extension of.</param>
        /// <returns>Cleaned extension.</returns>
        public static string GetCleanExtension(string fileName)
        {
            Guard.Argument(fileName, nameof(fileName)).NotNull().NotWhiteSpace();

            string extension = CleanExtension(Path.GetExtension(fileName));
            return extension;
        }

        /// <summary>
        /// Cleans up an extension by trimming any leading '.', which the Path
        /// methods often leave on. e.g. ".cshtml" becomes "cshtml".
        /// </summary>
        /// <param name="extension">The extension to clean.</param>
        /// <returns>Cleaned extension.</returns>
        public static string CleanExtension(string extension)
        {
            Guard.Argument(extension, nameof(extension)).NotNull().NotWhiteSpace();

            extension = extension.Trim().TrimStart('.').Trim();
            return extension;
        }

        /// <summary>
        /// Copies a directory from one place to another.
        /// </summary>
        /// <param name="sourceDirectoryName">The directory to copy.</param>
        /// <param name="destinationDirectoryName">The destination.</param>
        /// <param name="overwrite">Whether to overwrite the destination.</param>
        public static void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName, bool overwrite)
        {
            Guard.Argument(sourceDirectoryName, nameof(sourceDirectoryName)).NotNull().NotWhiteSpace();
            Guard.Argument(destinationDirectoryName, nameof(destinationDirectoryName)).NotNull().NotWhiteSpace();

            throw new NotImplementedException();
            //Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(sourceDirectoryName, destinationDirectoryName, overwrite);
        }

        /// <summary>
        /// Moves a directory from one place to another.
        /// </summary>
        /// <param name="sourceDirectoryName">The directory to move.</param>
        /// <param name="destinationDirectoryName">The destination.</param>
        /// <param name="overwrite">Whether to overwrite the destination.</param>
        public static void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName, bool overwrite)
        {
            Guard.Argument(sourceDirectoryName, nameof(sourceDirectoryName)).NotNull().NotWhiteSpace();
            Guard.Argument(destinationDirectoryName, nameof(destinationDirectoryName)).NotNull().NotWhiteSpace();

            throw new NotImplementedException();
            //Microsoft.VisualBasic.FileIO.FileSystem.MoveDirectory(sourceDirectoryName, destinationDirectoryName, overwrite);
        }

        /// <summary>
        /// Sets or resets the readonly flag on the files in a directory and optionally
        /// its subdirectories.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="readOnly">True or false.</param>
        /// <param name="options">Whether to search just the top directory or all directories.</param>
        public static void SetReadOnlyAttribute
            (
            string directory,
            bool readOnly,
            SearchOption options
            )
        {
            Guard.Argument(directory, nameof(directory)).NotNull().NotWhiteSpace();

            var di = new DirectoryInfo(directory);
            foreach (var fi in di.EnumerateFiles("*", options))
            {
                if (fi.IsReadOnly != readOnly)
                    fi.IsReadOnly = readOnly;
            }
        }

        /// <summary>
        /// Sets attributes on all files in a directory and optionally its subdirectories.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="attributes">The attributes to set.</param>
        /// <param name="options">Whether to search just the top directory or all directories.</param>
        public static void SetFileAttributes
            (
            string directory,
            FileAttributes attributes,
            SearchOption options
            )
        {
            Guard.Argument(directory, nameof(directory)).NotNull().NotWhiteSpace();

            var di = new DirectoryInfo(directory);
            foreach (var fi in di.EnumerateFiles("*", options))
            {
                if (fi.Attributes != attributes)
                    fi.Attributes = attributes;
            }
        }

        /// <summary>
        /// Deletes a directory. No exception is thrown if the directory does not exist.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        public static void DeleteDirectory(string directory)
        {
            Guard.Argument(directory, nameof(directory)).NotNull().NotWhiteSpace();

            if (Directory.Exists(directory))
            {
                SetReadOnlyAttribute(directory, false, SearchOption.AllDirectories);
                Directory.Delete(directory, true);
            }
        }

        /// <summary>
        /// Deletes all files and subdirectories in a directory, but leaves the directory.
        /// The directory does not need to exist (this is a no-op).
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        public static void DeleteDirectoryContents(string directory)
        {
            Guard.Argument(directory, nameof(directory)).NotNull().NotWhiteSpace();

            if (!Directory.Exists(directory))
                return;

            SetReadOnlyAttribute(directory, false, SearchOption.AllDirectories);

            foreach (string file in Directory.EnumerateFiles(directory))
                File.Delete(file);

            foreach (string dir in Directory.EnumerateDirectories(directory))
                Directory.Delete(dir, true);
        }


        /// <summary>
        /// Normalizes a path to the directory of the EXE (actually the entry assembly).
        /// If the path is rooted then it is returned as is, otherwise a new path relative
        /// to the directory of the exe is returned. In both cases, you get a full
        /// absolute path.
        /// </summary>
        /// <remarks>
        /// Useful for easily creating paths relative to your EXE, for example
        /// <code>NormalizeToExeDirectory("Plugins")</code> gives you the Plugins
        /// folder under your EXE, but <code>NormalizeToExeDirectory("C:\temp")</code>
        /// gives you C:\temp.
        /// </remarks>
        /// <param name="path">The path to normalize.</param>
        /// <returns>Normalized path.</returns>
        public static string NormalizeToExeDirectory(string path)
        {
            string exeDir = AssemblyExtensions.GetExeDirectory();
            path = NormalizeToDirectory(path, exeDir);
            return path;
        }

        /// <summary>
        /// Normalizes a path to the specified directory.
        /// If the path is rooted then it is returned as is, otherwise a new path relative
        /// to the <paramref name="directory"/> is returned. In both cases, you get a full
        /// absolute path.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="path">The path to normalize.</param>
        /// <param name="directory">The directory to normalize relative to.</param>
        /// <returns>Normalized path.</returns>
        public static string NormalizeToDirectory(string path, string directory)
        {
            Guard.Argument(path, nameof(path)).NotNull();
            Guard.Argument(directory, nameof(directory)).NotNull();

            if (Path.IsPathRooted(path))
            {
                return path;
            }
            else
            {
                path = Path.Combine(directory, path);
                return path;
            }
        }
    }
}
