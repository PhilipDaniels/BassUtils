using System;
using System.Data;
using Dawn;

namespace BassUtils.Data
{
    /*
	 * We are aiming for a family of methods:
	 * 
	 * public static Boolean GetBoolean(int i);                                 // built-in to IDataRecord
	 * public static Boolean GetBoolean(string name);                           // extension
	 * public static Boolean GetBoolean(int i, bool defaultIfNull);             // extension
	 * public static Boolean GetBoolean(string name, bool defaultIfNull);       // extension
	 * public static Boolean? GetNullableBoolean(int i);                        // extension
	 * public static Boolean? GetNullableBoolean(string name);                  // extension
	 */


    /// <summary>
    /// Extensions to the <code>System.Data.IDataRecord</code> class.
    /// </summary>
    public static class IDataRecordExtensions
    {
        /// <summary>
        /// Create a new object of type T using the specified delegate.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="record">The data record.</param>
        /// <param name="hydrationFunction">A delegate that can construct a new object of type T
        /// and initialise it from a data record.</param>
        /// <returns>A new object of type T.</returns>
        public static T Hydrate<T>(this IDataRecord record, Func<IDataRecord, T> hydrationFunction)
        {
            Guard.Argument(record, nameof(record)).NotNull();
            Guard.Argument(hydrationFunction, nameof(hydrationFunction)).NotNull();

            return hydrationFunction(record);
        }

        /// <summary>
        /// Return whether the specified field is set to null.S
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>True if the field is DbNull, false otherwise.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static bool IsDbNull(this IDataRecord record, string name)
        {
            return record.IsDBNull(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Boolean"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static bool GetBoolean(this IDataRecord record, string name)
        {
            return record.GetBoolean(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Boolean"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static bool GetBoolean(this IDataRecord record, int i, bool defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetBoolean(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Boolean"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static bool GetBoolean(this IDataRecord record, string name, bool defaultIfNull)
        {
            return record.GetBoolean(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Boolean&gt;"/> of <see cref="Boolean"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static bool? GetNullableBoolean(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? (bool?)null : record.GetBoolean(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Boolean&gt;"/> of <see cref="Boolean"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static bool? GetNullableBoolean(this IDataRecord record, string name)
        {
            return record.GetNullableBoolean(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Byte"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static byte GetByte(this IDataRecord record, string name)
        {
            return record.GetByte(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Byte"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static byte GetByte(this IDataRecord record, int i, byte defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetByte(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Byte"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static byte GetByte(this IDataRecord record, string name, byte defaultIfNull)
        {
            return record.GetByte(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Byte&gt;"/> of <see cref="Byte"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static byte? GetNullableByte(this IDataRecord record, string name)
        {
            return record.GetNullableByte(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Byte&gt;"/> of <see cref="Byte"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static byte? GetNullableByte(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? (byte?)null : record.GetByte(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Char"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static char GetChar(this IDataRecord record, string name)
        {
            return record.GetChar(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Char"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static char GetChar(this IDataRecord record, int i, char defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetChar(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Char"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static char GetChar(this IDataRecord record, string name, char defaultIfNull)
        {
            return record.GetChar(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Char&gt;"/> of <see cref="Char"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static char? GetNullableChar(this IDataRecord record, string name)
        {
            return record.GetNullableChar(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Char&gt;"/> of <see cref="Char"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static char? GetNullableChar(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? (char?)null : record.GetChar(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static DateTime GetDateTime(this IDataRecord record, string name)
        {
            return record.GetDateTime(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static DateTime GetDateTime(this IDataRecord record, int i, DateTime defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetDateTime(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static DateTime GetDateTime(this IDataRecord record, string name, DateTime defaultIfNull)
        {
            return record.GetDateTime(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;DateTime&gt;"/> of <see cref="DateTime"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static DateTime? GetNullableDateTime(this IDataRecord record, string name)
        {
            return record.GetNullableDateTime(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;DateTime&gt;"/> of <see cref="DateTime"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static DateTime? GetNullableDateTime(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? (DateTime?)null : record.GetDateTime(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Decimal"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static decimal GetDecimal(this IDataRecord record, string name)
        {
            return record.GetDecimal(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Decimal"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static decimal GetDecimal(this IDataRecord record, int i, decimal defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetDecimal(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Decimal"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static decimal GetDecimal(this IDataRecord record, string name, decimal defaultIfNull)
        {
            return record.GetDecimal(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Decimal&gt;"/> of <see cref="Decimal"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static decimal? GetNullableDecimal(this IDataRecord record, string name)
        {
            return record.GetNullableDecimal(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Decimal&gt;"/> of <see cref="Decimal"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static decimal? GetNullableDecimal(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? (decimal?)null : record.GetDecimal(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Double"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static double GetDouble(this IDataRecord record, string name)
        {
            return record.GetDouble(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Double"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static double GetDouble(this IDataRecord record, int i, double defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetDouble(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Double"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static double GetDouble(this IDataRecord record, string name, double defaultIfNull)
        {
            return record.GetDouble(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Double&gt;"/> of <see cref="Double"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static double? GetNullableDouble(this IDataRecord record, string name)
        {
            return record.GetNullableDouble(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Double&gt;"/> of <see cref="Double"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static double? GetNullableDouble(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? (double?)null : record.GetDouble(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Single"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static float GetFloat(this IDataRecord record, string name)
        {
            return record.GetFloat(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Single"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static float GetFloat(this IDataRecord record, int i, float defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetFloat(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Single"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static float GetFloat(this IDataRecord record, string name, float defaultIfNull)
        {
            return record.GetFloat(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Single&gt;"/> of <see cref="Single"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static float? GetNullableFloat(this IDataRecord record, string name)
        {
            return record.GetNullableFloat(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Single&gt;"/> of <see cref="Single"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static float? GetNullableFloat(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? (float?)null : record.GetFloat(i);
        }

        /// <summary>
        /// Gets the value of the specified column as an <see cref="Int16"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static short GetInt16(this IDataRecord record, string name)
        {
            return record.GetInt16(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int16"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static short GetInt16(this IDataRecord record, int i, short defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetInt16(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int16"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static short GetInt16(this IDataRecord record, string name, short defaultIfNull)
        {
            return record.GetInt16(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Int16&gt;"/> of <see cref="Int16"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static short? GetNullableInt16(this IDataRecord record, string name)
        {
            return record.GetNullableInt16(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Int16&gt;"/> of <see cref="Int16"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static short? GetNullableInt16(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? (short?)null : record.GetInt16(i);
        }

        /// <summary>
        /// Gets the value of the specified column as an <see cref="Int32"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static int GetInt32(this IDataRecord record, string name)
        {
            return record.GetInt32(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int32"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static int GetInt32(this IDataRecord record, int i, int defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetInt32(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int32"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static int GetInt32(this IDataRecord record, string name, int defaultIfNull)
        {
            return record.GetInt32(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Int32&gt;"/> of <see cref="Int32"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static int? GetNullableInt32(this IDataRecord record, string name)
        {
            return record.GetNullableInt32(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Int32&gt;"/> of <see cref="Int32"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static int? GetNullableInt32(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? (int?)null : record.GetInt32(i);
        }

        /// <summary>
        /// Gets the value of the specified column as an <see cref="Int64"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static long GetInt64(this IDataRecord record, string name)
        {
            return record.GetInt64(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int64"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static long GetInt64(this IDataRecord record, int i, long defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetInt64(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int64"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static long GetInt64(this IDataRecord record, string name, long defaultIfNull)
        {
            return record.GetInt64(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Int64&gt;"/> of <see cref="Int64"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static long? GetNullableInt64(this IDataRecord record, string name)
        {
            return record.GetNullableInt64(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Int64&gt;"/> of <see cref="Int64"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static long? GetNullableInt64(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? (long?)null : record.GetInt64(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="String"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static string GetString(this IDataRecord record, string name)
        {
            return record.GetString(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int64"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static string GetString(this IDataRecord record, int i, string defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetString(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="String"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static string GetString(this IDataRecord record, string name, string defaultIfNull)
        {
            return record.GetString(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="String"/>, or null (Nothing in Visual Basic).
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static string GetNullableString(this IDataRecord record, string name)
        {
            return record.GetNullableString(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="String"/>, or null (Nothing in Visual Basic).
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static string GetNullableString(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? null : record.GetString(i);
        }

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static object GetValue(this IDataRecord record, string name)
        {
            return record.GetValue(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static object GetValue(this IDataRecord record, int i, object defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetValue(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int64"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static object GetValue(this IDataRecord record, string name, object defaultIfNull)
        {
            return record.GetValue(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as an <see cref="Object"/>, or null (Nothing in Visual Basic).
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static object GetNullableValue(this IDataRecord record, string name)
        {
            return record.GetNullableValue(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as an <see cref="Object"/>, or null (Nothing in Visual Basic).
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static object GetNullableValue(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? null : record.GetValue(i);
        }

        /// <summary>
        /// Gets the value of the specified column as an <see cref="Guid"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Guid GetGuid(this IDataRecord record, string name)
        {
            return record.GetGuid(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Guid"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Guid GetGuid(this IDataRecord record, int i, Guid defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetGuid(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Guid"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Guid GetGuid(this IDataRecord record, string name, Guid defaultIfNull)
        {
            return record.GetGuid(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Guid&gt;"/> of <see cref="Guid"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Guid? GetNullableGuid(this IDataRecord record, string name)
        {
            return record.GetNullableGuid(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Guid&gt;"/> of <see cref="Guid"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Guid? GetNullableGuid(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? (Guid?)null : record.GetGuid(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Boolean"/>.
        /// Accepts Y, N, T, F, Yes, No, True, False, 0, 1.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .Net Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static bool GetBooleanExtended(this IDataRecord record, int i)
        {
            return Conv.ToBoolean(record.GetValue(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Boolean"/>.
        /// Accepts Y, N, T, F, Yes, No, True, False, 0, 1.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static bool GetBooleanExtended(this IDataRecord record, string name)
        {
            return record.GetBooleanExtended(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Boolean"/>.
        /// Accepts Y, N, T, F, Yes, No, True, False, 0, 1.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static bool GetBooleanExtended(this IDataRecord record, int i, bool defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetBooleanExtended(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Boolean"/>.
        /// Accepts Y, N, T, F, Yes, No, True, False, 0, 1.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static bool GetBooleanExtended(this IDataRecord record, string name, bool defaultIfNull)
        {
            return record.GetBooleanExtended(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Boolean&gt;"/> of <see cref="Boolean"/>.
        /// Accepts Y, N, T, F, Yes, No, True, False, 0, 1.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static bool? GetNullableBooleanExtended(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? (bool?)null : record.GetBooleanExtended(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Boolean&gt;"/> of <see cref="Boolean"/>.
        /// Accepts Y, N, T, F, Yes, No, True, False, 0, 1.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static bool? GetNullableBooleanExtended(this IDataRecord record, string name)
        {
            return record.GetNullableBooleanExtended(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column an an enum.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column converted to the corresponding enum value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static T GetEnum<T>(this IDataRecord record, int i)
            where T : struct
        {
            return Conv.ToEnum<T>(record.GetValue(i));
        }

        /// <summary>
        /// Gets the value of the specified column an an enum.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column converted to the corresponding enum value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static T GetEnum<T>(this IDataRecord record, string name)
             where T : struct
        {
            return record.GetEnum<T>(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column an an enum, returning a default value if the column is null.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">Default value to return if the column is null.</param>
        /// <returns>The value of the column converted to the corresponding enum value, or the default value if the column is null.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static T GetEnum<T>(this IDataRecord record, int i, T defaultIfNull)
             where T : struct
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetEnum<T>(i);
        }

        /// <summary>
        /// Gets the value of the specified column an an enum, returning a default value if the column is null.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultIfNull">Default value to return if the column is null.</param>
        /// <returns>The value of the column converted to the corresponding enum value, or the default value if the column is null.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static T GetEnum<T>(this IDataRecord record, string name, T defaultIfNull)
             where T : struct
        {
            return record.GetEnum<T>(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column an an enum, returning null if the column is DbNull.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column converted to the corresponding enum value, or null.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static T? GetNullableEnum<T>(this IDataRecord record, int i)
             where T : struct
        {
            return record.IsDBNull(i) ? (T?)null : record.GetEnum<T>(i);
        }

        /// <summary>
        /// Gets the value of the specified column an an enum, returning null if the column is DbNull.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column converted to the corresponding enum value, or null.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static T? GetNullableEnum<T>(this IDataRecord record, string name)
             where T : struct
        {
            return record.GetNullableEnum<T>(record.GetOrdinal(name));
        }
    }
}
