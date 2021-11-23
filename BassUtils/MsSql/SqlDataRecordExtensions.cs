using System;
using Microsoft.Data.SqlClient.Server;

namespace BassUtils.MsSql
{
    /// <summary>
    /// Extensions for the <seealso cref="SqlDataRecord"/> class.
    /// </summary>
    public static class SqlDataRecordExtensions
    {
        /// <summary>
        /// Gets a DateTime value by column name.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>DateTime object.</returns>
        public static DateTime GetDateTime(this SqlDataRecord record, string name)
        {
            return record.GetDateTime(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the DateTime value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to get.</param>
        /// <returns>null or the DateTime object.</returns>
        public static DateTime? GetNullableDateTime(this SqlDataRecord record, int ordinal)
        {
            if (record.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return record.GetDateTime(ordinal);
            }
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the DateTime value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>null or the DateTime object.</returns>
        public static DateTime? GetNullableDateTime(this SqlDataRecord record, string name)
        {
            return record.GetNullableDateTime(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a DateTime.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableDateTime(this SqlDataRecord record, int ordinal, DateTime? value)
        {
            if (value == null)
            {
                record.SetDBNull(ordinal);
            }
            else
            {
                record.SetDateTime(ordinal, value.Value);
            }
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a DateTime.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableDateTime(this SqlDataRecord record, string name, DateTime? value)
        {
            record.SetNullableDateTime(record.GetOrdinal(name), value);
        }

        /// <summary>
        /// Gets a DateTimeOffset value by column name.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>DateTimeOffset object.</returns>
        public static DateTimeOffset GetDateTimeOffset(this SqlDataRecord record, string name)
        {
            return record.GetDateTimeOffset(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the DateTimeOffset value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to get.</param>
        /// <returns>null or the DateTimeOffset object.</returns>
        public static DateTimeOffset? GetNullableDateTimeOffset(this SqlDataRecord record, int ordinal)
        {
            if (record.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return record.GetDateTimeOffset(ordinal);
            }
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the DateTimeOffset value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>null or the DateTimeOffset object.</returns>
        public static DateTimeOffset? GetNullableDateTimeOffset(this SqlDataRecord record, string name)
        {
            return record.GetNullableDateTimeOffset(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a DateTimeOffset.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableDateTimeOffset(this SqlDataRecord record, int ordinal, DateTimeOffset? value)
        {
            if (value == null)
            {
                record.SetDBNull(ordinal);
            }
            else
            {
                record.SetDateTimeOffset(ordinal, value.Value);
            }
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a DateTimeOffset.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableDateTimeOffset(this SqlDataRecord record, string name, DateTimeOffset? value)
        {
            record.SetNullableDateTimeOffset(record.GetOrdinal(name), value);
        }

        /// <summary>
        /// Gets a TimeSpan value by column name.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>TimeSpan object.</returns>
        public static TimeSpan GetTimeSpan(this SqlDataRecord record, string name)
        {
            return record.GetTimeSpan(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the TimeSpan value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to get.</param>
        /// <returns>null or the TimeSpan object.</returns>
        public static TimeSpan? GetNullableTimeSpan(this SqlDataRecord record, int ordinal)
        {
            if (record.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return record.GetTimeSpan(ordinal);
            }
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the TimeSpan value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>null or the TimeSpan object.</returns>
        public static TimeSpan? GetNullableTimeSpan(this SqlDataRecord record, string name)
        {
            return record.GetNullableTimeSpan(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a TimeSpan.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableTimeSpan(this SqlDataRecord record, int ordinal, TimeSpan? value)
        {
            if (value == null)
            {
                record.SetDBNull(ordinal);
            }
            else
            {
                record.SetTimeSpan(ordinal, value.Value);
            }
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a TimeSpan.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableTimeSpan(this SqlDataRecord record, string name, TimeSpan? value)
        {
            record.SetNullableTimeSpan(record.GetOrdinal(name), value);
        }

        /// <summary>
        /// Gets a String value by column name.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>String object.</returns>
        public static string GetString(this SqlDataRecord record, string name)
        {
            return record.GetString(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the String value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to get.</param>
        /// <returns>null or the String object.</returns>
        public static string GetNullableString(this SqlDataRecord record, int ordinal)
        {
            if (record.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return record.GetString(ordinal);
            }
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the String value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>null or the String object.</returns>
        public static string GetNullableString(this SqlDataRecord record, string name)
        {
            return record.GetNullableString(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a string.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableString(this SqlDataRecord record, int ordinal, string value)
        {
            if (value == null)
            {
                record.SetDBNull(ordinal);
            }
            else
            {
                record.SetString(ordinal, value);
            }
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a String.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableString(this SqlDataRecord record, string name, string value)
        {
            record.SetNullableString(record.GetOrdinal(name), value);
        }

        /// <summary>
        /// Gets a byte value by column name.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>Byte object.</returns>
        public static byte GetByte(this SqlDataRecord record, string name)
        {
            return record.GetByte(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the byte value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to get.</param>
        /// <returns>null or the byte object.</returns>
        public static byte? GetNullableByte(this SqlDataRecord record, int ordinal)
        {
            if (record.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return record.GetByte(ordinal);
            }
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the byte value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>null or the byte object.</returns>
        public static byte? GetNullableByte(this SqlDataRecord record, string name)
        {
            return record.GetNullableByte(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a byte.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableByte(this SqlDataRecord record, int ordinal, byte? value)
        {
            if (value == null)
            {
                record.SetDBNull(ordinal);
            }
            else
            {
                record.SetByte(ordinal, value.Value);
            }
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a byte.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableByte(this SqlDataRecord record, string name, byte? value)
        {
            record.SetNullableByte(record.GetOrdinal(name), value);
        }

        /// <summary>
        /// Gets a short value by column name.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>Short object.</returns>
        public static short GetInt16(this SqlDataRecord record, string name)
        {
            return record.GetInt16(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the short value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to get.</param>
        /// <returns>null or the short object.</returns>
        public static short? GetNullableInt16(this SqlDataRecord record, int ordinal)
        {
            if (record.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return record.GetInt16(ordinal);
            }
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the short value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>null or the short object.</returns>
        public static short? GetNullableInt16(this SqlDataRecord record, string name)
        {
            return record.GetNullableInt16(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a short.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableInt16(this SqlDataRecord record, int ordinal, short? value)
        {
            if (value == null)
            {
                record.SetDBNull(ordinal);
            }
            else
            {
                record.SetInt16(ordinal, value.Value);
            }
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a short.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableInt16(this SqlDataRecord record, string name, short? value)
        {
            record.SetNullableInt16(record.GetOrdinal(name), value);
        }

        /// <summary>
        /// Gets an int value by column name.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>Int object.</returns>
        public static int GetInt32(this SqlDataRecord record, string name)
        {
            return record.GetInt32(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the int value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to get.</param>
        /// <returns>null or the int object.</returns>
        public static int? GetNullableInt32(this SqlDataRecord record, int ordinal)
        {
            if (record.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return record.GetInt32(ordinal);
            }
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the int value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>null or the int object.</returns>
        public static int? GetNullableInt32(this SqlDataRecord record, string name)
        {
            return record.GetNullableInt32(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as an int.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableInt32(this SqlDataRecord record, int ordinal, int? value)
        {
            if (value == null)
            {
                record.SetDBNull(ordinal);
            }
            else
            {
                record.SetInt32(ordinal, value.Value);
            }
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as an int.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableInt32(this SqlDataRecord record, string name, int? value)
        {
            record.SetNullableInt32(record.GetOrdinal(name), value);
        }

        /// <summary>
        /// Gets a long value by column name.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>long object.</returns>
        public static long GetInt64(this SqlDataRecord record, string name)
        {
            return record.GetInt64(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the long value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to get.</param>
        /// <returns>null or the long object.</returns>
        public static long? GetNullableInt64(this SqlDataRecord record, int ordinal)
        {
            if (record.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return record.GetInt64(ordinal);
            }
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the long value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>null or the long object.</returns>
        public static long? GetNullableInt64(this SqlDataRecord record, string name)
        {
            return record.GetNullableInt64(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a long.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableInt64(this SqlDataRecord record, int ordinal, long? value)
        {
            if (value == null)
            {
                record.SetDBNull(ordinal);
            }
            else
            {
                record.SetInt64(ordinal, value.Value);
            }
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a long.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableInt64(this SqlDataRecord record, string name, long? value)
        {
            record.SetNullableInt64(record.GetOrdinal(name), value);
        }

        /// <summary>
        /// Gets a decimal value by column name.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>decimal object.</returns>
        public static decimal GetDecimal(this SqlDataRecord record, string name)
        {
            return record.GetDecimal(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the decimal value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to get.</param>
        /// <returns>null or the decimal object.</returns>
        public static decimal? GetNullableDecimal(this SqlDataRecord record, int ordinal)
        {
            if (record.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return record.GetDecimal(ordinal);
            }
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the decimal value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>null or the decimal object.</returns>
        public static decimal? GetNullableDecimal(this SqlDataRecord record, string name)
        {
            return record.GetNullableDecimal(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a decimal.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableDecimal(this SqlDataRecord record, int ordinal, decimal? value)
        {
            if (value == null)
            {
                record.SetDBNull(ordinal);
            }
            else
            {
                record.SetDecimal(ordinal, value.Value);
            }
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a decimal.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableDecimal(this SqlDataRecord record, string name, decimal? value)
        {
            record.SetNullableDecimal(record.GetOrdinal(name), value);
        }

        /// <summary>
        /// Gets a boolean value by column name.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>decimal object.</returns>
        public static bool GetBoolean(this SqlDataRecord record, string name)
        {
            return record.GetBoolean(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the boolean value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to get.</param>
        /// <returns>null or the boolean object.</returns>
        public static bool? GetNullableBoolean(this SqlDataRecord record, int ordinal)
        {
            if (record.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return record.GetBoolean(ordinal);
            }
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the boolean value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>null or the boolean object.</returns>
        public static bool? GetNullableBoolean(this SqlDataRecord record, string name)
        {
            return record.GetNullableBoolean(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a bool.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableBoolean(this SqlDataRecord record, int ordinal, bool? value)
        {
            if (value == null)
            {
                record.SetDBNull(ordinal);
            }
            else
            {
                record.SetBoolean(ordinal, value.Value);
            }
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a bool.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableBoolean(this SqlDataRecord record, string name, bool? value)
        {
            record.SetNullableBoolean(record.GetOrdinal(name), value);
        }

        /// <summary>
        /// Gets a float value by column name.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>float object.</returns>
        public static float GetFloat(this SqlDataRecord record, string name)
        {
            return record.GetFloat(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the float value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to get.</param>
        /// <returns>null or the float object.</returns>
        public static float? GetNullableFloat(this SqlDataRecord record, int ordinal)
        {
            if (record.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return record.GetFloat(ordinal);
            }
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the float value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>null or the float object.</returns>
        public static float? GetNullableFloat(this SqlDataRecord record, string name)
        {
            return record.GetNullableFloat(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a float.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableFloat(this SqlDataRecord record, int ordinal, float? value)
        {
            if (value == null)
            {
                record.SetDBNull(ordinal);
            }
            else
            {
                record.SetFloat(ordinal, value.Value);
            }
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a float.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableFloat(this SqlDataRecord record, string name, float? value)
        {
            record.SetNullableFloat(record.GetOrdinal(name), value);
        }

        /// <summary>
        /// Gets a double value by column name.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>double object.</returns>
        public static double GetDouble(this SqlDataRecord record, string name)
        {
            return record.GetDouble(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the double value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to get.</param>
        /// <returns>null or the double object.</returns>
        public static double? GetNullableDouble(this SqlDataRecord record, int ordinal)
        {
            if (record.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return record.GetDouble(ordinal);
            }
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the double value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>null or the double object.</returns>
        public static double? GetNullableDouble(this SqlDataRecord record, string name)
        {
            return record.GetNullableDouble(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a double.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableDouble(this SqlDataRecord record, int ordinal, double? value)
        {
            if (value == null)
            {
                record.SetDBNull(ordinal);
            }
            else
            {
                record.SetDouble(ordinal, value.Value);
            }
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a double.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableDouble(this SqlDataRecord record, string name, double? value)
        {
            record.SetNullableDouble(record.GetOrdinal(name), value);
        }

        /// <summary>
        /// Gets a Guid value by column name.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>Guid object.</returns>
        public static Guid GetGuid(this SqlDataRecord record, string name)
        {
            return record.GetGuid(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the Guid value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to get.</param>
        /// <returns>null or the Guid object.</returns>
        public static Guid? GetNullableGuid(this SqlDataRecord record, int ordinal)
        {
            if (record.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return record.GetGuid(ordinal);
            }
        }

        /// <summary>
        /// If the column value is DBNull returns null, else returns the Guid value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to get.</param>
        /// <returns>null or the Guid object.</returns>
        public static Guid? GetNullableGuid(this SqlDataRecord record, string name)
        {
            return record.GetNullableGuid(record.GetOrdinal(name));
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a Guid.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="ordinal">Ordinal of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableGuid(this SqlDataRecord record, int ordinal, Guid? value)
        {
            if (value == null)
            {
                record.SetDBNull(ordinal);
            }
            else
            {
                record.SetGuid(ordinal, value.Value);
            }
        }

        /// <summary>
        /// If the value is null, sets <c>DBNull</c>, else sets the value as a Guid.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="name">Name of the field to set.</param>
        /// <param name="value">The value to set. Can be null.</param>
        public static void SetNullableGuid(this SqlDataRecord record, string name, Guid? value)
        {
            record.SetNullableGuid(record.GetOrdinal(name), value);
        }
    }
}
