using System.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace BassUtils.Oracle
{
    /// <summary>
    /// Extensions for the <c>OracleConnection</c> class.
    /// </summary>
    public static class OracleConnectionExtensions
    {
        /// <summary>
        /// Begins a new transaction on the <paramref name="connection"/> at the default isolation level and
        /// bundles it into a <seealso cref="WrappedTransaction"/> for easier disposal.
        /// </summary>
        /// <param name="connection">The Oracle connection.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A wrapped transaction object.</returns>
        public static async Task<WrappedTransaction> BeginWrappedTransactionAsync(this OracleConnection connection, CancellationToken cancellationToken = default)
        {
            var txn = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
            var oracleTxn = txn as OracleTransaction;
            return new WrappedTransaction(connection, oracleTxn);
        }

        /// <summary>
        /// Begins a new transaction on the <paramref name="connection"/> at the specified <paramref name="isolationLevel"/> and
        /// bundles it into a <seealso cref="WrappedTransaction"/> for easier disposal.
        /// </summary>
        /// <param name="connection">The Oracle connection.</param>
        /// <param name="isolationLevel">Transaction isolation level.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A wrapped transaction object.</returns>
        public static async Task<WrappedTransaction> BeginWrappedTransactionAsync(this OracleConnection connection, IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            var txn = await connection.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(false);
            var oracleTxn = txn as OracleTransaction;
            return new WrappedTransaction(connection, oracleTxn);
        }

        /// <summary>
        /// Sets a UDT value. If the value is null then <c>DBNull.Value</c> will be set.
        /// </summary>
        /// <param name="connection">The Oracle connection.</param>
        /// <param name="udt">The UDT object on which to set the value.</param>
        /// <param name="attrName">The name of the attribute - this should be in all uppercase.</param>
        /// <param name="value">The value to set.</param>
        public static void SetUdtValue(this OracleConnection connection, object udt, string attrName, object value)
        {
            if (value == null)
            {
                OracleUdt.SetValue(connection, udt, attrName, DBNull.Value);
            }
            else
            {
                OracleUdt.SetValue(connection, udt, attrName, value);
            }
        }

        /// <summary>
        /// Gets a UDT string value. If the string can be null, use <seealso cref="GetUdtNullableString(OracleConnection, object, string)"/> instead.
        /// </summary>
        /// <param name="connection">The Oracle connection.</param>
        /// <param name="udt">The UDT object from which to get the value.</param>
        /// <param name="attrName">The name of the attribute - this should be in all uppercase.</param>
        /// <returns>The string value.</returns>
        public static string GetUdtString(this OracleConnection connection, object udt, string attrName)
        {
            return (string)OracleUdt.GetValue(connection, udt, attrName);
        }

        /// <summary>
        /// Gets a UDT string value, which may be null.
        /// </summary>
        /// <param name="connection">The Oracle connection.</param>
        /// <param name="udt">The UDT object from which to get the value.</param>
        /// <param name="attrName">The name of the attribute - this should be in all uppercase.</param>
        /// <returns>The string value.</returns>
        public static string GetUdtNullableString(this OracleConnection connection, object udt, string attrName)
        {
            return OracleUdt.IsDBNull(connection, udt, attrName) ? null : connection.GetUdtString(udt, attrName);
        }

        /// <summary>
        /// Gets a UDT Int16 value. If the value can be null, use <seealso cref="GetUdtNullableInt16(OracleConnection, object, string)"/> instead.
        /// </summary>
        /// <param name="connection">The Oracle connection.</param>
        /// <param name="udt">The UDT object from which to get the value.</param>
        /// <param name="attrName">The name of the attribute - this should be in all uppercase.</param>
        /// <returns>The Int16  value.</returns>
        public static short GetUdtInt16(this OracleConnection connection, object udt, string attrName)
        {
            return (short)OracleUdt.GetValue(connection, udt, attrName);
        }

        /// <summary>
        /// Gets a UDT Int16 value, which may be null.
        /// </summary>
        /// <param name="connection">The Oracle connection.</param>
        /// <param name="udt">The UDT object from which to get the value.</param>
        /// <param name="attrName">The name of the attribute - this should be in all uppercase.</param>
        /// <returns>The Int16  value or null.</returns>
        public static short? GetUdtNullableInt16(this OracleConnection connection, object udt, string attrName)
        {
            return OracleUdt.IsDBNull(connection, udt, attrName) ? null : connection.GetUdtInt16(udt, attrName);
        }

        /// <summary>
        /// Gets a UDT Int32 value. If the value can be null, use <seealso cref="GetUdtNullableInt32(OracleConnection, object, string)"/> instead.
        /// </summary>
        /// <param name="connection">The Oracle connection.</param>
        /// <param name="udt">The UDT object from which to get the value.</param>
        /// <param name="attrName">The name of the attribute - this should be in all uppercase.</param>
        /// <returns>The Int32  value.</returns>
        public static int GetUdtInt32(this OracleConnection connection, object udt, string attrName)
        {
            return (int)OracleUdt.GetValue(connection, udt, attrName);
        }

        /// <summary>
        /// Gets a UDT Int32 value, which may be null.
        /// </summary>
        /// <param name="connection">The Oracle connection.</param>
        /// <param name="udt">The UDT object from which to get the value.</param>
        /// <param name="attrName">The name of the attribute - this should be in all uppercase.</param>
        /// <returns>The Int32  value or null.</returns>
        public static int? GetUdtNullableInt32(this OracleConnection connection, object udt, string attrName)
        {
            return OracleUdt.IsDBNull(connection, udt, attrName) ? null : connection.GetUdtInt32(udt, attrName);
        }

        /// <summary>
        /// Gets a UDT Int64 value. If the value can be null, use <seealso cref="GetUdtNullableInt64(OracleConnection, object, string)"/> instead.
        /// </summary>
        /// <param name="connection">The Oracle connection.</param>
        /// <param name="udt">The UDT object from which to get the value.</param>
        /// <param name="attrName">The name of the attribute - this should be in all uppercase.</param>
        /// <returns>The Int64  value.</returns>
        public static long GetUdtInt64(this OracleConnection connection, object udt, string attrName)
        {
            return (long)OracleUdt.GetValue(connection, udt, attrName);
        }

        /// <summary>
        /// Gets a UDT Int64 value, which may be null.
        /// </summary>
        /// <param name="connection">The Oracle connection.</param>
        /// <param name="udt">The UDT object from which to get the value.</param>
        /// <param name="attrName">The name of the attribute - this should be in all uppercase.</param>
        /// <returns>The Int64  value or null.</returns>
        public static long? GetUdtNullableInt64(this OracleConnection connection, object udt, string attrName)
        {
            return OracleUdt.IsDBNull(connection, udt, attrName) ? null : connection.GetUdtInt64(udt, attrName);
        }

        /// <summary>
        /// Gets a UDT decimal value. If the value can be null, use <seealso cref="GetUdtNullableDecimal(OracleConnection, object, string)"/> instead.
        /// </summary>
        /// <param name="connection">The Oracle connection.</param>
        /// <param name="udt">The UDT object from which to get the value.</param>
        /// <param name="attrName">The name of the attribute - this should be in all uppercase.</param>
        /// <returns>The decimal  value.</returns>
        public static decimal GetUdtDecimal(this OracleConnection connection, object udt, string attrName)
        {
            return (decimal)OracleUdt.GetValue(connection, udt, attrName);
        }

        /// <summary>
        /// Gets a UDT decimal value, which may be null.
        /// </summary>
        /// <param name="connection">The Oracle connection.</param>
        /// <param name="udt">The UDT object from which to get the value.</param>
        /// <param name="attrName">The name of the attribute - this should be in all uppercase.</param>
        /// <returns>The decimal  value or null.</returns>
        public static decimal? GetUdtNullableDecimal(this OracleConnection connection, object udt, string attrName)
        {
            return OracleUdt.IsDBNull(connection, udt, attrName) ? null : connection.GetUdtDecimal(udt, attrName);
        }

        /// <summary>
        /// Gets a UDT DateTime value. If the value can be null, use <seealso cref="GetUdtNullableDateTime(OracleConnection, object, string)"/> instead.
        /// </summary>
        /// <param name="connection">The Oracle connection.</param>
        /// <param name="udt">The UDT object from which to get the value.</param>
        /// <param name="attrName">The name of the attribute - this should be in all uppercase.</param>
        /// <returns>The DateTime  value.</returns>
        public static DateTime GetUdtDateTime(this OracleConnection connection, object udt, string attrName)
        {
            return (DateTime)OracleUdt.GetValue(connection, udt, attrName);
        }

        /// <summary>
        /// Gets a UDT DateTime value, which may be null.
        /// </summary>
        /// <param name="connection">The Oracle connection.</param>
        /// <param name="udt">The UDT object from which to get the value.</param>
        /// <param name="attrName">The name of the attribute - this should be in all uppercase.</param>
        /// <returns>The DateTime  value or null.</returns>
        public static DateTime? GetUdtNullableDateTime(this OracleConnection connection, object udt, string attrName)
        {
            return OracleUdt.IsDBNull(connection, udt, attrName) ? null : connection.GetUdtDateTime(udt, attrName);
        }
    }
}
