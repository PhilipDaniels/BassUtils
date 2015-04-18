using System;
using System.Diagnostics;

namespace BassUtils
{
    /// <summary>
    /// Represents the different options that can be used by the AppendCSV methods of
    /// StringExtensions and StringBuilder extensions.
    /// </summary>
    [DebuggerDisplay("Sep={Separator}, Del={Delimiter}, SkipNull={SkipNullValues}, SkipEmpty={SkipEmptyValues}, TrimStrings={TrimStrings}")]
    public class CSVOptions
    {
        /// <summary>
        /// The character(s) to use as a separator between fields. Defaults to ",".
        /// </summary>
        public string Separator { get; set; }

        /// <summary>
        /// The character(s) to use as a delimiter between fields. Defaults to " (double quote).
        /// If the delimiter appears in the terms being appended, it is replaced by doubling it.
        /// </summary>
        public string Delimiter { get; set; }

        /// <summary>
        /// Whether to skip nulls. If true, when a field is null it will not be written, and
        /// neither will the Separator.
        /// </summary>
        public bool SkipNullValues { get; set; }

        /// <summary>
        /// Whether to skip empty strings. If true, when a field is empty it will not be written, and
        /// neither will the Separator.
        /// </summary>
        public bool SkipEmptyValues { get; set; }

        /// <summary>
        /// Whether to trim strings before writing them. If true, the trimming is done before the
        /// check for <code>SkipNullValues</code>, so if both these values are true whitespace
        /// fields will be skipped.
        /// </summary>
        public bool TrimStrings { get; set; }

        /// <summary>
        /// Initialises a new instance of the <code>CSVOptions</code> class.
        /// </summary>
        public CSVOptions()
        {
            Separator = ",";
            Delimiter = "\"";
        }

        /// <summary>
        /// Returns a CSVOptions object configured suitable for "crunching down" objects
        /// into human readable strings. It skips null or empty strings and does not use
        /// a delimiter. The end result is a compact, comma-separated list.
        /// </summary>
        public static CSVOptions CrunchingOptions
        {
            get
            {
                return new CSVOptions()
                {
                    Delimiter = String.Empty,
                    SkipNullValues = true,
                    SkipEmptyValues = true,
                    TrimStrings = true
                };
            }
        }
    }
}
