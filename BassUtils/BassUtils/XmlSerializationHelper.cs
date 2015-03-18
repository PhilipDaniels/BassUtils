using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace BassUtils
{
    public static class SerializationHelper
    {
        public static string SerializeObjectToXmlString<T>(T value)
        {
            value.ThrowIfNull("value");

            var serializer = new XmlSerializer(typeof(T));
            using (var sw = new StringWriter(CultureInfo.InvariantCulture))
            {
                serializer.Serialize(sw, value);
                return sw.ToString();
            }
        }

        public static T DeserializeXmlStringToObject<T>(string xml)
        {
            xml.ThrowIfNullOrWhiteSpace("xml");

            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(xml))
            {
                var data = (T)serializer.Deserialize(reader);
                return data;
            }
        }
    }
}
