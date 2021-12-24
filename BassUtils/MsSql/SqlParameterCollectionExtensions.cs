using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dawn;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;

namespace BassUtils.MsSql
{
    /// <summary>
    /// Extensions for the SqlParameterCollection class.
    /// </summary>
    public static class SqlParameterCollectionExtensions
    {
        /// <summary>
        /// Adds a value to the end of the <c>SqlParameterCollection</c>.
        /// Uses <c>DBNull.Value</c> if the <paramref name="value"/> is null.
        /// </summary>
        /// <param name="parameterCollection">The parameter collection to add to.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value to be added. DBNull.Value will be used for null values.</param>
        /// <returns>The new SqlParameter object.</returns>
        public static SqlParameter AddWithNullableValue(
            this SqlParameterCollection parameterCollection,
            string parameterName,
            object value)
        {
            Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
            Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();

            if (value == null)
            {
                return parameterCollection.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                return parameterCollection.AddWithValue(parameterName, value);
            }
        }

        /// <summary>
        /// Adds a table-valued parameter to the end of the <c>SqlParameterCollection</c>.
        /// </summary>
        /// <param name="parameterCollection">The parameter collection to add to.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="typeName">The name of the custom table type in SQL Server.</param>
        /// <param name="records">The collection of records to add as the parameter value.</param>
        /// <returns>The new SqlParameter object.</returns>
        public static SqlParameter AddTableValuedParameter(
            this SqlParameterCollection parameterCollection,
            string parameterName,
            string typeName,
            IEnumerable<SqlDataRecord> records
            )
        {
            Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
            Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();
            Guard.Argument(typeName, nameof(typeName)).NotNull().NotWhiteSpace();

            var prm = parameterCollection.Add(parameterName, SqlDbType.Structured);
            prm.TypeName = typeName;

            if (records == null)
            {
                prm.Value = DBNull.Value;
            }
            else
            {
                prm.Value = records.ToArray();
            }

            return prm;
        }
    }
}
