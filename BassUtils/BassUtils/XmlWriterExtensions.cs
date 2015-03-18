using System;
using System.Globalization;
using System.Xml;

namespace BassUtils
{
    public static class XmlWriterExtensions
    {
        /// <summary>
        /// Writes an attribute, but only if the attribute is non-null.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        public static void WriteAttributeIfNonNull(this XmlWriter writer, string attributeName, object value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");
            value.ThrowIfNull("value");

            Type valType = value.GetType();

            if (valType == typeof(bool))
            {
                WriteAttributeIfNonNull(writer, attributeName, (bool)value);
            }
            else if (valType == typeof(bool?))
            {
                WriteAttributeIfNonNull(writer, attributeName, (bool?)value);
            }
            else if (valType == typeof(byte))
            {
                WriteAttributeIfNonNull(writer, attributeName, (byte)value);
            }
            else if (valType == typeof(byte?))
            {
                WriteAttributeIfNonNull(writer, attributeName, (byte?)value);
            }
            else if (valType == typeof(short))
            {
                WriteAttributeIfNonNull(writer, attributeName, (short)value);
            }
            else if (valType == typeof(short?))
            {
                WriteAttributeIfNonNull(writer, attributeName, (short?)value);
            }
            else if (valType == typeof(int))
            {
                WriteAttributeIfNonNull(writer, attributeName, (int)value);
            }
            else if (valType == typeof(int?))
            {
                WriteAttributeIfNonNull(writer, attributeName, (int?)value);
            }
            else if (valType == typeof(long))
            {
                WriteAttributeIfNonNull(writer, attributeName, (long)value);
            }
            else if (valType == typeof(long?))
            {
                WriteAttributeIfNonNull(writer, attributeName, (long?)value);
            }
            else if (valType == typeof(decimal))
            {
                WriteAttributeIfNonNull(writer, attributeName, (decimal)value);
            }
            else if (valType == typeof(decimal?))
            {
                WriteAttributeIfNonNull(writer, attributeName, (decimal?)value);
            }
            else if (valType == typeof(float))
            {
                WriteAttributeIfNonNull(writer, attributeName, (float)value);
            }
            else if (valType == typeof(float?))
            {
                WriteAttributeIfNonNull(writer, attributeName, (float?)value);
            }
            else if (valType == typeof(double))
            {
                WriteAttributeIfNonNull(writer, attributeName, (double)value);
            }
            else if (valType == typeof(double?))
            {
                WriteAttributeIfNonNull(writer, attributeName, (double?)value);
            }
            else if (valType == typeof(DateTime))
            {
                WriteAttributeIfNonNull(writer, attributeName, (DateTime)value);
            }
            else if (valType == typeof(DateTime?))
            {
                WriteAttributeIfNonNull(writer, attributeName, (DateTime?)value);
            }
            else if (valType == typeof(Guid))
            {
                WriteAttributeIfNonNull(writer, attributeName, (Guid)value);
            }
            else if (valType == typeof(Guid?))
            {
                WriteAttributeIfNonNull(writer, attributeName, (Guid?)value);
            }
            else if (valType == typeof(string))
            {
                WriteAttributeIfNonNull(writer, attributeName, (string)value);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Unhandled attribute type: " + valType.ToString());
            }
        }

        public static void WriteAttributeIfNonNull(this XmlWriter writer, string attributeName, string value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            writer.WriteAttributeString(attributeName, value);
        }

        public static void WriteAttributeIfNonNull(this XmlWriter writer, string attributeName, bool? value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
                writer.WriteAttributeString(attributeName, value.Value.ToString(CultureInfo.InvariantCulture));
        }

        public static void WriteAttributeIfNonNull(this XmlWriter writer, string attributeName, byte? value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
                writer.WriteAttributeString(attributeName, value.Value.ToString(CultureInfo.InvariantCulture));
        }

        public static void WriteAttributeIfNonNull(this XmlWriter writer, string attributeName, short? value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
                writer.WriteAttributeString(attributeName, value.Value.ToString(CultureInfo.InvariantCulture));
        }

        public static void WriteAttributeIfNonNull(this XmlWriter writer, string attributeName, int? value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
                writer.WriteAttributeString(attributeName, value.Value.ToString(CultureInfo.InvariantCulture));
        }

        public static void WriteAttributeIfNonNull(this XmlWriter writer, string attributeName, long? value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
                writer.WriteAttributeString(attributeName, value.Value.ToString(CultureInfo.InvariantCulture));
        }

        public static void WriteAttributeIfNonNull(this XmlWriter writer, string attributeName, decimal? value, string format)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
            {
                if (format == null)
                    writer.WriteAttributeString(attributeName, value.Value.ToString(CultureInfo.InvariantCulture));
                else
                    writer.WriteAttributeString(attributeName, value.Value.ToString(format, CultureInfo.InvariantCulture));
            }
        }

        public static void WriteAttributeIfNonNull(this XmlWriter writer, string attributeName, float? value, string format)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
            {
                if (format == null)
                    writer.WriteAttributeString(attributeName, value.Value.ToString(CultureInfo.InvariantCulture));
                else
                    writer.WriteAttributeString(attributeName, value.Value.ToString(format, CultureInfo.InvariantCulture));
            }
        }

        public static void WriteAttributeIfNonNull(this XmlWriter writer, string attributeName, double? value, string format)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
            {
                if (format == null)
                    writer.WriteAttributeString(attributeName, value.Value.ToString(CultureInfo.InvariantCulture));
                else
                    writer.WriteAttributeString(attributeName, value.Value.ToString(format, CultureInfo.InvariantCulture));
            }
        }

        public static void WriteAttributeIfNonNull(this XmlWriter writer, string attributeName, DateTime? value, string format)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
            {
                if (format == null)
                    writer.WriteAttributeString(attributeName, value.Value.ToString(CultureInfo.InvariantCulture));
                else
                    writer.WriteAttributeString(attributeName, value.Value.ToString(format, CultureInfo.InvariantCulture));
            }
        }

        public static void WriteAttributeIfNonNull(this XmlWriter writer, string attributeName, Guid? value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
                writer.WriteAttributeIfNonNull(attributeName, value.Value.ToString());
        }
    }
}
