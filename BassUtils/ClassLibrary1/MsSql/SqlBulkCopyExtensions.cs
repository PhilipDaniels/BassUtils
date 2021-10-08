using System.Reflection;
using Dawn;
using Microsoft.Data.SqlClient;

namespace ClassLibrary1.MsSql
{
    /// <summary>
    /// Extension methods for the SqlBulkCopy class.
    /// </summary>
    public static class SqlBulkCopyExtensions
    {
        static FieldInfo rowsCopiedField = null;

        /// <summary>
        /// Retrieve the total number of rows copied in a SqlBulkCopy operation.
        /// </summary>
        /// <param name="bulkCopy">The bulk copy object.</param>
        /// <returns>Total number of rows copied.</returns>
        public static int TotalRowsCopied(this SqlBulkCopy bulkCopy)
        {
            Guard.Argument(bulkCopy, nameof(bulkCopy)).NotNull();

            if (rowsCopiedField == null)
            {
                rowsCopiedField = typeof(SqlBulkCopy).GetField
                    (
                    "_rowsCopied",
                    BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance
                    );
            }

            return (int)rowsCopiedField.GetValue(bulkCopy);
        }
    }
}
