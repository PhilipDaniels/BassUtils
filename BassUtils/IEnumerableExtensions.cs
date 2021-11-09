using System.Collections.Generic;
using System.Data;
using BassUtils.Data;
using Dawn;

namespace BassUtils
{
    /// <summary>
    /// Extensions to <code>IEnumerable of T</code>.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Wraps the IEnumerable in a DbDataReader, having one column for each "scalar" property of the type T.  
        /// The collection will be enumerated as the client calls IDataReader.Read().
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IDataReader AsDataReader<T>(this IEnumerable<T> collection)
        {
            Guard.Argument(collection, nameof(collection)).NotNull();

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
            Guard.Argument(collection, nameof(collection)).NotNull();

            return new ObjectDataReader<T>(collection, nullConversion);
        }
    }
}
