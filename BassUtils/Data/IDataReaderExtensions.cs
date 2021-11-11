using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
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
        /// Calls <c>Read</c> on the <paramref name="reader"/> and throws
        /// an exception if it returns false - so this method will either
        /// throw or position you on the first record.
        /// </summary>
        public static void ReadOne(this IDataReader reader)
        {
            if (!reader.Read())
            {
                throw new Exception("No rows returned in " + nameof(ReadOne) + "()");
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
        public static T HydrateOne<T>(this IDataReader reader, Func<IDataRecord, T> hydrationFunction)
        {
            Guard.Argument(reader, nameof(reader)).NotNull();
            Guard.Argument(hydrationFunction, nameof(hydrationFunction)).NotNull();

            if (!reader.Read())
            {
                throw new Exception("No rows returned in " + nameof(HydrateOne) + "()");
            }

            var value = hydrationFunction(reader);

            if (reader.Read())
            {
                throw new Exception("More than 1 row returned in " + nameof(HydrateOne) + "()");
            }

            return value;
        }

        /// <summary>
        /// Hydrates a single object from the <paramref name="reader"/>. If 0 rows are returned
        /// by the reader then false is returned and the out parameter is unspecified.
        /// If more than 1 rows are returned by the reader then an exception is thrown.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="reader">The data reader to iterate.</param>
        /// <param name="hydrationFunction">A delegate that can construct objects of type T.</param>
        /// <param name="value">An object of the specified type.</param>
        /// <returns>True if exactly one object could be hydrated, false otherwise.</returns>
        public static bool TryHydrateOne<T>(this IDataReader reader, Func<IDataRecord, T> hydrationFunction, out T value)
        {
            Guard.Argument(reader, nameof(reader)).NotNull();
            Guard.Argument(hydrationFunction, nameof(hydrationFunction)).NotNull();

            value = default;

            if (!reader.Read())
            {
                return false;
            }

            var temp_value = hydrationFunction(reader);

            if (reader.Read())
            {
                throw new Exception("More than 1 row returned in " + nameof(TryHydrateOne) + "()");
            }

            value = temp_value;
            return true;
        }

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
                    select rec.Hydrate<T>(hydrationFunction));
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

            var schemaTable = reader.GetSchemaTable();
            foreach (DataRow row in schemaTable.Rows)
            {
                columns.Add(row["ColumnName"].ToString());
            }

            return columns.AsReadOnly();
        }

        /// <summary>
        /// Converts an IDataReader to a DataTable.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>A DataTable object.</returns>
        public static DataTable ToDataTable(this IDataReader reader)
        {
            return MakeTableFromReader(reader, "IDataReader table");
        }

        /// <summary>
        /// Converts an IDataReader to a DataSet. Each set of records is returned as an
        /// individual table in the DataSet.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>A DataSet object with 1 or more tables.</returns>
        public static DataSet ToDataSet(this IDataReader reader)
        {
            Guard.Argument(reader, nameof(reader)).NotNull();

            var ds = new DataSet("Query Results");
            ds.Locale = CultureInfo.InvariantCulture;

            var tableNumber = 1;
            while (!reader.IsClosed)
            {
                var tableName = "Table " + tableNumber.ToString(CultureInfo.InvariantCulture);
                ds.Tables.Add(MakeTableFromReader(reader, tableName));
                tableNumber++;
            }

            return ds;
        }

        static DataTable MakeTableFromReader(IDataReader reader, string tableName)
        {
            Guard.Argument(reader, nameof(reader)).NotNull();
            Guard.Argument(tableName, nameof(tableName)).NotNull();

            var dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            dt.TableName = tableName;
            dt.Load(reader);
            return dt;
        }

        /// <summary>
        /// Copies the rows from <paramref name="sourceReader"/> to <paramref name="destinationTable"/>,
        /// but only if the columns actually exist in <paramref name="destinationTable"/>.
        /// This routine can be used to "hydrate" entities from generic results sets.
        /// </summary>
        /// <param name="sourceReader">The data reader to copy rows from.</param>
        /// <param name="destinationTable">The table to copy rows to.</param>
        public static void CopyRows(this IDataReader sourceReader, DataTable destinationTable)
        {
            Guard.Argument(sourceReader, nameof(sourceReader)).NotNull();
            Guard.Argument(destinationTable, nameof(destinationTable)).NotNull();

            var readerColumns = sourceReader.GetColumns();

            while (sourceReader.Read())
            {
                DataRow targetRow = destinationTable.NewRow();
                CopyData(readerColumns, sourceReader, targetRow);
                destinationTable.Rows.Add(targetRow);
            }
        }

        /// <summary>
        /// Copies 1 row from <paramref name="sourceReader"/> to <paramref name="destinationRow"/>,
        /// but only if the columns actually exist in <paramref name="destinationRow"/>.
        /// This routine can be used to "hydrate" entities from generic results sets.
        /// </summary>
        /// <param name="sourceReader">The row to copy columns from.</param>
        /// <param name="destinationRow">The row to copy columns to.</param>
        public static void CopyRow(this IDataReader sourceReader, DataRow destinationRow)
        {
            Guard.Argument(sourceReader, nameof(sourceReader)).NotNull();
            Guard.Argument(destinationRow, nameof(destinationRow)).NotNull();

            var readerColumns = sourceReader.GetColumns();
            CopyData(readerColumns, sourceReader, destinationRow);
        }

        /// <summary>
        /// Copies the data in the specified columns to the target.
        /// </summary>
        /// <param name="readerColumns">The reader columns.</param>
        /// <param name="sourceReader">The source reader.</param>
        /// <param name="destinationRow">The target row.</param>
        private static void CopyData(IEnumerable<string> readerColumns, IDataReader sourceReader, DataRow destinationRow)
        {
            Guard.Argument(readerColumns, nameof(readerColumns)).NotNull();
            Guard.Argument(sourceReader, nameof(sourceReader)).NotNull();
            Guard.Argument(destinationRow, nameof(destinationRow)).NotNull();

            foreach (DataColumn column in destinationRow.Table.Columns)
            {
                if (readerColumns.Contains(column.ColumnName))
                {
                    destinationRow[column.ColumnName] = sourceReader[column.ColumnName];
                }
            }
        }
    }
}
