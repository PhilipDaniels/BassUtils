﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BassUtils.Tests.ConfigurationLoaderTests
{
    [DebuggerDisplay("{Name]")]
    public sealed class ContinentConfigurationSection : ConfigurationLoader
    {
        /// <summary>
        /// You must supply a default constructor for the XmlSerializer.
        /// </summary>
        public ContinentConfigurationSection()
        {
        }

        public ContinentConfigurationSection(bool load)
            : base(load)
        {
        }

        [XmlAttribute]
        [Required]
        public string Name { get; set; }

        public List<Country> Countries { get; set; }
    }
}