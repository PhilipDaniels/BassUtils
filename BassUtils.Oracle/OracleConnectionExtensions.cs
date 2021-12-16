using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace BassUtils.Oracle
{
    public static class OracleConnectionExtensions
    {
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

        public static string GetUdtString(this OracleConnection connection, object udt, string attrName)
        {
            return (string)OracleUdt.GetValue(connection, udt, attrName);
        }

        public static string GetUdtNullableString(this OracleConnection connection, object udt, string attrName)
        {
            return OracleUdt.IsDBNull(connection, udt, attrName) ? null : connection.GetUdtString(udt, attrName);
        }

        public static short GetUdtInt16(this OracleConnection connection, object udt, string attrName)
        {
            return (short)OracleUdt.GetValue(connection, udt, attrName);
        }

        public static short? GetUdtNullableInt16(this OracleConnection connection, object udt, string attrName)
        {
            return OracleUdt.IsDBNull(connection, udt, attrName) ? null : connection.GetUdtInt16(udt, attrName);
        }

        public static int GetUdtInt32(this OracleConnection connection, object udt, string attrName)
        {
            return (int)OracleUdt.GetValue(connection, udt, attrName);
        }

        public static int? GetUdtNullableInt32(this OracleConnection connection, object udt, string attrName)
        {
            return OracleUdt.IsDBNull(connection, udt, attrName) ? null : connection.GetUdtInt32(udt, attrName);
        }

        public static long GetUdtInt64(this OracleConnection connection, object udt, string attrName)
        {
            return (long)OracleUdt.GetValue(connection, udt, attrName);
        }

        public static long? GetUdtNullableInt64(this OracleConnection connection, object udt, string attrName)
        {
            return OracleUdt.IsDBNull(connection, udt, attrName) ? null : connection.GetUdtInt64(udt, attrName);
        }

        public static decimal GetUdtDecimal(this OracleConnection connection, object udt, string attrName)
        {
            return (decimal)OracleUdt.GetValue(connection, udt, attrName);
        }

        public static decimal? GetUdtNullableDecimal(this OracleConnection connection, object udt, string attrName)
        {
            return OracleUdt.IsDBNull(connection, udt, attrName) ? null : connection.GetUdtDecimal(udt, attrName);
        }

        public static DateTime GetUdtDateTime(this OracleConnection connection, object udt, string attrName)
        {
            return (DateTime)OracleUdt.GetValue(connection, udt, attrName);
        }

        public static DateTime? GetUdtNullableDateTime(this OracleConnection connection, object udt, string attrName)
        {
            return OracleUdt.IsDBNull(connection, udt, attrName) ? null : connection.GetUdtDateTime(udt, attrName);
        }
    }
}
