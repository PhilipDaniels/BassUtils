using System;
using System.IO;
using Dawn;

namespace BassUtils
{
    /// <summary>
    /// Extensions for the <seealso cref="TextWriter"/> class.
    /// </summary>
    public static class TextWriterExtensions
    {
        /// <summary>
        /// Appends <paramref name="args"/> in a "CSV style" to the end of the writer.
        /// See the <see cref="CsvOptions"/> class for ways to control the appending.
        /// This overload uses the default options, which results in output compliant with
        /// <c>https://tools.ietf.org/html/rfc4180</c>.
        /// </summary>
        /// <param name="writer">The writer to append to.</param>
        /// <param name="args">The arguments to append.</param>
        /// <returns>The writer that was passed in, to enable chaining.</returns>
        public static TextWriter AppendCsv(this TextWriter writer, params object[] args)
        {
            return writer.AppendCsv(CsvOptions.Default, args);
        }

        /// <summary>
        /// Appends <paramref name="args"/> in a "CSV style" to the end of the writer.
        /// See the <see cref="CsvOptions"/> class for ways to control the appending.
        /// </summary>
        /// <param name="writer">The writer to append to.</param>
        /// <param name="options">Options to control the appending.</param>
        /// <param name="args">The arguments to append.</param>
        /// <returns>The writer that was passed in, to enable chaining.</returns>
        public static TextWriter AppendCsv
            (
            this TextWriter writer,
            CsvOptions options,
            params object[] args
            )
        {
            Guard.Argument(writer, nameof(writer)).NotNull();
            Guard.Argument(options, nameof(options)).NotNull();

            if (options.AlwaysWriteDelimiter && options.Delimiter == null)
                throw new ArgumentException("If options.AlwaysWriteDelimiter is true, options.Delimiter must be non-null.", nameof(options));

            if (args == null || args.Length == 0)
                return writer;

            bool separatorRequired = options.WriteLeadingSeparator;

            foreach (var arg in args)
            {
                if (arg == null && options.SkipNullValues)
                    continue;

                string s = String.Empty;
                if (arg != null)
                {
                    s = arg.ToString();
                    if (options.TrimStrings)
                        s = s.Trim();
                }

                if (options.SkipEmptyValues && s.Length == 0)
                    continue;

                // Delimiters inside terms cause problems, this is dealt with by doubling them.
                bool delimiterRequired = !String.IsNullOrEmpty(options.Delimiter) &&
                                         (
                                         options.AlwaysWriteDelimiter ||
                                         (options.CharactersForcingDelimiter != null && s.IndexOfAny(options.CharactersForcingDelimiter) >= 0)
                                         );

                if (delimiterRequired)
                {
                    s = s.Replace(options.Delimiter, options.Delimiter + options.Delimiter);
                }

                // We are now ready to output a term.
                if (separatorRequired)
                {
                    writer.Write(options.Separator);
                }
                else
                {
                    separatorRequired = true;
                }

                if (delimiterRequired)
                {
                    writer.Write(options.Delimiter);
                }

                writer.Write(s);

                if (delimiterRequired)
                {
                    writer.Write(options.Delimiter);
                }
            }

            return writer;
        }
    }
}
