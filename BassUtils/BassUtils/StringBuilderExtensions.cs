using System;
using System.IO;
using System.Text;

namespace BassUtils
{
    /// <summary>
    /// Contains extensions for the StringBuilder class.
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Check to see whether a StringBuilder ends with a particular character.
        /// Empty builders always return false.
        /// </summary>
        /// <param name="builder">The StringBuilder.</param>
        /// <param name="value">The character to look for.</param>
        /// <returns>True if the builder ends with the character, false otherwise.</returns>
        public static bool EndsWith(this StringBuilder builder, char value)
        {
            builder.ThrowIfNull("builder");

            if (builder.Length == 0)
                return false;
            else
                return builder[builder.Length - 1] == value;
        }

        /// <summary>
        /// Check to see whether a StringBuilder ends with a particular string.
        /// Empty builders and empty values always return false.
        /// </summary>
        /// <remarks>If your string is a single character, the overload using a character value
        /// is more efficient as it avoids a string allocation.</remarks>
        /// <param name="builder">The StringBuilder.</param>
        /// <param name="value">The string to look for.</param>
        /// <returns>True if the builder ends with the string, false otherwise.</returns>
        public static bool EndsWith(this StringBuilder builder, string value)
        {
            return builder.EndsWith(value, StringComparison.Ordinal);
        }

        /// <summary>
        /// Check to see whether a StringBuilder ends with a particular string.
        /// Empty builders and empty values always return false.
        /// </summary>
        /// <param name="builder">The StringBuilder.</param>
        /// <param name="value">The string to look for.</param>
        /// <param name="comparisonType">String comparison type to use when checking to see if
        /// the builder ends with the value.</param>
        /// <returns>True if the builder ends with the string, false otherwise.</returns>
        public static bool EndsWith(this StringBuilder builder, string value, StringComparison comparisonType)
        {
            builder.ThrowIfNull("builder");
            value.ThrowIfNull("value");

            if (builder.Length == 0 || value.Length == 0 || builder.Length < value.Length)
                return false;

            int startIndex = builder.Length - value.Length;
            string end = builder.ToString(startIndex, value.Length);
            return end.Equals(value, comparisonType);
        }

        /// <summary>
        /// Appends a character to the <paramref name="builder"/>, but only if the builder does not already
        /// end with that character. This is useful for building up lists.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">The character to append.</param>
        /// <returns>The builder that was passed in, to enable chaining.</returns>
        public static StringBuilder AppendIfDoesNotEndWith(this StringBuilder builder, char value)
        {
            builder.ThrowIfNull("builder");

            if (!builder.EndsWith(value))
                builder.Append(value);
            return builder;
        }

        /// <summary>
        /// Appends a string to the <paramref name="builder"/>, but only if the builder does not already
        /// end with that string. This is useful for building up lists.
        /// </summary>
        /// <remarks>If your string is a single character, the overload using a character value
        /// is more efficient as it avoids a string allocation.</remarks>
        /// <param name="builder">The builder.</param>
        /// <param name="value">The character to append.</param>
        /// <returns>The builder that was passed in, to enable chaining.</returns>
        public static StringBuilder AppendIfDoesNotEndWith(this StringBuilder builder, string value)
        {
            return builder.AppendIfDoesNotEndWith(value, StringComparison.Ordinal);
        }

        /// <summary>
        /// Appends a string to the <paramref name="builder"/>, but only if the builder does not already
        /// end with that string. This is useful for building up lists.
        /// </summary>
        /// <remarks>If your string is a single character, the overload using a character value
        /// is more efficient as it avoids a string allocation.</remarks>
        /// <param name="builder">The builder.</param>
        /// <param name="value">The character to append.</param>
        /// <param name="comparisonType">String comparison type to use when checking to see if
        /// the builder ends with the value.</param>
        /// <returns>The builder that was passed in, to enable chaining.</returns>
        public static StringBuilder AppendIfDoesNotEndWith(this StringBuilder builder, string value, StringComparison comparisonType)
        {
            builder.ThrowIfNull("builder");
            value.ThrowIfNull("value");

            if (!builder.EndsWith(value, comparisonType))
                builder.Append(value);
            return builder;
        }

        /// <summary>
        /// Trims <paramref name="value"/> before appending it. The value cannot be null.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">The value to trim and append.</param>
        /// <returns>The builder that was passed in, to enable chaining.</returns>
        public static StringBuilder TrimAppend(this StringBuilder builder, string value)
        {
            builder.ThrowIfNull("builder");
            value.ThrowIfNull("value");

            builder.Append(value.Trim());
            return builder;
        }

        /// <summary>
        /// Appends <paramref name="args"/> in a "CSV style" to the end of the builder.
        /// See the <see cref="CSVOptions"/> class for ways to control the appending.
        /// This overload uses the default options.
        /// </summary>
        /// <param name="builder">The builder to append to.</param>
        /// <param name="args">The arguments to append.</param>
        /// <returns>The builder that was passed in, to enable chaining.</returns>
        public static StringBuilder AppendCSV(this StringBuilder builder, params object[] args)
        {
            builder.ThrowIfNull(nameof(builder));

            using (var sw = new StringWriter(builder))
            {
                sw.AppendCSV(args);
            }

            return builder;
        }

        /// <summary>
        /// Appends <paramref name="args"/> in a "CSV style" to the end of the builder.
        /// See the <see cref="CSVOptions"/> class for ways to control the appending.
        /// </summary>
        /// <param name="builder">The builder to append to.</param>
        /// <param name="options">Options to control the appending.</param>
        /// <param name="args">The arguments to append.</param>
        /// <returns>The builder that was passed in, to enable chaining.</returns>
        public static StringBuilder AppendCSV
            (
            this StringBuilder builder,
            CSVOptions options,
            params object[] args
            )
        {
            builder.ThrowIfNull(nameof(builder));
            options.ThrowIfNull(nameof(options));

            using (var sw = new StringWriter(builder))
            {
                sw.AppendCSV(options, args);
            }

            return builder;
        }
    }
}
