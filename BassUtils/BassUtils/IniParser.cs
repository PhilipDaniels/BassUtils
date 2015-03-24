using System;
using System.Collections.Generic;
using System.Linq;

namespace BassUtils
{
    /// <summary>
    /// Dead simple class to read INI files.
    /// Based on https://gist.github.com/grumly57/5725301
    /// Better than NINI because it allows double quotes in values and I've
    /// hacked it to support multiple line values.
    /// There is an IniSection class which is defined at the bottom of
    /// this file. The entire file is self contained.
    /// </summary>
    public sealed class IniParser
    {
        IDictionary<string, IniSection> iniSections { get; set; }

        /// <summary>
        /// Initialize an INI file from a string.
        /// </summary>
        /// <param name="iniData">The INI data content (such as the text read from a file).</param>
        public IniParser(string iniData)
            : this(iniData, false)
        {
        }

        /// <summary>
        /// Initialize an INI file from a string.
        /// </summary>
        /// <param name="iniData">The INI data content (such as the text read from a file).</param>
        /// <param name="coalesceLineContinuations">If true, the parser will consider a slash '\' at the
        /// end of a line to be a line continuation, and remove it before parsing, forming the
        /// value into one long string.</param>
        public IniParser(string iniData, bool coalesceLineContinuations)
        {
            iniData.ThrowIfNullOrWhiteSpace("iniData");

            iniSections = new Dictionary<string, IniSection>(StringComparer.OrdinalIgnoreCase);

            if (coalesceLineContinuations)
            {
                iniData = iniData.Replace("\\" + Environment.NewLine, "");
            }

            Parse(iniData);
        }

        void Parse(string iniData)
        {
            var currentSection = new IniSection("");
            iniSections.Add(currentSection.Name, currentSection);

            foreach (var l in iniData.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                              .Select((t, i) => new
                              {
                                  idx = i,
                                  text = t.Trim()
                              }))
            // .Where(t => !string.IsNullOrWhiteSpace(t) && !t.StartsWith(";")))
            {
                var line = l.text;

                if (line.StartsWith(";", StringComparison.OrdinalIgnoreCase) || String.IsNullOrWhiteSpace(line))
                {
                    //currentSection.Add(";" + l.idx.ToString(), line);
                    continue;
                }

                if (line.StartsWith("[", StringComparison.OrdinalIgnoreCase) && line.EndsWith("]", StringComparison.OrdinalIgnoreCase))
                {
                    string sectionName = line.Substring(1, line.Length - 2);
                    currentSection = new IniSection(sectionName);
                    iniSections.Add(currentSection.Name, currentSection);
                    continue;
                }

                var idx = line.IndexOf("=", StringComparison.OrdinalIgnoreCase);
                if (idx == -1)
                {
                    // Attempt an alternative.
                    idx = line.IndexOf(":", StringComparison.OrdinalIgnoreCase);
                }

                if (idx == -1)
                {
                    currentSection[line] = "";
                }
                else
                {
                    string key = line.Substring(0, idx).Trim();
                    string value = line.Substring(idx + 1).Trim();
                    currentSection[key] = value;
                }
            }
        }

        /// <summary>
        /// Get a parameter value at the root level.
        /// </summary>
        /// <param name="key">Parameter key.</param>
        /// <returns>Value of the <paramref name="key"/>.</returns>
        public string GetValue(string key)
        {
            key.ThrowIfNull("key");

            return GetValue("", key, "");
        }

        /// <summary>
        /// Get a parameter value in a section.
        /// </summary>
        /// <param name="section">Name of the section.</param>
        /// <param name="key">Parameter key.</param>
        /// <returns>Value of the <paramref name="key"/>.</returns>
        public string GetValue(string section, string key)
        {
            section.ThrowIfNull("section");
            key.ThrowIfNull("key");

            return GetValue(section, key, "");
        }

        /// <summary>
        /// Get a parameter value in a section with a default value if not found.
        /// </summary>
        /// <param name="section">Name of the section.</param>
        /// <param name="key">Parameter key.</param>
        /// /// <param name="defaultIfNotFound">Default to be returned if no value found.</param>
        /// <returns>Value of the <paramref name="key"/>.</returns>
        public string GetValue(string section, string key, string defaultIfNotFound)
        {
            if (!iniSections.ContainsKey(section))
                return defaultIfNotFound;

            if (!iniSections[section].ContainsKey(key))
                return defaultIfNotFound;

            return iniSections[section][key];
        }

        /// <summary>
        /// Get all the section names.
        /// </summary>
        /// <returns>Sequence of section names.</returns>
        public IEnumerable<IniSection> Sections
        {
            get
            {
                return from s in iniSections
                       where !String.IsNullOrEmpty(s.Key)
                       select s.Value;
            }
        }

        /// <summary>
        /// Get all the keys names in a section.
        /// </summary>
        /// <param name="section">Name of the section.</param>
        /// <returns>Sequence of key names.</returns>
        public IEnumerable<string> GetKeys(string section)
        {
            section.ThrowIfNull("section");

            if (!iniSections.ContainsKey(section))
                return new string[0];

            return iniSections[section].Keys;
        }
    }






    /// <summary>
    /// Represents one section of the INI file.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
        Justification="By design")]
    public sealed class IniSection : IDictionary<string, string>
    {
        readonly IDictionary<string, string> InnerDict;

        /// <summary>
        /// Returns the name of the section.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Creates a new IniSection.
        /// </summary>
        /// <param name="name">Name of the section.</param>
        public IniSection(string name)
        {
            Name = name.ThrowIfNull("name");
            InnerDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Adds a new key-value to the section.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, string value)
        {
            InnerDict.Add(key, value);
        }

        /// <summary>
        /// Returns whether the section has a particular key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>True if the section has the key, false otherwise.</returns>
        public bool ContainsKey(string key)
        {
            return InnerDict.ContainsKey(key);
        }

        /// <summary>
        /// Gets all the keys in the section.
        /// </summary>
        public ICollection<string> Keys
        {
            get { return InnerDict.Keys; }
        }

        /// <summary>
        /// Removes a key from the section.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <returns>True if the key was successfully removed from the section, false otherwise.
        /// This method also returns false if item is not found in section.
        /// </returns>
        public bool Remove(string key)
        {
            return InnerDict.Remove(key);
        }

        /// <summary>
        /// Trys to get the value associated with a particular key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value. Only valid if the method returns true.</param>
        /// <returns>True if the key exists in the section, false otherwise.</returns>
        public bool TryGetValue(string key, out string value)
        {
            return InnerDict.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets all the values in the section.
        /// </summary>
        public ICollection<string> Values
        {
            get { return InnerDict.Values; }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value for that key.</returns>
        public string this[string key]
        {
            get
            {
                return InnerDict[key];
            }
            set
            {
                InnerDict[key] = value;
            }
        }

        /// <summary>
        /// Adds a new key-value pair to the section.
        /// </summary>
        /// <param name="item">The key-value parir to add.</param>
        public void Add(KeyValuePair<string, string> item)
        {
            InnerDict.Add(item);
        }

        /// <summary>
        /// Clears the section (removes all keys and values).
        /// </summary>
        public void Clear()
        {
            InnerDict.Clear();
        }

        /// <summary>
        /// Checks to see if the section contains the specified key-value pair.
        /// </summary>
        /// <param name="item">The key-value parir to check for.</param>
        /// <returns>True if the section contains the key-value pair, false otherwise.</returns>
        public bool Contains(KeyValuePair<string, string> item)
        {
            return InnerDict.Contains(item);
        }

        /// <summary>
        /// Copies the keys and values in the section to an array starting at a specified index.
        /// The array must be large enough to receieve all items.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The index to start copying to.</param>
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            InnerDict.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns the number of keys in the section.
        /// </summary>
        public int Count
        {
            get { return InnerDict.Count; }
        }

        /// <summary>
        /// Returns true if the section is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return InnerDict.IsReadOnly; }
        }

        /// <summary>
        /// Removes a key-value pair from the section.
        /// </summary>
        /// <param name="item">The key-value pair to remove.</param>
        /// <returns>True if item was successfully removed from the section, false otherwise.
        /// This method also returns false if the item is not found in section.
        /// </returns>
        public bool Remove(KeyValuePair<string, string> item)
        {
            return InnerDict.Remove(item);
        }

        /// <summary>
        /// Gets an enumerator over the section.
        /// </summary>
        /// <returns>Enumerator object.</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return InnerDict.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator over the section.
        /// </summary>
        /// <returns>Enumerator object.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return InnerDict.GetEnumerator();
        }
    }
}
