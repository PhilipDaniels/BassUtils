using System.Collections.Generic;

namespace BassUtils
{
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
    }
}
