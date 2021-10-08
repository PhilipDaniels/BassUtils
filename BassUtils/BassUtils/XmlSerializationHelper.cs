using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using Dawn;

namespace BassUtils
{
    /// <summary>
    /// Utility methods to help with serializing and deserializing objects to/from XML.
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        /// Serializes an object to an XML string using the <code>XmlSerializer</code> class.
        /// </summary>
        /// <typeparam name="T">Type of thing to serialize.</typeparam>
        /// <param name="value">The thing to serialize.</param>
        /// <returns>XML string represenation of the thing.</returns>
        public static string SerializeObjectToXmlString<T>(T value)
            where T: class
        {
            Guard.Argument(value, nameof(value)).NotNull();

            var serializer = new XmlSerializer(typeof(T));
            using (var sw = new StringWriter(CultureInfo.InvariantCulture))
            {
                serializer.Serialize(sw, value);
                return sw.ToString();
            }
        }

        /// <summary>
        /// Deserializes an object from an XML string using the <code>XmlSerializer</code> class.
        /// </summary>
        /// <typeparam name="T">Type of thing to deserialize.</typeparam>
        /// <param name="xml">The xml string containing the serialized object.</param>
        /// <returns>The deserialized object instance.</returns>
        public static T DeserializeXmlStringToObject<T>(string xml)
        {
            Guard.Argument(xml, nameof(xml)).NotNull();

            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(xml))
            {
                var data = (T)serializer.Deserialize(reader);
                return data;
            }
        }
    }
}
