using Dawn;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ClassLibrary1
{
    /// <summary>
    /// A class for doing conversions, in the style of System.Convert.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly",
        MessageId = "Conv", Justification = "My version of System.Convert.")]
    public static class Conv
    {
        /// <summary>
        /// The set of strings that are considered to equate to <c>true</c> by the
        /// <see cref="ToBoolean"/> method.
        /// The strings are used in case-insensitive comparisons.
        /// </summary>
        static readonly List<string> trueStrings = new List<string> { "TRUE", "T", "YES", "Y", "1" };

        /// <summary>
        /// The set of strings that are considered to equate to <c>false</c> by the
        /// <see cref="ToBoolean"/> method.
        /// The strings are used in case-insensitive comparisons.
        /// </summary>
        static readonly List<string> falseStrings = new List<string> { "FALSE", "F", "NO", "N", "0" };

        /// <summary>
        /// Convert a <paramref name="value"/> to the corresponding enumeration value. First checks to see if the
        /// <paramref name="value"/> is an integer, it is is then tries to cast the number to the enum value. If the
        /// value is not a number and is a <c>Char</c> or a <c>String</c> of length 1, then convert that
        /// character to an integer and try and cast it to the enum value. Otherwise, if the <paramref name="value"/>
        /// is a string of more than length 1, try and match the enum by name using <c>Enum.Parse</c>.
        /// </summary>
        /// <param name="enumType">The target enum type.</param>
        /// <param name="value">The value to convert.</param>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="InvalidOperationException">T is not an enum type.</exception>
        /// <exception cref="InvalidCastException">The value cannot be converted.</exception>
        /// <returns>An enum value of the specified type.</returns>
        public static object ToEnum(Type enumType, object value)
        {
            Guard.Argument(enumType, nameof(enumType)).NotNull();
            Guard.Argument(value, nameof(value)).NotNull();

            if (!enumType.IsEnum)
                throw new InvalidOperationException("Result type (" + enumType.FullName + ") is not an enum.");

            Type underlyingType = Enum.GetUnderlyingType(enumType);

            object convertedValue = null;
            if (IsIntegerType(value))
            {
                convertedValue = Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
            }
            else if (value.GetType() == typeof(Char))
            {
                convertedValue = Convert.ChangeType((char)value, underlyingType, CultureInfo.InvariantCulture);
            }
            else if (value.GetType() == typeof(String))
            {
                string s = value.ToString();
                if (s.Length < 1)
                {
                    throw new InvalidCastException("Cannot convert empty string to enum " + enumType.FullName);
                }
                else if (s.Length == 1)
                {
                    convertedValue = Convert.ChangeType(s[0], underlyingType, CultureInfo.InvariantCulture);
                }
                else
                {
                    convertedValue = Enum.Parse(enumType, s);
                }
            }
            else
            {
                throw new InvalidCastException("Cannot convert " + value.ToString() + " to enumeration.");
            }

            if (!Enum.IsDefined(enumType, convertedValue))
            {
                string msg = String.Concat("The value '", convertedValue, "' does not map to a member of enumeration ", enumType.FullName);
                throw new InvalidCastException(msg);
            }

            return Enum.ToObject(enumType, convertedValue);
        }

        /// <summary>
        /// Convert a <paramref name="value"/> to the corresponding enumeration value. First checks to see if the
        /// <paramref name="value"/> is a number, it is is then tries to cast the number to the enum value. If the
        /// value is not a number and is a <c>Char</c> or a <c>String</c> of length 1, then convert that
        /// character to an integer and try and cast it to the enum value. Otherwise, if the <paramref name="value"/>
        /// is a string of more than length 1, try and match the enum by name using <c>Enum.Parse</c>.
        /// </summary>
        /// <typeparam name="T">The target enum type.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="InvalidOperationException">T is not an enum type.</exception>
        /// <exception cref="InvalidCastException">The value cannot be converted.</exception>
        /// <returns>An enum value of the specified type.</returns>
        public static T ToEnum<T>(object value)
            where T : struct
        {
            return (T)ToEnum(typeof(T), value);
        }

        /// <summary>
        /// Convert a <paramref name="value"/> to a <c>Boolean</c>. If the <paramref name="value"/> is a
        /// number, then return false if it is 0, else true. If the <paramref name="value"/>
        /// is not a number then it is converted to a string, and matched against an internal list
        /// of strings considered true and false.
        /// </summary>
        /// <remarks>
        /// The string collections are immutable due to reasons of thread-safety surrounding
        /// static collections.
        /// </remarks>
        /// <param name="value">The value to convert.</param>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="InvalidCastException">The value could not be converted.</exception>
        /// <returns>true or false, as appropriate.</returns>
        public static bool ToBoolean(object value)
        {
            Guard.Argument(value, nameof(value)).NotNull();

            if (value.GetType() == typeof(Boolean))
                return (Boolean)value;

            if (Conv.IsNumeric(value))
            {
                double vdub = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                return vdub != 0;
            }

            if (value.GetType() == typeof(Char) || value.GetType() == typeof(String))
            {
                string vstr = value.ToString().Trim();

                if (trueStrings != null && trueStrings.Contains(vstr, StringComparer.OrdinalIgnoreCase))
                    return true;
                if (falseStrings != null && falseStrings.Contains(vstr, StringComparer.OrdinalIgnoreCase))
                    return false;

                throw new InvalidCastException("Cannot convert '" + vstr + "' to Boolean.");
            }

            throw new InvalidCastException("Cannot convert '" + value.ToString() + "' to Boolean.");
        }

        /// <summary>
        /// Checks to see if the <paramref name="typeCode"/> is an integer type.
        /// Char is not considered to be an integer type.
        /// </summary>
        /// <param name="typeCode">The TypeCode to check.</param>
        /// <returns>True if the typeCode is considered to be an integer type.</returns>
        public static bool IsIntegerTypeCode(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks to see if the <paramref name="typeCode"/> is a floating point type.
        /// </summary>
        /// <param name="typeCode">The TypeCode to check.</param>
        /// <returns>True if the typeCode is considered to be an integer type.</returns>
        public static bool IsFloatingPointTypeCode(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks to see if the TypeCode corresponds to a numeric type (integer or floating point).
        /// Char is not considered to be numeric.
        /// </summary>
        /// <param name="typeCode">The TypeCode to check.</param>
        /// <returns>True if the typeCode is considered to be a numeric type.</returns>
        public static bool IsNumericTypeCode(TypeCode typeCode)
        {
            return IsIntegerTypeCode(typeCode) || IsFloatingPointTypeCode(typeCode);
        }

        /// <summary>
        /// Checks to see if the <paramref name="type"/> is integer.
        /// </summary>
        /// <param name="type">The Type to check.</param>
        /// <returns>True if the type is considered to be an integer type.</returns>
        public static bool IsIntegerType(Type type)
        {
            return IsIntegerTypeCode(Type.GetTypeCode(type));
        }

        /// <summary>
        /// Checks to see if the value is of an integer type.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <exception cref="ArgumentNullException">The value is null.</exception>
        /// <returns>True if the value is considered to be an integer type.</returns>
        public static bool IsIntegerType(object value)
        {
            Guard.Argument(value, nameof(value)).NotNull();

            return IsIntegerTypeCode(Type.GetTypeCode(value.GetType()));
        }

        /// <summary>
        /// Checks to see if the <paramref name="type"/> is floating point.
        /// </summary>
        /// <param name="type">The Type to check.</param>
        /// <returns>True if the type is considered to be a floating point type.</returns>
        public static bool IsFloatingPointType(Type type)
        {
            return IsFloatingPointTypeCode(Type.GetTypeCode(type));
        }

        /// <summary>
        /// Checks to see if the value is of a floating point type.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <exception cref="ArgumentNullException">The value is null.</exception>
        /// <returns>True if the value is considered to be a floating point type.</returns>
        public static bool IsFloatingPointType(object value)
        {
            Guard.Argument(value, nameof(value)).NotNull();

            return IsFloatingPointTypeCode(Type.GetTypeCode(value.GetType()));
        }

        /// <summary>
        /// Checks to see if the Type is numeric (integer or floating point).
        /// </summary>
        /// <param name="type">The Type to check.</param>
        /// <returns>True if the type is considered to be a numeric type.</returns>
        public static bool IsNumericType(Type type)
        {
            return IsNumericTypeCode(Type.GetTypeCode(type));
        }

        /// <summary>
        /// Checks to see if the value is of a numeric type (integer or floating point).
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <exception cref="ArgumentNullException">The value is null.</exception>
        /// <returns>True if the value is considered to be a numeric type.</returns>
        public static bool IsNumericType(object value)
        {
            Guard.Argument(value, nameof(value)).NotNull();

            return IsNumericTypeCode(Type.GetTypeCode(value.GetType()));
        }

        /// <summary>
        /// Checks to see if a value can be considered numeric. If the type is numeric then it
        /// automaticaly is. If the value is String or Char, an attempt is made to parse it
        /// as a double: if the attempt succeeds, true is returned.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value is numeric.</returns>
        public static bool IsNumeric(object value)
        {
            // Ripped from Microsoft.VisualBasic.dll using ILSpy and hacked until it works.
            // http://stackoverflow.com/questions/1749966/c-sharp-how-to-determine-whether-a-type-is-a-number
            IConvertible convertible = value as IConvertible;
            if (convertible == null)
            {
                char[] array = value as char[];
                if (array == null)
                {
                    return false;
                }
                value = new string(array);
            }

            TypeCode typeCode = convertible.GetTypeCode();
            if (typeCode == TypeCode.String || typeCode == TypeCode.Char)
            {
                string valueAsString = convertible.ToString(null);

                /*
                 * PD: For my purposes I do not want to consider hex and octal as numbers.
                 * For example, "F" is considered numeric if you uncomment the code
                 * below, but you will get an exception if you try and convert it to a 
                 * double.
                try
                {
                    if (IsHexValue(value) || IsOctalValue(value))
                        return true;
                }
                catch (StackOverflowException ex)
                {
                    throw ex;
                }
                catch (OutOfMemoryException ex2)
                {
                    throw ex2;
                }
                catch (ThreadAbortException ex3)
                {
                    throw ex3;
                }
                catch (Exception)
                {
                    return false;
                }
                */

                return Double.TryParse(valueAsString, out _);
            }

            return IsNumericTypeCode(typeCode);
        }

        /// <summary>
        /// Checks to see if a string value represents a hex string.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the string can be parsed as a hex number.</returns>
        public static bool IsHexValue(string value)
        {
            return Int32.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _);
        }

        /// <summary>
        /// Checks to see if a string value represents an octal number.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the string can be parsed as an octal number.</returns>
        public static bool IsOctalValue(string value)
        {
            try
            {
                Convert.ToInt32(value, 8);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (StackOverflowException)
            {
                return false;
            }
        }

        /// <summary>
        /// Converts a string to the "best match" fundamental type.
        /// nulls are returned as null strings.
        /// True/true/False/false are returned as the corresponding boolean.
        /// Things that look like DateTimes are returned as such.
        /// Things that look like TimeSpans according to the format [d].hh:mm:ss[.fff] are returned as TimeSpans
        /// (days and fractions of a second are optional).
        /// If it looks like a Guid according to one of the format specifiers D, B, P or X then a Guid is returned.
        /// Hex literals (0x...) are returned as the corresponding int or long (always the signed type).
        /// Finally, things that look like doubles, decimals, longs or ints are returned as such. You can use the
        /// thousands separator too, e.g. "123,456.78".
        /// If none of the above match, the original string is returned.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="culture">Culture to use for parsing.</param>
        /// <returns>The appropriate converted value.</returns>
        public static object StringToBest(string value, CultureInfo culture)
        {
            if (value == null)
                return value;

            if (value.Equals("true", StringComparison.Ordinal) || value.Equals("True", StringComparison.Ordinal))
                return true;

            if (value.Equals("false", StringComparison.Ordinal) || value.Equals("False", StringComparison.Ordinal))
                return false;

            // Check for timespans. Must do this before DateTimes.
            string[] timeSpanFormats = new string[] { @"d\.hh\:mm\:ss", @"hh\:mm\:ss", @"hh\:mm\:ss\.fff", @"d\.hh\:mm\:ss\.fff" };
            TimeSpan timeSpanResult;
            if (TimeSpan.TryParseExact(value, timeSpanFormats, culture, out timeSpanResult))
                return timeSpanResult;

            DateTime dt;
            if (DateTime.TryParse(value, culture, DateTimeStyles.AllowWhiteSpaces, out dt))
                return dt;

            // Check for Guids. Do not use the N format because it could easily clash with an int or a string.
            Guid guidResult;
            if (Guid.TryParseExact(value, "D", out guidResult))
                return guidResult;
            if (Guid.TryParseExact(value, "B", out guidResult))
                return guidResult;
            if (Guid.TryParseExact(value, "P", out guidResult))
                return guidResult;
            if (Guid.TryParseExact(value, "X", out guidResult))
                return guidResult;

            // Check for hex literals.
            // TODO: There are other cases, see https://msdn.microsoft.com/en-us/library/aa664674%28v=vs.71%29.aspx
            if (value.StartsWith("0x", StringComparison.Ordinal))
            {
                string v = value.Substring(2);
                int intHexResult;
                if (Int32.TryParse(v, NumberStyles.HexNumber, culture, out intHexResult))
                    return intHexResult;
                long longHexResult;
                if (Int64.TryParse(v, NumberStyles.HexNumber, culture, out longHexResult))
                    return longHexResult;

                return value;
            }

            // Check for numbers with explicit type suffixes.
            if (value.EndsWith("M", StringComparison.OrdinalIgnoreCase))
            {
                string v = value.Substring(0, value.Length - 1);
                if (Decimal.TryParse(v, NumberStyles.Number, culture, out decimal decimal_result))
                    return decimal_result;
            }

            double double_result;
            if (value.EndsWith("D", StringComparison.OrdinalIgnoreCase))
            {
                string v = value.Substring(0, value.Length - 1);
                if (Double.TryParse(v, NumberStyles.Number | NumberStyles.AllowExponent, culture, out double_result))
                    return double_result;
            }

            long long_result;
            if (value.EndsWith("L", StringComparison.OrdinalIgnoreCase))
            {
                string v = value.Substring(0, value.Length - 1);
                if (Int64.TryParse(v, NumberStyles.Integer | NumberStyles.AllowThousands, culture, out long_result))
                    return long_result;
            }

            bool double_ok = Double.TryParse(value, NumberStyles.Number | NumberStyles.AllowExponent, culture, out double_result);
            bool long_ok = Int64.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, culture, out long_result);
            bool int_ok = Int32.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, culture, out int int_result);

            if (value.Contains(culture.NumberFormat.CurrencyDecimalSeparator, StringComparison.OrdinalIgnoreCase) ||
                value.Contains("e", StringComparison.OrdinalIgnoreCase))
            {
                // Possibly a floating point number.
                if (double_ok)
                    return double_result;
            }
            else
            {
                // Possibly an int or long.
                if (long_ok && !int_ok)
                    return long_result;
                if (int_ok)
                    return int_result;
            }

            // Nothing else matches? Just return the string.
            return value;
        }
    }
}
