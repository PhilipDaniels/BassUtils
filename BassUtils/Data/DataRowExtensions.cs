using System.Data;
using Dawn;

namespace BassUtils.Data
{
    /// <summary>
    /// Extensions to the <code>System.Data.DataRow</code> class.
    /// </summary>
    public static class DataRowExtensions
    {
        /// <summary>
        /// Copies the columns from <paramref name="sourceRow"/> to <paramref name="targetRow"/>,
        /// but only if the columns actually exist in <paramref name="targetRow"/>.
        /// This routine can be used to "hydrate" entities from generic results sets.
        /// </summary>
        /// <param name="sourceRow">The row to copy columns from.</param>
        /// <param name="targetRow">The row to copy columns to.</param>
        public static void CopyRow(this DataRow sourceRow, DataRow targetRow)
        {
            Guard.Argument(sourceRow, nameof(sourceRow)).NotNull();
            Guard.Argument(targetRow, nameof(targetRow)).NotNull();

            foreach (DataColumn column in targetRow.Table.Columns)
            {
                if (sourceRow.Table.Columns.Contains(column.ColumnName))
                {
                    targetRow[column.ColumnName] = sourceRow[column.ColumnName];
                }
            }
        }
    }
}
