using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BassUtils.Tests.ConfigLoader
{
    public sealed class SecondConfigurationSection : ConfigLoader<SecondConfigurationSection>
    {
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
