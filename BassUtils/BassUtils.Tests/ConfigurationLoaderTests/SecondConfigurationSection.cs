using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BassUtils.Tests.ConfigurationLoaderTests
{
    public sealed class SecondConfigurationSection : ConfigurationLoader
    {
        /// <summary>
        /// You must supply a default constructor for the XmlSerializer.
        /// </summary>
        public SecondConfigurationSection()
        {
        }

        public SecondConfigurationSection(bool load)
            : base(load)
        {
        }

        [XmlAttribute]
        public string Mechanism { get; set; }

        public string CardType { get; set; }

        public int Discount { get; set; }

        public PaymentClass PaymentClass { get; set; }

        [Required]
        [RegularExpression(@".*\.exe")]
        public string ProgramName { get; set; }
    }
}
