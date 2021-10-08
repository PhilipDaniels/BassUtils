using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dawn;

namespace BassUtils.Data
{
    /// <summary>
    /// Extensions to the <code>System.Data.DataTable</code> class.
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// Sets the ReadOnly flag on the DataTable to true.
        /// </summary>
        /// <param name="dataTable">The DataTable.</param>
        public static void SetReadOnly(this DataTable dataTable)
        {
            dataTable.SetReadOnly(true);
        }

        /// <summary>
        /// Sets the ReadOnly flag on the DataTable to true or false.
        /// </summary>
        /// <param name="dataTable">The DataTable.</param>
        /// <param name="readOnly">The new value for the read-only flag.</param>
        public static void SetReadOnly(this DataTable dataTable, bool readOnly)
        {
            Guard.Argument(dataTable, nameof(dataTable)).NotNull();

            foreach (DataColumn dc in dataTable.Columns)
            {
                if (String.IsNullOrEmpty(dc.Expression))
                {
                    dc.ReadOnly = readOnly;
                }
            }
        }

        /// <summary>
        /// Copies the rows from <paramref name="sourceTable"/> to <paramref name="destinationTable"/>,
        /// but only if the columns actually exist in <paramref name="destinationTable"/>.
        /// This routine can be used to "hydrate" entities from generic results sets.
        /// It used the CopyColumns routine.
        /// </summary>
        /// <param name="sourceTable">The table to copy rows from.</param>
        /// <param name="destinationTable">The table to copy rows to.</param>
        public static void CopyRows(this DataTable sourceTable, DataTable destinationTable)
        {
            Guard.Argument(sourceTable, nameof(sourceTable)).NotNull();
            Guard.Argument(destinationTable, nameof(destinationTable)).NotNull();

            foreach (DataRow sourceRow in sourceTable.Rows)
            {
                DataRow targetRow = destinationTable.NewRow();
                sourceRow.CopyRow(targetRow);
                destinationTable.Rows.Add(targetRow);
            }
        }

        /// <summary>
        /// Return a list of all the column names in the table.
        /// </summary>
        /// <param name="dataTable">The data table that you want the column names from.</param>
        /// <returns>List of column names.</returns>
        public static IEnumerable<string> ColumnNames(this DataTable dataTable)
        {
            Guard.Argument(dataTable, nameof(dataTable)).NotNull();

            var columnNames = dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
            return columnNames;
        }

        /// <summary>
        /// Determines whether the DataTable contains any changed rows.
        /// </summary>
        /// <param name="dataTable">The table.</param>
        /// <returns>True if the table has at least one changed row, false otherwise.</returns>
        public static bool HasChangedRows(this DataTable dataTable)
        {
            Guard.Argument(dataTable, nameof(dataTable)).NotNull();

            foreach (DataRow row in dataTable.Rows)
            {
                if (!row.RowState.Equals(DataRowState.Unchanged))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
