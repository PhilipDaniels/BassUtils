using System.Collections.Generic;

namespace BassUtils
{
    /// <summary>
    /// Extensions to the Expando (really, IDictionary&lt;string, object&gt;) class.
    /// </summary>
    public static class ExpandoExtensions
    {
        /// <summary>
        /// Merges two dynamics by copying the properties of <paramref name="source"/> over
        /// to <paramref name="target"/>. In the event of name clashes, the properties in <paramref name="target"/>
        /// will be overwritten.
        /// </summary>
        /// <param name="target">The target of the merge.</param>
        /// <param name="source">The source of the merge.</param>
        public static void Merge(this IDictionary<string, object> target, IDictionary<string, object> source)
        {
            target.ThrowIfNull("target");
            source.ThrowIfNull("source");

            foreach (var kvp in source)
                target[kvp.Key] = kvp.Value;
        }

        /// <summary>
        /// Adds a <paramref name="thing"/> to the <paramref name="target"/>, as a property
        /// called <paramref name="name"/>.
        /// </summary>
        /// <param name="target">The Expando (or dictionary) object to add to.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="thing">The object to add.</param>
        public static void SetProperty(this IDictionary<string, object> target, string name, object thing)
        {
            target.ThrowIfNull("target");
            name.ThrowIfNull("name");

            target[name] = thing;
        }
    }
}
