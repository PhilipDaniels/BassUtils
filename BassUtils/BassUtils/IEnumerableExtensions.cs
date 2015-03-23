using System.Collections.Generic;
using System.Data;

namespace BassUtils
{
    /// <summary>
    /// Extensions to <code>IEnumerable of T</code>.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Split an enumerable into partitions of size N.
        /// </summary>
        /// <remarks>
        /// The input enumerable is only iterated over once and no temporary storage
        /// is allocated.
        /// </remarks>
        /// <typeparam name="T">The type of element in the input enumerable.</typeparam>
        /// <param name="input">The enumerable to partition.</param>
        /// <param name="size">The desired number of elements in each partition.</param>
        /// <returns>An enumerable of enumerables, each child enumerable being at most <paramref name="size"/> elements in length.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification="By design")]
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> input, int size)
        {
            var enumerator = input.GetEnumerator();

            while (enumerator.MoveNext())
            {
                yield return NextPartition(enumerator, size);
            }
        }

        static IEnumerable<T> NextPartition<T>(IEnumerator<T> enumerator, int blockSize)
        {
            do
            {
                yield return enumerator.Current;
            }
            while (--blockSize > 0 && enumerator.MoveNext());
        }

        /// <summary>
        /// Wraps the IEnumerable in a DbDataReader, having one column for each "scalar" property of the type T.  
        /// The collection will be enumerated as the client calls IDataReader.Read().
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IDataReader AsDataReader<T>(this IEnumerable<T> collection)
        {
            collection.ThrowIfNull("collection");

            // For anonymous type projections default to flattening related objects and not prefixing columns
            // The reason being that if the programmer has taken control of the projection, the default should
            // be to expose everying in the projection and not mess with the names.
            if (typeof(T).IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false))
            {
                return new ObjectDataReader<T>(collection, NullConversion.None);
            }
            else
            {
                return new ObjectDataReader<T>(collection);
            }
        }

        /// <summary>
        /// Wraps the IEnumerable in a DbDataReader, having one column for each "scalar" property of the type T.  
        /// The collection will be enumerated as the client calls IDataReader.Read().
        /// </summary>
        /// <typeparam name="T">The element type of the collectin.</typeparam>
        /// <param name="collection">A collection to wrap in a DataReader</param>
        /// <param name="nullConversion">Whether to convert nulls.</param>
        /// <returns>An IDataReader wrapping the collection.</returns>
        public static IDataReader AsDataReader<T>(this IEnumerable<T> collection, NullConversion nullConversion)
        {
            collection.ThrowIfNull("collection");

            return new ObjectDataReader<T>(collection, nullConversion);
        }
    }
}
