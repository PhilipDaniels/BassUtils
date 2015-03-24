using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

/*
 * Based on code from http://www.csvreader.com/posts/validating_datareader.php
 */

namespace BassUtils
{
    /// <summary>
    /// The SqlBulkCopyDataReader is used to wrap an existing IDataReader (which can be a BassUtils.ObjectDataReader)
    /// in order to upload the records to SQL Server using SqlBulkCopy. It adds the ability to report bulk
    /// upload errors due to column/size mismatches in a meaningful way, and optionally to trim long strings
    /// in your source data down to length, so that they fit in the destination table.
    /// Typical usage:
    /// <example>
    /// <code>
    /// using (var listingsReader = new ObjectDataReader&lt;ListingRow&gt;(listings))
    /// using (var bc = new SqlBulkCopy(conn) { DestinationTableName = "dbo.MyTable" })
    /// using (var valRdr = new SqlBulkCopyDataReader(listingsReader, conn, bc))
    /// {
    ///     bc.WriteToServer(valRdr);
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public class SqlBulkCopyDataReader : IDataReader
    {
        /// <summary>
        /// If true, the <c>SqlBulkCopyDataReader</c> will trim input strings that are too
        /// long to fit in the destination column down to size rather than throwing an exception.
        /// </summary>
        public bool TrimLongStringsInsteadOfThrowing { get; set; }

        IDataReader wrappedReader;
        bool disposed;
        int currentRecord;
        DataRow[] lookup;
        string targetTableName;
        string targetDatabaseName;
        string targetServerName;

        /// <summary>
        /// Construct a new SqlBulkCopyDataReader object.
        /// </summary>
        /// <param name="readerToWrap">The IDataReader to wrap. This is the reader that will actually be the true
        /// source of the data.</param>
        /// <param name="connection">SqlConnection to be used (required for performing queries for schema validation).</param>
        /// <param name="bulkCopy">The bulk copy object that will be used to insert data.</param>
        /// <param name="trimLongStringsInsteadOfThrowing">True if long strings in the readerToWrap should be
        /// trimmed down to fit in the target table. If false, overlong strings will cause an exception.</param>
        public SqlBulkCopyDataReader
            (
            IDataReader readerToWrap,
            SqlConnection connection,
            SqlBulkCopy bulkCopy,
            bool trimLongStringsInsteadOfThrowing
            )
        {
            connection.ThrowIfNull("conn");
            bulkCopy.ThrowIfNull("bulkCopy");
            bulkCopy.DestinationTableName.ThrowIfNullOrEmpty("bcp.DestinationTableName");

            TrimLongStringsInsteadOfThrowing = trimLongStringsInsteadOfThrowing;
            wrappedReader = readerToWrap.ThrowIfNull("reader");
            targetTableName = bulkCopy.DestinationTableName;
            targetDatabaseName = connection.Database;
            targetServerName = connection.DataSource;
            currentRecord = -1;

            ConnectionState origState = connection.State;
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            ValidateBulkCopySourceColumnMappings(bulkCopy.ColumnMappings);
            DataTable schemaTableOfDestinationTable = GetSchemaTable(connection, origState);

            lookup = new DataRow[readerToWrap.FieldCount];

            if (bulkCopy.ColumnMappings.Count > 0)
            {
                DataRow[] columns = new DataRow[schemaTableOfDestinationTable.Rows.Count];
                Dictionary<string, int> columnLookup = new Dictionary<string, int>();

                foreach (DataRow column in schemaTableOfDestinationTable.Rows)
                {
                    string columnName = (string)column["ColumnName"];
                    int columnOrdinal = (int)column["ColumnOrdinal"];
                    columns[columnOrdinal] = column;
                    columnLookup[columnName] = columnOrdinal;
                }

                ValidateBulkCopyDestinationColumnMappings(bulkCopy.ColumnMappings, columnLookup, columns);
                CreateLookupFromColumnMappings(bulkCopy.ColumnMappings, schemaTableOfDestinationTable);
            }
            else
            {
                foreach (DataRow column in schemaTableOfDestinationTable.Rows)
                {
                    int columnOrdinal = (int)column["ColumnOrdinal"];
                    lookup[columnOrdinal] = column;
                }
            }
        }

        void CreateLookupFromColumnMappings
            (
            SqlBulkCopyColumnMappingCollection mappings,
            DataTable schemaTableOfDestinationTable
            )
        {
            // create lookup dest column definition by source index
            foreach (SqlBulkCopyColumnMapping mapping in mappings)
            {
                int sourceIndex = -1;
                var sourceColumn = new SqlName(mapping.SourceColumn);
                if (!String.IsNullOrEmpty(sourceColumn.Name))
                {
                    sourceIndex = wrappedReader.GetOrdinal(sourceColumn.Name);
                }
                else
                {
                    sourceIndex = mapping.SourceOrdinal;
                }

                DataRow destColumnDef = null;
                var destColumn = new SqlName(mapping.DestinationColumn);
                if (!String.IsNullOrEmpty(destColumn.Name))
                {
                    destColumnDef = schemaTableOfDestinationTable.Rows.Cast<DataRow>().
                                    Single(c => (string)c["ColumnName"] == destColumn.Name);

                    //foreach (DataRow column in schemaTableOfDestinationTable.Rows)
                    //{
                    //    if ((string)column["ColumnName"] == destColumn.Name)
                    //    {
                    //        destColumnDef = column;
                    //        break;
                    //    }
                    //}
                }
                else
                {
                    destColumnDef = schemaTableOfDestinationTable.Rows.Cast<DataRow>().
                                    Single(c => (int)c["ColumnOrdinal"] == mapping.DestinationOrdinal);

                    //foreach (DataRow column in schemaTableOfDestinationTable.Rows)
                    //{
                    //    if ((int)column["ColumnOrdinal"] == mapping.DestinationOrdinal)
                    //    {
                    //        destColumnDef = column;
                    //        break;
                    //    }
                    //}
                }

                lookup[sourceIndex] = destColumnDef;
            }
        }

        DataTable GetSchemaTable(SqlConnection conn, ConnectionState origState)
        {
            DataTable schemaTable = null;

            try
            {
                schemaTable = conn.GetSchemaTable(targetTableName);
            }
            catch (SqlException ex)
            {
                if (ex.Message.StartsWith("Invalid object name", StringComparison.OrdinalIgnoreCase))
                {
                    throw new SchemaException
                        (
                        "Destination table " + targetTableName +
                        " does not exist in database " + conn.Database +
                        " on server " + conn.DataSource + ".",
                        ex
                        );
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (origState == ConnectionState.Closed)
                {
                    conn.Close();
                }
            }

            return schemaTable;
        }

        /// <summary>
        /// Check that every column specified in the mappings collection exists
        /// in the wrapped reader.
        /// </summary>
        void ValidateBulkCopySourceColumnMappings(SqlBulkCopyColumnMappingCollection mappings)
        {
            foreach (SqlBulkCopyColumnMapping mapping in mappings)
            {
                var sc = new SqlName(mapping.SourceColumn);

                if (!String.IsNullOrEmpty(sc.Name))
                {
                    if (wrappedReader.GetOrdinal(sc.Name) == -1)
                    {
                        string bestFit = wrappedReader.GetColumns().SingleOrDefault(c => c.Equals(sc.Name, StringComparison.OrdinalIgnoreCase));

                        if (bestFit == null)
                        {
                            throw new SchemaException("Source column " + mapping.SourceColumn + " does not exist in source.");
                        }
                        else
                        {
                            throw new SchemaException
                                (
                                "Source column " + mapping.SourceColumn + " does not exist in source." +
                                " Column name mappings are case specific and best found match is " + bestFit + "."
                                );
                        }
                    }
                }
                else
                {
                    if (mapping.SourceOrdinal < 0 || mapping.SourceOrdinal >= wrappedReader.FieldCount)
                    {
                        throw new SchemaException("No column exists at index " + mapping.SourceOrdinal + " in source.");
                    }
                }
            }
        }

        void ValidateBulkCopyDestinationColumnMappings
            (
            SqlBulkCopyColumnMappingCollection mappings,
            Dictionary<string, int> columnLookup,
            DataRow[] columns
            )
        {
            foreach (SqlBulkCopyColumnMapping mapping in mappings)
            {
                var destColumn = new SqlName(mapping.DestinationColumn);

                if (!String.IsNullOrEmpty(destColumn.Name))
                {
                    if (!columnLookup.ContainsKey(destColumn.Name))
                    {
                        // If we can't find an exact match by case, try for a case-insensitive match.
                        string bestFit = columnLookup.Keys.SingleOrDefault(c => c.Equals(destColumn.Name, StringComparison.OrdinalIgnoreCase));

                        if (bestFit == null)
                        {
                            throw new SchemaException
                                (
                                "Destination column " + mapping.DestinationColumn +
                                " does not exist in destination table " + targetTableName +
                                " in database " + targetDatabaseName +
                                " on server " + targetServerName + "."
                                );
                        }
                        else
                        {
                            throw new SchemaException
                                (
                                "Destination column " + mapping.DestinationColumn +
                                " does not exist in destination table " + targetTableName +
                                " in database " + targetDatabaseName +
                                " on server " + targetServerName +
                                ". Column name mappings are case specific and best found match is " + bestFit + "."
                                );
                        }
                    }
                }
                else
                {
                    if (mapping.DestinationOrdinal < 0 || mapping.DestinationOrdinal >= columns.Length)
                    {
                        throw new SchemaException
                            (
                            "No column exists at index " + mapping.DestinationOrdinal +
                            " in destination table " + targetTableName +
                            " in database " + targetDatabaseName +
                            " on server " + targetServerName + "."
                            );
                    }
                }
            }
        }

        /// <summary>
        /// Returns the current record.
        /// </summary>
        public int CurrentRecord
        {
            get
            {
                return currentRecord;
            }
        }

        /// <summary>
        /// Disposes the data reader.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                // managed resource releases
                if (disposing)
                {
                }

                // unmanaged resource releases
                (wrappedReader as IDisposable).Dispose();
                wrappedReader = null;
                disposed = true;
            }
        }

        /// <summary>
        /// Disposes the data reader.
        /// </summary>
        ~SqlBulkCopyDataReader()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the SQL statement.
        /// </summary>
        public int RecordsAffected
        {
            get
            {
                return wrappedReader.RecordsAffected;
            }
        }

        /// <summary>
        /// Returns true if the reader is closed.
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return disposed;
            }
        }

        /// <summary>
        /// Advances the data reader to the next result, when reading the results of batch SQL statements.
        /// </summary>
        /// <returns>True if there are more rows; otherwise, false.</returns>
        public bool NextResult()
        {
            return wrappedReader.NextResult();
        }

        /// <summary>
        /// Closes the data reader.
        /// </summary>
        public void Close()
        {
            (this as IDisposable).Dispose();
        }

        /// <summary>
        /// Advances the data reader to the next record.
        /// </summary>
        /// <returns>True if there are more rows; false otherwise.</returns>
        public bool Read()
        {
            bool canRead = wrappedReader.Read();

            if (canRead)
            {
                currentRecord++;
            }

            return canRead;
        }

        /// <summary>
        /// Gets a value indicating the depth of nesting for the current row.
        /// </summary>
        public int Depth
        {
            get
            {
                return wrappedReader.Depth;
            }
        }

        /// <summary>
        /// Returns a System.Data.DataTable that describes the column metadata of the
        /// System.Data.IDataReader.
        /// </summary>
        /// <returns>A System.Data.DataTable that describes the column metadata.</returns>
        public DataTable GetSchemaTable()
        {
            return wrappedReader.GetSchemaTable();
        }

        /// <summary>
        /// Gets the Int32 at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Int32 value.</returns>
        public int GetInt32(int i)
        {
            return wrappedReader.GetInt32(i);
        }

        /// <summary>
        /// Return the value of the specified field.
        /// </summary>
        /// <param name="name">Name of the field.</param>
        /// <returns>Value from the column.</returns>
        public object this[string name]
        {
            get
            {
                int ordinal = wrappedReader.GetOrdinal(name);

                if (ordinal > -1)
                {
                    return (this as IDataRecord).GetValue(ordinal);
                }
                else
                {
                    return wrappedReader[name];
                }
            }
        }

        /// <summary>
        /// Return the value of the specified field.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Value from column i.</returns>
        public object this[int i]
        {
            get
            {
                return (this as IDataRecord).GetValue(i);
            }
        }

        /// <summary>
        /// Return the value of the specified field.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Value from column i.</returns>
        public object GetValue(int i)
        {
            object columnValue = wrappedReader.GetValue(i);

            if (i > -1 && i < lookup.Length)
            {
                DataRow columnDef = lookup[i];
                if
                (
                    (
                        (string)columnDef["DataTypeName"] == "varchar" ||
                        (string)columnDef["DataTypeName"] == "nvarchar" ||
                        (string)columnDef["DataTypeName"] == "char" ||
                        (string)columnDef["DataTypeName"] == "nchar"
                    ) &&
                    (
                        columnValue != null &&
                        columnValue != DBNull.Value
                    )
                )
                {
                    string stringValue = columnValue.ToString();

                    int colSize = (int)columnDef["ColumnSize"];
                    if (stringValue.Length > colSize)
                    {
                        if (TrimLongStringsInsteadOfThrowing)
                        {
                            return stringValue.Substring(0, colSize);
                        }

                        string message =
                            "Column value \"" + stringValue.Replace("\"", "\\\"") + "\"" +
                            " with length " + stringValue.Length.ToString("###,##0", CultureInfo.InvariantCulture) +
                            " from source column " + (this as IDataRecord).GetName(i) +
                            " in record " + currentRecord.ToString("###,##0", CultureInfo.InvariantCulture) +
                            " does not fit in destination column " + columnDef["ColumnName"] +
                            " with length " + ((int)columnDef["ColumnSize"]).ToString("###,##0", CultureInfo.InvariantCulture) +
                            " in table " + targetTableName +
                            " in database " + targetDatabaseName +
                            " on server " + targetServerName + ".";

                        throw new SchemaException(message);
                    }
                }
            }

            return columnValue;
        }

        /// <summary>
        /// Return whether the specified field is null.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>True if the field is DbNull, false otherwise.</returns>
        public bool IsDBNull(int i)
        {
            return wrappedReader.IsDBNull(i);
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer
        /// as an array, starting at the given buffer offset.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldOffset">The index within the row from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for buffer to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of bytes read.</returns>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return wrappedReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        /// <summary>
        /// Gets the Byte at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Byte value.</returns>
        public byte GetByte(int i)
        {
            return wrappedReader.GetByte(i);
        }

        /// <summary>
        /// Gets the System.Type information corresponding to the type of System.Object
        /// that would be returned from System.Data.IDataRecord.GetValue(System.Int32).
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The System.Type information corresponding to the type of System.Object that
        /// would be returned from System.Data.IDataRecord.GetValue(System.Int32).
        /// </returns>
        public Type GetFieldType(int i)
        {
            return wrappedReader.GetFieldType(i);
        }

        /// <summary>
        /// Gets the Decimal at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Decimal value.</returns>
        public decimal GetDecimal(int i)
        {
            return wrappedReader.GetDecimal(i);
        }

        /// <summary>
        /// Populates an array of objects with the column values of the current record.
        /// </summary>
        /// <param name="values">An array of System.Object to copy the attribute fields into.</param>
        /// <returns>The number of instances of System.Object in the array.</returns>
        public int GetValues(object[] values)
        {
            return wrappedReader.GetValues(values);
        }

        /// <summary>
        /// Gets the name of the column at ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Name of the field.</returns>
        public string GetName(int i)
        {
            return wrappedReader.GetName(i);
        }

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        /// <returns>Number of columns.</returns>
        public int FieldCount
        {
            get
            {
                return wrappedReader.FieldCount;
            }
        }

        /// <summary>
        /// Gets the Int64 at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Int64 value.</returns>
        public long GetInt64(int i)
        {
            return wrappedReader.GetInt64(i);
        }

        /// <summary>
        /// Gets the double at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Double value.</returns>
        public double GetDouble(int i)
        {
            return wrappedReader.GetDouble(i);
        }

        /// <summary>
        /// Gets the boolean at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Boolean value.</returns>
        public bool GetBoolean(int i)
        {
            return wrappedReader.GetBoolean(i);
        }

        /// <summary>
        /// Gets the Guid at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Guid value.</returns>
        public Guid GetGuid(int i)
        {
            return wrappedReader.GetGuid(i);
        }

        /// <summary>
        /// Gets the DateTime at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>DateTime value.</returns>
        public DateTime GetDateTime(int i)
        {
            return wrappedReader.GetDateTime(i);
        }

        /// <summary>
        /// Gets the column ordinal for the column with the specified name.
        /// </summary>
        /// <param name="name">Name of the column.</param>
        /// <returns>Corresponding olumn ordinal.</returns>
        public int GetOrdinal(string name)
        {
            return wrappedReader.GetOrdinal(name);
        }

        /// <summary>
        /// Gets the data type information for the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The data type information for the specified field.</returns>
        public string GetDataTypeName(int i)
        {
            return wrappedReader.GetDataTypeName(i);
        }

        /// <summary>
        /// Gets the float at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Float value.</returns>
        public float GetFloat(int i)
        {
            return wrappedReader.GetFloat(i);
        }

        /// <summary>
        /// Returns an IDataReader for the specified column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>An IDataReader.</returns>
        public IDataReader GetData(int i)
        {
            return wrappedReader.GetData(i);
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer
        /// as an array, starting at the given buffer offset.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldoffset">The index within the row from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for buffer to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of characters read.</returns>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return wrappedReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        /// <summary>
        /// Gets the string at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>String value.</returns>
        public string GetString(int i)
        {
            return (string)(this as IDataRecord).GetValue(i);
        }

        /// <summary>
        /// Gets the char at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Char value.</returns>
        public char GetChar(int i)
        {
            return wrappedReader.GetChar(i);
        }

        /// <summary>
        /// Gets the Int16 (short) at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Int16 (short) value.</returns>
        public short GetInt16(int i)
        {
            return wrappedReader.GetInt16(i);
        }
    }
}
