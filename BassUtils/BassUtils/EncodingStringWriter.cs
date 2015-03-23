using System;
using System.IO;
using System.Text;

namespace BassUtils
{
    /// <summary>
    /// A subclass of StringWriter that allows the encoding to be set.
    /// </summary>
    public class EncodingStringWriter : StringWriter
    {
        /// <summary>
        /// Gets the encoding in use.
        /// </summary>
        public new Encoding Encoding { get; private set; }

        /// <summary>
        /// Construct a new <code>EncodingStringWriter</code> object.
        /// </summary>
        /// <param name="builder">StringBuilder used to back the writer.</param>
        /// <param name="formatProvider">Format provider.</param>
        /// <param name="encoding">Encoding to be used.</param>
        public EncodingStringWriter(StringBuilder builder, IFormatProvider formatProvider, Encoding encoding)
            : base(builder, formatProvider)
        {
            Encoding = encoding.ThrowIfNull("encoding");
        }
    }
}
