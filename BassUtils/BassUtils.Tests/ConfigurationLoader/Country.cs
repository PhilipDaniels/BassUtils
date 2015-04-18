using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BassUtils.Tests.ConfigurationLoader
{
    /// <summary>
    /// Notice this is just a normal class.
    /// </summary>
    [DebuggerDisplay("{Name}, Pop={PopulationInMillions}, Area={AreaInSquareKm}")]
    public sealed class Country
    {
        [XmlAttribute]
        [Required]
        public string Name { get; set; }

        [XmlAttribute]
        public int PopulationInMillions { get; set; }

        [XmlAttribute]
        public double AreaInSquareKm { get; set; }
    }
}
