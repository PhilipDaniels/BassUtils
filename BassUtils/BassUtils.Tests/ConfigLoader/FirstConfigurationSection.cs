using System.Xml.Serialization;

namespace BassUtils.Tests.ConfigLoader
{
    public sealed class FirstConfigurationSection : ConfigLoader<FirstConfigurationSection>
    {
        [XmlAttribute]
        public string FirstName { get; set; }

        [XmlAttribute]
        public string Surname { get; set; }
    }
}
