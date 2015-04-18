using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BassUtils.Tests.ConfigLoader
{
    [DebuggerDisplay("{Name]")]
    public sealed class ContinentConfigurationSection : ConfigLoader<ContinentConfigurationSection>
    {
        [XmlAttribute]
        [Required]
        public string Name { get; set; }

        public List<Country> Countries { get; set; }
    }
}
