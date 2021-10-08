using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dawn;

namespace ClassLibrary1
{
    /// <summary>
    /// Class to read INI data and parse it out into sections.
    /// This class supports duplicate keys (the latest key wins) and keys without values, for example
    ///     [ValidNames]
    ///     Philip
    /// The returned data structure is immutable.
    /// The names of keys and section names can be configured to be case-sensitive or not.
    /// </summary>
    /// <remarks>
    /// This class does not provide any support for writing or modifying INI files, and it ignores comments.
    /// If more sophisticated support is required, the package at <code>https://www.nuget.org/packages/ini-parser-netstandard/</code>
    /// is recommended.
    /// </remarks>
    [DebuggerDisplay("{iniSections.Count} sections")]
    public class IniData
    {
        private Dictionary<string, IniSection> iniSections;

        private IniData()
        {
        }

        /// <summary>
        /// Get all the section names.
        /// </summary>
        /// <returns>Sequence of section names.</returns>
        public IEnumerable<IniSection> Sections
        {
            get
            {
                return iniSections.Values;
            }
        }

        /// <summary>
        /// Gets a section by name.
        /// </summary>
        /// <param name="section">The section name.</param>
        /// <returns>The corresponding section.</returns>
        public IniSection this[string section]
        {
            get
            {
                if (!iniSections.ContainsKey(section))
                {
                    throw new KeyNotFoundException("The section '" + section + "' does not exist.");
                }

                return iniSections[section];
            }
        }

        /// <summary>
        /// Parses the specified ini data and returns it as a read-only structure.
        /// </summary>
        /// <param name="iniData">The INI data content (such as the text read from a file).</param>
        /// <param name="coalesceLineContinuations">If true, the parser will consider a slash '\' at the
        /// end of a line to be a line continuation, and remove it before parsing, forming the
        /// value into one long string.</param>
        /// <param name="stringComparer">Type of string comparer to use for section names and keys.</param>
        /// <returns>The parsed INI data.</returns>
        public static IniData Parse(string iniData, bool coalesceLineContinuations, StringComparer stringComparer)
        {
            Guard.Argument(iniData, nameof(iniData)).NotNull();
            Guard.Argument(stringComparer, nameof(stringComparer)).NotNull();

            var result = new IniData();
            result.iniSections = new Dictionary<string, IniSection>(stringComparer);

            if (coalesceLineContinuations)
            {
                iniData = iniData.Replace("\\" + Environment.NewLine, string.Empty);
            }

            // This section with a blank name will contain any top level keys.
            var currentSection = new IniSection(string.Empty, stringComparer);
            result.iniSections[currentSection.Name] = currentSection;

            foreach (string line in iniData.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).TrimAll())
            {
                // A comment.
                if (line.StartsWith(";", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                // A section name.
                if (line.StartsWith("[", StringComparison.OrdinalIgnoreCase) && line.EndsWith("]", StringComparison.OrdinalIgnoreCase))
                {
                    string sectionName = line.Substring(1, line.Length - 2).Trim();
                    currentSection = new IniSection(sectionName, stringComparer);
                    result.iniSections[currentSection.Name] = currentSection;
                    continue;
                }

                var idx = line.IndexOf("=", StringComparison.OrdinalIgnoreCase);
                if (idx == -1)
                {
                    // A key with no value. Just store the key.
                    currentSection[line] = string.Empty;
                }
                else
                {
                    string key = line.Substring(0, idx).Trim();
                    string value = line.Substring(idx + 1).Trim();
                    currentSection[key] = value;
                }
            }

            // Many files don't have a root section. Don't confuse by including it
            // if we don't need to.
            if (result.iniSections[string.Empty].Count == 0)
            {
                result.iniSections.Remove(string.Empty);
            }

            return result;
        }

        /// <summary>
        /// Get a value at the root level.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Value of the <paramref name="key"/>.</returns>
        public string GetValue(string key)
        {
            Guard.Argument(key, nameof(key)).NotNull();

            return GetValue(string.Empty, key, string.Empty);
        }

        /// <summary>
        /// Get a value in a section.
        /// </summary>
        /// <param name="section">Name of the section.</param>
        /// <param name="key">The key.</param>
        /// <returns>Value of the <paramref name="key"/>.</returns>
        public string GetValue(string section, string key)
        {
            Guard.Argument(section, nameof(section)).NotNull();
            Guard.Argument(key, nameof(key)).NotNull();

            return GetValue(section, key, string.Empty);
        }

        /// <summary>
        /// Get a value in a section with a default value if not found.
        /// </summary>
        /// <param name="section">Name of the section.</param>
        /// <param name="key">The key.</param>
        /// /// <param name="defaultIfNotFound">Default to be returned if no value found.</param>
        /// <returns>Value of the <paramref name="key"/>.</returns>
        public string GetValue(string section, string key, string defaultIfNotFound)
        {
            Guard.Argument(section, nameof(section)).NotNull();
            Guard.Argument(key, nameof(key)).NotNull();

            if (!iniSections.ContainsKey(section))
            {
                return defaultIfNotFound;
            }

            if (!iniSections[section].ContainsKey(key))
            {
                return defaultIfNotFound;
            }

            return iniSections[section][key];
        }
    }
}
