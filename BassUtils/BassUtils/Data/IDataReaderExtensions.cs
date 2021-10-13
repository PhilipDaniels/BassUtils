using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Dawn;

namespace BassUtils.Data
{
    /// <summary>
    /// Extensions to the <seealso cref="IDataReader"/> class.
    /// </summary>
    public static class IDataReaderExtensions
    {
        /// <summary>
        /// Create an object of the specified type for every row in the DataReader.
        /// A delegate is asked to do the actual object construction.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="reader">The data reader to iterate.</param>
        /// <param name="hydrationFunction">A delegate that can construct objects of type T.</param>
        /// <returns>An enumerable of objects of the specified type.</returns>
        public static IEnumerable<T> HydrateAll<T>(this IDataReader reader, Func<IDataRecord, T> hydrationFunction)
        {
            Guard.Argument(reader, nameof(reader)).NotNull();
            Guard.Argument(hydrationFunction, nameof(hydrationFunction)).NotNull();

            return (from r in reader.CurrentRecords()
                    let rec = r as IDataRecord
                    select rec.Hydrate<T>(hydrationFunction)).AsEnumerable();
        }

        /// <summary>
        /// Iterate (i.e. as IEnumerable) over all the rows in the current result set.
        /// </summary>
        /// <param name="reader">The data reader to iterate.</param>
        /// <returns>An enumerable of all the rows in the reader's current resultset.</returns>
        public static IEnumerable<IDataRecord> CurrentRecords(this IDataReader reader)
        {
            Guard.Argument(reader, nameof(reader)).NotNull();

            while (reader.Read())
            {
                yield return reader;
            }
        }

        /// <summary>
        /// Gets the names of the columns in the reader.	
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>A collection of column names.</returns>
        public static ReadOnlyCollection<string> GetColumns(this IDataReader reader)
        {
            Guard.Argument(reader, nameof(reader)).NotNull();

            var columns = new List<string>();

            DataTable schemaTable = reader.GetSchemaTable();
            foreach (DataRow row in schemaTable.Rows)
            {
                columns.Add(row["ColumnName"].ToString());
            }

            return columns.AsReadOnly();
        }
    }
}
