using System;
using System.Diagnostics;

namespace ClassLibrary1
{
    /// <summary>
    /// Represents the different options that can be used by the AppendCsv methods of
    /// StringExtensions and StringBuilder extensions.
    /// </summary>
    [DebuggerDisplay("Sep={Separator}, Del={Delimiter}, SkipNull={SkipNullValues}, SkipEmpty={SkipEmptyValues}, TrimStrings={TrimStrings}")]
    public class CsvOptions : ICloneable
    {
        /// <summary>
        /// Flag which allows us to optimise the static default instances - we can allocate
        /// them once if we arrange things so that they cannot be changed.
        /// </summary>
        public bool IsReadOnly { get; internal set; }

        private string separator;
        private bool writeLeadingSeparator;
        private string delimiter;
        private char[] charactersForcingDelimiter;
        private bool alwaysWriteDelimiter;
        private bool skipNullValues;
        private bool skipEmptyValues;
        private bool trimStrings;

        /// <summary>
        /// The character(s) to use as a separator between fields. Defaults to ",".
        /// </summary>
        public string Separator
        {
            get { return separator; }
            set
            {
                CheckReadOnly();
                separator = value;
            }
        }

        /// <summary>
        /// Whether to write a separator before the first argument. This is typically used
        /// when appending to a non-empty string or StringBuilder.
        /// </summary>
        public bool WriteLeadingSeparator
        {
            get { return writeLeadingSeparator; }
            set
            {
                CheckReadOnly();
                writeLeadingSeparator = value;
            }
        }

        /// <summary>
        /// The character(s) to use as a delimiter around fields. Defaults to " (double quote).
        /// If the delimiter appears in the terms being appended, it is replaced by doubling it.
        /// </summary>
        public string Delimiter
        {
            get { return delimiter; }
            set
            {
                CheckReadOnly();
                delimiter = value;
            }
        }

        /// <summary>
        /// Gets or sets the set of characters that force a value to be wrapped by the delimiter.
        /// Defaults to <c>\r, \n, " and , (comma).</c>
        /// </summary>
        /// <value>
        /// The set of characters that force a value to be wrapped by the delimiter.
        /// </value>
        public char[] CharactersForcingDelimiter
        {
            get { return charactersForcingDelimiter; }
            set
            {
                CheckReadOnly();
                charactersForcingDelimiter = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to always write a delimiter. If false, a
        /// delimiter is only written if one of <see cref="CharactersForcingDelimiter"/> occurs
        /// in the value being appended.
        /// </summary>
        public bool AlwaysWriteDelimiter
        {
            get { return alwaysWriteDelimiter; }
            set
            {
                CheckReadOnly();
                alwaysWriteDelimiter = value;
            }
        }

        /// <summary>
        /// Whether to skip nulls. If true, when a field is null it will not be written, and
        /// neither will the Separator.
        /// </summary>
        public bool SkipNullValues
        {
            get { return skipNullValues; }
            set
            {
                CheckReadOnly();
                skipNullValues = value;
            }
        }

        /// <summary>
        /// Whether to skip empty strings. If true, when a field is empty it will not be written, and
        /// neither will the Separator.
        /// </summary>
        public bool SkipEmptyValues
        {
            get { return skipEmptyValues; }
            set
            {
                CheckReadOnly();
                skipEmptyValues = value;
            }
        }

        /// <summary>
        /// Whether to trim strings before writing them. If true, the trimming is done before the
        /// check for <code>SkipNullValues</code>, so if both these values are true whitespace
        /// fields will be skipped.
        /// </summary>
        public bool TrimStrings
        {
            get { return trimStrings; }
            set
            {
                CheckReadOnly();
                trimStrings = value;
            }
        }

        /// <summary>
        /// Initialises a new instance of the <code>CsvOptions</code> class that is compliant with
        /// the RFC 4180 spec as described at https://en.wikipedia.org/wiki/Comma-separated_values#Standardization
        /// and https://tools.ietf.org/html/rfc4180
        /// </summary>
        public CsvOptions()
        {
            Separator = ",";
            Delimiter = "\"";
            CharactersForcingDelimiter = new char[] { '\r', '\n', '"', ',' };
        }


        /// <summary>
        /// A <see cref="CsvOptions"/> object that is compliant with the the RFC 4180 spec as described at
        /// https://en.wikipedia.org/wiki/Comma-separated_values#Standardization
        /// and https://tools.ietf.org/html/rfc4180
        /// </summary>
        public static readonly CsvOptions Default = new CsvOptions()
        {
            IsReadOnly = true
        };

        /// <summary>
        /// Returns a CsvOptions object configured suitable for "crunching down" objects
        /// into human readable strings. It skips null or empty strings and does not use
        /// a delimiter. The end result is a compact, comma-separated list, but it is not
        /// necessarily valid if written to a file.
        /// </summary>
        [Obsolete("Use HumanReadable or HumanReadableWithSpace instead.")]
        public static readonly CsvOptions CrunchingOptions = new CsvOptions()
        {
            Delimiter = String.Empty,
            CharactersForcingDelimiter = null,
            SkipNullValues = true,
            SkipEmptyValues = true,
            TrimStrings = true,
            IsReadOnly = true
        };

        /// <summary>
        /// A <see cref="CsvOptions"/> object that will produce human-readable CSV by using a separator of ","
        /// and eliminating null and empty strings.
        /// </summary>
        public static readonly CsvOptions HumanReadable = new CsvOptions()
        {
            Delimiter = String.Empty,
            CharactersForcingDelimiter = null,
            SkipNullValues = true,
            SkipEmptyValues = true,
            TrimStrings = true,
            IsReadOnly = true
        };

        /// <summary>
        /// A <see cref="CsvOptions"/> object that will produce human-readable CSV by using a separator of ", "
        /// and eliminating null and empty strings.
        /// </summary>
        public static readonly CsvOptions HumanReadableWithSpace = new CsvOptions()
        {
            Delimiter = String.Empty,
            Separator = ", ",
            CharactersForcingDelimiter = null,
            SkipNullValues = true,
            SkipEmptyValues = true,
            TrimStrings = true,
            IsReadOnly = true
        };

        private void CheckReadOnly()
        {
            if (IsReadOnly)
                throw new InvalidOperationException("This " + nameof(CsvOptions) + " instance cannot be changed.");
        }

        #region ICloneable Implementation (Copy constructor style)
        public CsvOptions(CsvOptions rhs)
        {
            separator = rhs.separator;
            writeLeadingSeparator = rhs.writeLeadingSeparator;
            delimiter = rhs.delimiter;
            charactersForcingDelimiter = rhs.charactersForcingDelimiter;
            alwaysWriteDelimiter = rhs.alwaysWriteDelimiter;
            skipNullValues = rhs.skipNullValues;
            skipEmptyValues = rhs.skipEmptyValues;
            trimStrings = rhs.trimStrings;
            IsReadOnly = false;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public CsvOptions Clone()
        {
            return new CsvOptions(this);
        }
        #endregion
    }
}
