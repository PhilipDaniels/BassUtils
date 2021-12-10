using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Dawn;

namespace BassUtils.Data
{
    /// <summary>
    /// Extensions to the <seealso cref="DbDataReader"/> class.
    /// This class allows us to make asynchronous calls which are not available down on the
    /// <seealso cref="IDataReader"/> interface.
    /// </summary>
    public static class DbDataReaderExtensions
    {
        /// <summary>
        /// Calls <c>Read</c> on the <paramref name="reader"/> and throws
        /// an exception if it returns false - so this method will either
        /// throw or position you on the first record.
        /// </summary>
        public static async Task ReadOneAsync(this DbDataReader reader)
        {
            if (!await reader.ReadAsync().ConfigureAwait(false))
            {
                throw new Exception("No rows returned in " + nameof(ReadOneAsync) + "()");
            }
        }

        /// <summary>
        /// Hydrates a single object from the <paramref name="reader"/>. If 0 or more than
        /// 1 rows are returned by the reader then an exception is thrown.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="reader">The data reader to iterate.</param>
        /// <param name="hydrationFunction">A delegate that can construct objects of type T.</param>
        /// <returns>A single object of the specified type.</returns>
        public static async Task<T> HydrateOneAsync<T>(this DbDataReader reader, Func<IDataRecord, T> hydrationFunction)
        {
            Guard.Argument(reader, nameof(reader)).NotNull();
            Guard.Argument(hydrationFunction, nameof(hydrationFunction)).NotNull();

            if (!await reader.ReadAsync().ConfigureAwait(false))
            {
                throw new Exception("No rows returned in " + nameof(HydrateOneAsync) + "()");
            }

            var value = hydrationFunction(reader);

            if (await reader.ReadAsync().ConfigureAwait(false))
            {
                throw new Exception("More than 1 row returned in " + nameof(HydrateOneAsync) + "()");
            }

            return value;
        }

        /// <summary>
        /// Create an object of the specified type for every row in the DataReader.
        /// A delegate is asked to do the actual object construction.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="reader">The data reader to iterate.</param>
        /// <param name="hydrationFunction">A delegate that can construct objects of type T.</param>
        /// <returns>An enumerable of objects of the specified type.</returns>
        public static async IAsyncEnumerable<T> HydrateAllAsync<T>(this DbDataReader reader, Func<IDataRecord, T> hydrationFunction)
        {
            Guard.Argument(reader, nameof(reader)).NotNull();
            Guard.Argument(hydrationFunction, nameof(hydrationFunction)).NotNull();

            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                yield return hydrationFunction(reader);
            }
        }
    }
}
