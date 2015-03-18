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
        public new Encoding Encoding { get; private set; }

        public EncodingStringWriter(StringBuilder builder, IFormatProvider formatProvider, Encoding encoding)
            : base(builder, formatProvider)
        {
            Encoding = encoding.ThrowIfNull("encoding");
        }
    }
}
