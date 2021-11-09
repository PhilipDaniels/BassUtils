using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Dawn;

namespace BassUtils
{
    /// <summary>
    /// Various handy string extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Left aligns <paramref name="value"/> and pads with spaces to the specified <paramref name="width"/>.
        /// </summary>
        /// <param name="value">The input text.</param>
        /// <param name="width">The final width of the text.</param>
        /// <returns>Padded result.</returns>
        public static string PadAndAlign(this string value, int width)
        {
            return PadAndAlign(value, width, width, PaddingAlignment.Left, ' ');
        }

        /// <summary>
        /// Left aligns <paramref name="value"/> and pads with spaces to the specified <paramref name="minWidth"/>,
        /// but trims the output if it exceeds <paramref name="maxWidth"/>.
        /// </summary>
        /// <param name="value">The input text.</param>
        /// <param name="minWidth">The minimum width of the result.</param>
        /// <param name="maxWidth">The maximum width of the result.</param>
        /// <returns>Padded result.</returns>
        public static string PadAndAlign(this string value, int minWidth, int maxWidth)
        {
            return PadAndAlign(value, minWidth, maxWidth, PaddingAlignment.Left, ' ');
        }

        /// <summary>
        /// Applies the specified <paramref name="alignment"/> to <paramref name="value"/> and pads 
        /// with spaces to the specified <paramref name="minWidth"/>, but trims the output if it 
        /// exceeds <paramref name="maxWidth"/>.
        /// </summary>
        /// <param name="value">The input text.</param>
        /// <param name="minWidth">The minimum width of the result.</param>
        /// <param name="maxWidth">The maximum width of the result.</param>
        /// <param name="alignment">The alignment of the text.</param>
        /// <returns>Padded result.</returns>
        public static string PadAndAlign(this string value, int minWidth, int maxWidth, PaddingAlignment alignment)
        {
            return PadAndAlign(value, minWidth, maxWidth, alignment, ' ');
        }

        /// <summary>
        /// Applies the specified <paramref name="alignment"/> to <paramref name="value"/> and pads 
        /// with <paramref name="paddingCharacter"/> to the specified <paramref name="minWidth"/>, but trims the output if it 
        /// exceeds <paramref name="maxWidth"/>.
        /// </summary>
        /// <param name="value">The input text.</param>
        /// <param name="minWidth">The minimum width of the result.</param>
        /// <param name="maxWidth">The maximum width of the result.</param>
        /// <param name="alignment">The alignment of the text.</param>
        /// <param name="paddingCharacter">The character to pad with.</param>
        /// <returns>Padded result.</returns>
        public static string PadAndAlign(this string value, int minWidth, int maxWidth, PaddingAlignment alignment, char paddingCharacter)
        {
            Guard.Argument(minWidth, nameof(minWidth)).Min(0);
            Guard.Argument(maxWidth, nameof(maxWidth)).Min(0);
            Guard.Argument(minWidth, nameof(minWidth)).Max(maxWidth, (x, y) => "minWidth must be less than or equal to the maxWidth.");

            if (value == null)
                value = "";

            if (value.Length > maxWidth)
            {
                switch (alignment)
                {
                    case PaddingAlignment.Left:
                        // The left hand side is most important and should be retained.
                        value = value.Substring(0, maxWidth);
                        break;
                    case PaddingAlignment.Right:
                        // The right hand side is most important and should be retained.
                        value = value.Substring(value.Length - maxWidth);
                        break;
                    case PaddingAlignment.Center:
                        // The center is most important and should be retained.
                        var leftCharsToChop = (value.Length - maxWidth) / 2;
                        value = value.Substring(leftCharsToChop, maxWidth);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Unhandled alignment: " + alignment.ToString());
                }
            }
            else if (value.Length < minWidth)
            {
                switch (alignment)
                {
                    case PaddingAlignment.Left:
                        value = value.PadRight(minWidth, paddingCharacter);
                        break;
                    case PaddingAlignment.Right:
                        value = value.PadLeft(minWidth, paddingCharacter);
                        break;
                    case PaddingAlignment.Center:
                        var leftSpaces = (minWidth - value.Length) / 2;
                        value = new String(paddingCharacter, leftSpaces) + value;
                        value = value.PadRight(minWidth, paddingCharacter);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Unhandled alignment: " + alignment.ToString());
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the characters before the first occurence of <paramref name="separator"/>.
        /// The search for <paramref name="separator"/> is case-insensitive.
        /// If <paramref name="separator"/> does not occur in <paramref name="value"/> or
        /// <paramref name="value"/> is null then null is returned.
        /// </summary>
        /// <param name="value">The text to search.</param>
        /// <param name="separator">The value to search for.</param>
        /// <returns>The portion of text before the value.</returns>
        public static string Before(this string value, string separator)
        {
            return value.Before(separator, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns the characters before the first occurence of <paramref name="separator"/>.
        /// If <paramref name="separator"/> does not occur in <paramref name="value"/> or
        /// <paramref name="value"/> is null then null is returned.
        /// </summary>
        /// <param name="value">The text to search.</param>
        /// <param name="separator">The value to search for.</param>
        /// <param name="comparisonType">Type of string comparison to apply.</param>
        /// <returns>The portion of text before the value.</returns>
        public static string Before(this string value, string separator, StringComparison comparisonType)
        {
            Guard.Argument(separator, nameof(separator)).NotNull();

            if (value == null)
                return null;
            int index = value.IndexOf(separator, comparisonType);
            if (index == -1)
                return null;
            else
                return value.Substring(0, index);
        }

        /// <summary>
        /// Returns the characters after the first occurence of <paramref name="separator"/>.
        /// The search for <paramref name="separator"/> is case-insensitive.
        /// If <paramref name="separator"/> does not occur in <paramref name="value"/> or
        /// <paramref name="value"/> is null then null is returned.
        /// </summary>
        /// <param name="value">The text to search.</param>
        /// <param name="separator">The value to search for.</param>
        /// <returns>The portion of text after the value.</returns>
        public static string After(this string value, string separator)
        {
            return value.After(separator, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns the characters after the first occurence of <paramref name="separator"/>.
        /// If <paramref name="separator"/> does not occur in <paramref name="value"/> or
        /// <paramref name="value"/> is null then null is returned.
        /// </summary>
        /// <param name="value">The text to search.</param>
        /// <param name="separator">The value to search for.</param>
        /// <param name="comparisonType">Type of string comparison to apply.</param>
        /// <returns>The portion of text after the value.</returns>
        public static string After(this string value, string separator, StringComparison comparisonType)
        {
            Guard.Argument(separator, nameof(separator)).NotNull();

            if (value == null)
                return null;
            int index = value.IndexOf(separator, comparisonType);
            if (index == -1)
                return null;
            else
                return value.Substring(index + separator.Length);
        }

        /// <summary>
        /// Returns the characters both before and after the first occurrence of <paramref name="separator"/>.
        /// The search for <paramref name="separator"/> is case-insensitive.
        /// If <paramref name="separator"/> does not occur in <paramref name="value"/> or
        /// <paramref name="value"/> is null then null is returned.
        /// </summary>
        /// <param name="value">The text to search.</param>
        /// <param name="separator">The value to search for.</param>
        /// <param name="before">The portion of text before the search value.</param>
        /// <param name="after">The portion of text after the search value.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#")]
        public static void BeforeAndAfter(this string value, string separator, out string before, out string after)
        {
            value.BeforeAndAfter(separator, StringComparison.OrdinalIgnoreCase, out before, out after);
        }

        /// <summary>
        /// Returns the characters both before and after the first occurrence of <paramref name="separator"/>.
        /// The search for <paramref name="separator"/> is case-insensitive.        /// 
        /// If <paramref name="separator"/> does not occur in <paramref name="value"/> or
        /// <paramref name="value"/> is null then null is returned.
        /// </summary>
        /// <param name="value">The text to search.</param>
        /// <param name="separator">The value to search for.</param>
        /// <param name="comparisonType">Type of string comparison to apply.</param>
        /// <param name="before">The portion of text before the search value.</param>
        /// <param name="after">The portion of text after the search value.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "4#")]
        public static void BeforeAndAfter(this string value, string separator, StringComparison comparisonType, out string before, out string after)
        {
            Guard.Argument(separator, nameof(separator)).NotNull();

            before = value.Before(separator, comparisonType);
            after = value.After(separator, comparisonType);
        }

        /// <summary>
        /// Returns the characters both before and after the first occurrence of <paramref name="separator"/>.
        /// If <paramref name="separator"/> does not occur in <paramref name="value"/> or
        /// <paramref name="value"/> is null then null is returned.
        /// </summary>
        /// <param name="value">The text to search.</param>
        /// <param name="separator">The value to search for.</param>
        /// <returns>A 2-tuple, where the first item is the substring before the value, and the second item is the substring after the value.</returns>
        public static Tuple<string, string> BeforeAndAfter(this string value, string separator)
        {
            return BeforeAndAfter(value, separator, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns the characters both before and after the first occurrence of <paramref name="separator"/>.
        /// If <paramref name="separator"/> does not occur in <paramref name="value"/> or
        /// <paramref name="value"/> is null then null is returned.
        /// </summary>
        /// <param name="value">The text to search.</param>
        /// <param name="separator">The value to search for.</param>
        /// <param name="comparisonType">Type of string comparison to apply.</param>
        /// <returns>A 2-tuple, where the first item is the substring before the value, and the second item is the substring after the value.</returns>
        public static Tuple<string, string> BeforeAndAfter(this string value, string separator, StringComparison comparisonType)
        {
            Guard.Argument(separator, nameof(separator)).NotNull();

            string before = value.Before(separator, comparisonType);
            string after = value.After(separator, comparisonType);
            var t = Tuple.Create(before, after);
            return t;
        }

        /// <summary>
        /// Check to see whether <paramref name="value"/> contains <paramref name="valueToFind"/>, and
        /// allow you to specify whether it is case-sensitive or not.
        /// </summary>
        /// <param name="value">The text to search.</param>
        /// <param name="valueToFind">The value to search for.</param>
        /// <param name="comparisonType">Type of string comparison to apply.</param>
        /// <returns>True if text contains the value according to the comparisonType.</returns>
        public static bool Contains(this string value, string valueToFind, StringComparison comparisonType)
        {
            Guard.Argument(value, nameof(value)).NotNull();
            Guard.Argument(valueToFind, nameof(valueToFind)).NotNull();

            return value.IndexOf(valueToFind, comparisonType) != -1;
        }

        /// <summary>
        /// Determines whether <paramref name="value"/> contains all the words
        /// in <paramref name="words"/>. The check uses <c>StringComparison.OrdinalIgnoreCase</c>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="words">The words to check for.</param>
        /// <returns><c>True</c> if the value contains all the words, <c>false</c> otherwise</returns>
        public static bool ContainsAll(this string value, params string[] words)
        {
            return ContainsAll(value, StringComparison.OrdinalIgnoreCase, words);
        }

        /// <summary>
        /// Determines whether <paramref name="value"/> contains all the words
        /// in <paramref name="words"/>.
        /// </summary>
        /// <param name="value">The value. If null, false is returned.</param>
        /// <param name="comparisonType">The type of string comparison to use.</param>
        /// <param name="words">The words to check for. Can be empty or null, in which case <c>true</c> is returned.</param>
        /// <returns><c>True</c> if the value contains all the words, <c>false</c> otherwise</returns>
        public static bool ContainsAll(this string value, StringComparison comparisonType, params string[] words)
        {
            if (value == null)
            {
                return false;
            }

            if (words == null || words.Count() == 0)
            {
                return true;
            }

            return words.All(w => value.IndexOf(w, comparisonType) != -1);
        }

        /// <summary>
        /// Replace all occurrences of <paramref name="valueToFind"/> in <paramref name="value"/>
        /// with <paramref name="replacement"/>.
        /// </summary>
        /// <param name="value">The text to search.</param>
        /// <param name="valueToFind">The string to replace.</param>
        /// <param name="replacement">The replacement value.</param>
        /// <param name="comparison">Type of string comparison to apply.</param>
        /// <returns>String with appropriate replacements made.</returns>
        public static string Replace(this string value, string valueToFind, string replacement, StringComparison comparison)
        {
            Guard.Argument(value, nameof(value)).NotNull();
            Guard.Argument(valueToFind, nameof(valueToFind)).NotNull();
            Guard.Argument(replacement, nameof(replacement)).NotNull();

            var sb = new StringBuilder();

            int previousIndex = 0;
            int index = value.IndexOf(valueToFind, comparison);
            while (index != -1)
            {
                sb.Append(value.Substring(previousIndex, index - previousIndex));
                sb.Append(replacement);
                index += valueToFind.Length;

                previousIndex = index;
                index = value.IndexOf(valueToFind, index, comparison);
            }
            sb.Append(value.Substring(previousIndex));

            return sb.ToString();
        }

        /// <summary>
        /// Checks <paramref name="value"/> to see whether it matches the file-globbing
        /// pattern in <paramref name="pattern"/>.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <param name="pattern">The wildcard, where "*" means any sequence of characters, and "?" means any single character.</param>
        /// <returns><c>true</c> if the string matches the given pattern; otherwise <c>false</c>.</returns>
        public static bool MatchesFileGlobPattern(this string value, string pattern)
        {
            var r = new Regex
                (
                "^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
                );

            return r.IsMatch(value);
        }

        /// <summary>
        /// Converts the input string to proper case, i.e. initial caps and
        /// lowercase rest.
        /// </summary>
        /// See also http://msdn.microsoft.com/en-us/library/system.globalization.textinfo.totitlecase.aspx
        /// which has limitations.
        /// <remarks>
        /// </remarks>
        /// <param name="value">The string to convert.</param>
        /// <param name="culture">The culture to use for upper/lower-case conversion.</param>
        /// <returns>Proper cased string.</returns>
        public static string ToProperCase(this string value, CultureInfo culture)
        {
            Guard.Argument(value, nameof(value)).NotNull();

            StringBuilder sb = new StringBuilder();
            bool emptyBefore = true;
            foreach (char ch in value)
            {
                char chThis = ch;
                if (Char.IsWhiteSpace(chThis))
                {
                    emptyBefore = true;
                }
                else
                {
                    if (Char.IsLetter(chThis) && emptyBefore)
                        chThis = Char.ToUpper(chThis, culture);
                    else
                        chThis = Char.ToLower(chThis, culture);
                    emptyBefore = false;
                }

                sb.Append(chThis);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Call String.Format in a safe fashion; if the number of args doesn't match the
        /// number of placeholders in the format string then just return the format string
        /// rather than throwing an exception.
        /// 
        /// If the format string or args is null, the original <paramref name="format"/> is returned.
        /// </summary>
        /// <param name="culture">Culture to use for formatting.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">Arguments to be substituted.</param>
        /// <returns>Formatted string, or the original string if the formatting fails.</returns>
        public static string SafeFormat(this string format, CultureInfo culture, params object[] args)
        {
            try
            {
                if (format == null || args == null || !args.Any())
                    return format;

                string result = String.Format(culture, format, args);
                return result;
            }
            catch (FormatException)
            {
                return format;
            }
        }

        /// <summary>
        /// Call String.Format in a safe fashion; if the number of args doesn't match the
        /// number of placeholders in the format string then just return the format string
        /// rather than throwing an exception. The InvariantCulture is used for formatting.
        ///
        /// If the format string or args is null, the original <paramref name="format"/> is returned.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">Arguments to be substituted.</param>
        /// <returns>Formatted string, or the original string if the formatting fails.</returns>
        public static string SafeFormat(this string format, params object[] args)
        {
            return SafeFormat(format, CultureInfo.InvariantCulture, args);
        }

        /// <summary>
        /// Removes the newlines characters '\r' and '\n' from the specified string by replacing them
        /// with <c>string.Empty</c>. An '\r\n' pair is replaced by a single space (to avoid joining
        /// words across line endings).
        /// </summary>
        /// <param name="value">The string to remove newlines from. Can be null, in which case
        /// null is returned.</param>
        /// <returns>String with newlines removed.</returns>
        public static string RemoveNewLines(this string value)
        {
            if (value == null)
                return null;

            return value.Replace("\r\n", " ").Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        /// <summary>
        /// Safely apply the Trim() operation to a string.
        /// If <paramref name="value"/> is null then null is
        /// returned, else <code>toTrim.Trim() is returned.</code>
        /// Whitespace strings are converted to null.
        /// </summary>
        /// <param name="value">The string to trim. Can be null.</param>
        /// <returns>Trimmed string, or null.</returns>
        public static string SafeTrim(this string value)
        {
            return SafeTrim(value, true);
        }

        /// <summary>
        /// Safely apply the Trim() operation to a string.
        /// If <paramref name="value"/> is null then null is
        /// returned, else <code>toTrim.Trim() is returned.</code>
        /// </summary>
        /// <param name="value">The string to trim. Can be null.</param>
        /// <param name="convertWhiteSpaceToNull">Whether to convert whitespace strings to null.</param>
        /// <returns>Trimmed string, or null.</returns>
        public static string SafeTrim(this string value, bool convertWhiteSpaceToNull)
        {
            if (value == null)
                return null;

            var s = value.Trim();
            if (convertWhiteSpaceToNull && s.Length == 0)
                return null;
            else
                return s;
        }

        /// <summary>
        /// Given a string InCamelCaseLikeThis, split it into words
        /// separated by a space.
        /// </summary>
        /// <param name="value">The string to split.</param>
        /// <returns>String with spaces inserted at word breaks.</returns>
        public static string SplitCamelCaseIntoWords(this string value)
        {
            Guard.Argument(value, nameof(value)).NotNull();

            string result = Regex.Replace
                (
                value,
                "([A-Z][A-Z]*)",
                " $1",
                RegexOptions.Compiled
                );

            result = result.Trim();
            return result;
        }

        /// <summary>
        /// Return the leftmost characters of str. This function will not
        /// throw an exception if str is null or is shorter than length.
        /// If str is null then null is returned.
        /// </summary>
        /// <param name="value">The string to extract from.</param>
        /// <param name="length">The number of characters to extract. Can be zero.</param>
        /// <returns>Leftmost <paramref name="length"/> characters of <paramref name="value"/>.</returns>
        public static string Left(this string value, int length)
        {
            Guard.Argument(length, nameof(length)).Min(0);

            if (value == null)
                return null;

            if (length >= value.Length)
                return value;
            else
                return value.Substring(0, length);
        }

        /// <summary>
        /// Return the rightmost characters of str. This function will not
        /// throw an exception if str is null or is shorter than length.
        /// If str is null then null is returned.
        /// </summary>
        /// <param name="value">The string to extract from.</param>
        /// <param name="length">The number of characters to extract.</param>
        /// <returns>Rightmost <paramref name="length"/> characters of <paramref name="value"/>.</returns>
        public static string Right(this string value, int length)
        {
            Guard.Argument(length, nameof(length)).Min(0);

            if (value == null)
                return null;

            if (length >= value.Length)
                return value;
            else
                return value.Substring(value.Length - length, length);
        }

        /// <summary>
        /// Returns a leading number from a string such as "123Hello", using the InvariantCulture
        /// for conversion.
        /// </summary>
        /// <param name="value">The value to extract the number from.</param>
        /// <returns>The leading number, or null if there is no leading number.</returns>
        public static decimal? GetLeadingNumber(this string value)
        {
            Guard.Argument(value, nameof(value)).NotNull();

            return GetLeadingNumber(value, CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Returns a leading number from a string such as "123Hello".
        /// </summary>
        /// <param name="value">The value to extract the number from.</param>
        /// <param name="numberFormatInfo">Number format to use for conversion.</param>
        /// <returns>The leading number, or null if there is no leading number.</returns>
        public static decimal? GetLeadingNumber(this string value, NumberFormatInfo numberFormatInfo)
        {
            Guard.Argument(value, nameof(value)).NotNull();
            Guard.Argument(numberFormatInfo, nameof(numberFormatInfo)).NotNull();

            var index = 0;
            while (index < value.Length && (Char.IsDigit(value[index]) || value[index].ToString() == numberFormatInfo.NumberDecimalSeparator))
            {
                index++;
            }

            if (index == 0)
                return null;

            var s = value.Substring(0, index);
            return Convert.ToDecimal(s, numberFormatInfo);
        }

        /// <summary>
        /// Returns a trailing number from a string such as "Hello123", using the InvariantCulture
        /// for conversion.
        /// </summary>
        /// <param name="value">The value to extract the number from.</param>
        /// <returns>The trailing number, or null if there is no trailing number.</returns>
        public static decimal? GetTrailingNumber(this string value)
        {
            return GetTrailingNumber(value, CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Returns a trailing number from a string such as "Hello123". This will handle ints and
        /// numbers with a decimal point (as specified by the number format), but not scientific notation.
        /// You can get a <seealso cref="NumberFormatInfo"/> from a <seealso cref="CultureInfo"/> object.
        /// </summary>
        /// <param name="value">The value to extract the number from.</param>
        /// <param name="numberFormatInfo">Number format to use for conversion.</param>
        /// <returns>The trailing number, or null if there is no trailing number.</returns>
        public static decimal? GetTrailingNumber(this string value, NumberFormatInfo numberFormatInfo)
        {
            Guard.Argument(value, nameof(value)).NotNull();
            Guard.Argument(numberFormatInfo, nameof(numberFormatInfo)).NotNull();

            var index = value.Length - 1;
            while (index >= 0 && (Char.IsDigit(value[index]) || value[index].ToString() == numberFormatInfo.NumberDecimalSeparator))
            {
                index--;
            }

            if (index == value.Length - 1)
                return null;

            var s = value.Substring(index + 1);
            return Convert.ToDecimal(s, numberFormatInfo);
        }

        static readonly Regex BadFileNameCharacters = new Regex(@"[\\\/:\*\?""<>|]");

        /// <summary>
        /// Removes characters that are invalid in Windows filenames from a string.
        /// </summary>
        /// <param name="value">The string to remove characters from.</param>
        /// <returns>New string with invalid filename characters removed.</returns>
        public static string RemoveInvalidFileNameCharacters(this string value)
        {
            return BadFileNameCharacters.Replace(value, String.Empty);
        }

        /// <summary>
        /// Repeats <paramref name="value"/> <paramref name="count"/> times.
        /// </summary>
        /// <param name="value">The string to repeat. Must not be null.</param>
        /// <param name="count">The number of times to repeat. Must be 0 or more.</param>
        /// <returns>Repeated string.</returns>
        public static string Repeat(this string value, int count)
        {
            Guard.Argument(value, nameof(value)).NotNull();
            Guard.Argument(count, nameof(count)).Min(0);

            var sb = new StringBuilder(value.Length * count + 1);
            for (int i = 0; i < count; i++)
                sb.Append(value);
            return sb.ToString();
        }

        /// <summary>
        /// Converts a delimited string to a list of objects of the requested type.
        /// </summary>
        /// <typeparam name="T">Type of thing in the output list.</typeparam>
        /// <param name="value">The input string. Can be null or empty, which results in an empty list.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="allowDuplicates">Whether to allow duplicates in the result.</param>
        /// <returns>List of things.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists",
            Justification = "It's a static method, will not be inherited.")]
        public static List<T> ToList<T>(this string value, string delimiter, bool allowDuplicates)
            where T : IConvertible
        {
            Guard.Argument(value, nameof(value)).NotNull();
            Guard.Argument(delimiter, nameof(delimiter)).NotNull();

            var result = value.ToList((string s) => (T)Convert.ChangeType(s, typeof(T), CultureInfo.InvariantCulture), delimiter, allowDuplicates);
            return result;
        }

        /// <summary>
        /// Converts a delimited string to a list of objects of the requested type,
        /// using a specified conversion function.
        /// </summary>
        /// <typeparam name="T">Type of thing in the output list.</typeparam>
        /// <param name="value">The input string. Can be null or empty, which results in an empty list.</param>
        /// <param name="converter">A function to convert strings to objects of type <c>T</c>.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="allowDuplicates">Whether to allow duplicates in the result.</param>
        /// <returns>List of things.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists",
            Justification = "It's a static method, will not be inherited.")]
        public static List<T> ToList<T>
            (
            this string value,
            Func<string, T> converter,
            string delimiter,
            bool allowDuplicates
            )
        where T : IConvertible
        {
            Guard.Argument(value, nameof(value)).NotNull();
            Guard.Argument(converter, nameof(converter)).NotNull();
            Guard.Argument(delimiter, nameof(delimiter)).NotNull();

            var result = new List<T>();

            if (!String.IsNullOrEmpty(value))
            {
                string[] split = value.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                result = split.ToList().ConvertAll<T>(s => converter(s));
            }

            if (allowDuplicates)
                return result;
            else
                return result.Distinct().ToList();
        }

        /// <summary>
        /// Change a character in a string.
        /// If you need to change many characters, it is probably best to use a StringBuilder.
        /// </summary>
        /// <param name="value">The string to change in.</param>
        /// <param name="index">The index of the character. An exception results if the index is not in range.</param>
        /// <param name="newChar">The new character.</param>
        /// <returns>New string with character replaced.</returns>
        public static string SetChar(this string value, int index, char newChar)
        {
            Guard.Argument(value, nameof(value)).NotNull();

            if (index < 0 || index >= value.Length)
                throw new ArgumentOutOfRangeException("index", "index must be 0 to value.Length - 1");

            char[] chars = value.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }

        /// <summary>
        /// Appends a character to the <paramref name="value"/>, but only if the value does not already
        /// end with that character. This is useful for building up lists.
        /// </summary>
        /// <param name="value">The string to append to.</param>
        /// <param name="charToAppend">The character to append.</param>
        /// <returns>The appended (or not, as the case may be), string.</returns>
        public static string AppendIfDoesNotEndWith(this string value, char charToAppend)
        {
            Guard.Argument(value, nameof(value)).NotNull();

            if (!value.EndsWith(charToAppend.ToString()))
                return value + charToAppend;
            else
                return value;
        }

        /// <summary>
        /// Appends a string to the <paramref name="value"/>, but only if the value does not already
        /// end with that string. This is useful for building up lists.
        /// </summary>
        /// <param name="value">The string to append to.</param>
        /// <param name="valueToAppend">The string to append.</param>
        /// <returns>The appended (or not, as the case may be), string.</returns>
        public static string AppendIfDoesNotEndWith(this string value, string valueToAppend)
        {
            return value.AppendIfDoesNotEndWith(valueToAppend, StringComparison.Ordinal);
        }

        /// <summary>
        /// Appends a string to the <paramref name="value"/>, but only if the value does not already
        /// end with that string. This is useful for building up lists.
        /// </summary>
        /// <param name="value">The string to append to.</param>
        /// <param name="valueToAppend">The string to append.</param>
        /// <param name="comparisonType">String comparison type to use when checking to see if
        /// the string ends with the value.</param>
        /// <returns>The appended (or not, as the case may be), string.</returns>
        public static string AppendIfDoesNotEndWith(this string value, string valueToAppend, StringComparison comparisonType)
        {
            Guard.Argument(value, nameof(value)).NotNull();
            Guard.Argument(valueToAppend, nameof(valueToAppend)).NotNull();

            if (!value.EndsWith(valueToAppend, comparisonType))
                return value + valueToAppend;
            else
                return value;
        }

        /// <summary>
        /// Trims <paramref name="valueToAppend"/> before appending it. The value cannot be null.
        /// </summary>
        /// <param name="value">The string to append to.</param>
        /// <param name="valueToAppend">The value to trim and append.</param>
        /// <returns>The appended string.</returns>
        public static string TrimAppend(this string value, string valueToAppend)
        {
            Guard.Argument(value, nameof(value)).NotNull();
            Guard.Argument(valueToAppend, nameof(valueToAppend)).NotNull();

            return value + valueToAppend.Trim();
        }

        /// <summary>
        /// Appends <paramref name="args"/> in a "CSV style" to the end of the <paramref name="value"/>.
        /// See the <see cref="CsvOptions"/> class for ways to control the appending.
        /// This overload uses the default options.
        /// </summary>
        /// <param name="value">The string to append to.</param>
        /// <param name="args">The arguments to append.</param>
        /// <returns>The appended string.</returns>
        public static string AppendCsv(this string value, params object[] args)
        {
            Guard.Argument(value, nameof(value)).NotNull();

            var options = value.Length > 0
                          ? WithLeadingSeparator
                          : NoLeadingSeparator;

            return value.AppendCsv(options, args);
        }

        /// <summary>
        /// Appends <paramref name="args"/> in a "CSV style" to the end of the <paramref name="value"/>.
        /// See the <see cref="CsvOptions"/> class for ways to control the appending.
        /// </summary>
        /// <param name="value">The string to append to.</param>
        /// <param name="options">Options to control the appending.</param>
        /// <param name="args">The arguments to append.</param>
        /// <returns>The appended string.</returns>
        public static string AppendCsv
            (
            this string value,
            CsvOptions options,
            params object[] args
            )
        {
            Guard.Argument(value, nameof(value)).NotNull();
            Guard.Argument(options, nameof(options)).NotNull();

            var sb = new StringBuilder(value);
            sb.AppendCsv(options, args);
            return sb.ToString();
        }

        private static readonly CsvOptions WithLeadingSeparator = new CsvOptions()
        {
            WriteLeadingSeparator = true,
            IsReadOnly = true
        };

        private static readonly CsvOptions NoLeadingSeparator = new CsvOptions()
        {
            WriteLeadingSeparator = false,
            IsReadOnly = true
        };

    }
}
