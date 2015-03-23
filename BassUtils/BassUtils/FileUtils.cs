using System.IO;

namespace BassUtils
{
    /// <summary>
    /// Various file-system oriented utility functions.
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        /// Creates a directory if it does not exist (and returns true).
        /// If the directory already exists, returns false.
        /// </summary>
        /// <param name="directory">The directory to create.</param>
        /// <returns>True if the directory was created, false if it already existed.</returns>
        public static bool EnsureDirectory(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");

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
            fileName.ThrowIfNullOrWhiteSpace("filename");

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
            fileName.ThrowIfNullOrWhiteSpace("filename");

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
            extension.ThrowIfNull("extension");

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
            sourceDirectoryName.ThrowIfDirectoryDoesNotExist("sourceDirectoryName");
            destinationDirectoryName.ThrowIfNullOrWhiteSpace("destinationDirectoryName");

            Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(sourceDirectoryName, destinationDirectoryName, overwrite);
        }

        /// <summary>
        /// Moves a directory from one place to another.
        /// </summary>
        /// <param name="sourceDirectoryName">The directory to move.</param>
        /// <param name="destinationDirectoryName">The destination.</param>
        /// <param name="overwrite">Whether to overwrite the destination.</param>
        public static void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName, bool overwrite)
        {
            sourceDirectoryName.ThrowIfDirectoryDoesNotExist("sourceDirectoryName");
            destinationDirectoryName.ThrowIfNullOrWhiteSpace("destinationDirectoryName");

            Microsoft.VisualBasic.FileIO.FileSystem.MoveDirectory(sourceDirectoryName, destinationDirectoryName, overwrite);
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
            directory.ThrowIfDirectoryDoesNotExist("directory");

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
            directory.ThrowIfDirectoryDoesNotExist("directory");

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
            directory.ThrowIfNullOrWhiteSpace("directory");

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
            directory.ThrowIfNullOrWhiteSpace("directory");

            if (!Directory.Exists(directory))
                return;

            SetReadOnlyAttribute(directory, false, SearchOption.AllDirectories);

            foreach (string file in Directory.EnumerateFiles(directory))
                File.Delete(file);

            foreach (string dir in Directory.EnumerateDirectories(directory))
                Directory.Delete(dir, true);
        }
    }
}
