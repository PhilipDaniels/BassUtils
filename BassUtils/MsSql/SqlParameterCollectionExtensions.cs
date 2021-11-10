using System;
using Microsoft.Data.SqlClient;

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
        /// <param name="collection">The parameter collection to add to.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value to be added. DBNull.Value will be used for null values.</param>
        /// <returns>A Microsoft.Data.SqlClient.SqlParameter object.</returns>
        public static SqlParameter AddWithNullableValue(
            this SqlParameterCollection collection,
            string parameterName,
            object value)
        {
            if (value == null)
            {
                return collection.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                return collection.AddWithValue(parameterName, value);
            }
        }
    }
}
