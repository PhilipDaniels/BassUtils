using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Dawn;

namespace BassUtils
{
    /// <summary>
    /// Represents one section of the INI file.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Section is more standard than Collection for this usage.")]
    [DebuggerDisplay("Name={Name}")]
    public sealed class IniSection : IEnumerable<KeyValuePair<string, string>>, IEnumerable
    {
        private readonly IDictionary<string, string> innerDict;

        /// <summary>
        /// Creates a new IniSection.
        /// </summary>
        /// <param name="name">Name of the section.</param>
        /// <param name="keyComparer">Type of comparer to use for key name comparisons.</param>
        internal IniSection(string name, StringComparer keyComparer)
        {
            Name = Guard.Argument(name, nameof(name)).NotNull().Value.Trim();
            Guard.Argument(keyComparer, nameof(keyComparer)).NotNull();

            innerDict = new Dictionary<string, string>(keyComparer);
        }

        /// <summary>
        /// Returns the name of the section. An empty string is used to mean
        /// the section that is used to store keys that appear outside any
        /// section (so called root-level keys).
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets all the keys in the section.
        /// </summary>
        public IEnumerable<string> Keys
        {
            get { return innerDict.Keys; }
        }

        /// <summary>
        /// Gets all the values in the section.
        /// </summary>
        public IEnumerable<string> Values
        {
            get { return innerDict.Values; }
        }

        /// <summary>
        /// Returns the number of keys in the section.
        /// </summary>
        public int Count
        {
            get { return innerDict.Count; }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value for that key.</returns>
        public string this[string key]
        {
            get
            {
                if (!innerDict.ContainsKey(key))
                {
                    throw new KeyNotFoundException("The section '" + Name + "' does not contain the key '" + key + "'.");
                }

                return innerDict[key];
            }

            internal set
            {
                innerDict[key] = value;
            }
        }

        /// <summary>
        /// Returns whether the section has a particular key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>True if the section has the key, false otherwise.</returns>
        public bool ContainsKey(string key)
        {
            return innerDict.ContainsKey(key);
        }

        /// <summary>
        /// Try to get the value associated with a particular key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value. Only valid if the method returns true.</param>
        /// <returns>True if the key exists in the section, false otherwise.</returns>
        public bool TryGetValue(string key, out string value)
        {
            return innerDict.TryGetValue(key, out value);
        }

        /// <summary>
        /// Checks to see if the section contains the specified key-value pair.
        /// </summary>
        /// <param name="item">The key-value pair to check for.</param>
        /// <returns>True if the section contains the key-value pair, false otherwise.</returns>
        public bool Contains(KeyValuePair<string, string> item)
        {
            return innerDict.Contains(item);
        }

        /// <summary>
        /// Gets an enumerator over the section.
        /// </summary>
        /// <returns>Enumerator object.</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return innerDict.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator over the section.
        /// </summary>
        /// <returns>Enumerator object.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return innerDict.GetEnumerator();
        }
    }
}
