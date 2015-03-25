using System.Collections.Generic;

namespace BassUtils
{
    /// <summary>
    /// Extensions to the <code>ICollection</code> interface.
    /// </summary>
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Adds a collection of items to an existing collection.
        /// </summary>
        /// <typeparam name="T">The type of thing in the collection.</typeparam>
        /// <param name="sequence">The sequence to add the items to.</param>
        /// <param name="itemsToAdd">The collection of items to add.</param>
        public static void AddRange<T>(this ICollection<T> sequence, IEnumerable<T> itemsToAdd)
        {
            sequence.ThrowIfNull("sequence");
            itemsToAdd.ThrowIfNull("itemsToAdd");

            foreach (var item in itemsToAdd)
                sequence.Add(item);
        }
    }
}
