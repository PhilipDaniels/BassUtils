using System.Xml.Serialization;

namespace BassUtils.Tests.ConfigLoader
{
    public sealed class FirstConfigurationSection : ConfigLoader<FirstConfigurationSection>
    {
        /// <summary>
        /// You must supply a default constructor for the XmlSerializer.
        /// </summary>
        public FirstConfigurationSection()
        {
        }

        public FirstConfigurationSection(bool load)
            : base(load)
        {
        }

        [XmlAttribute]
        public string FirstName { get; set; }

        [XmlAttribute]
        public string Surname { get; set; }
    }
}
