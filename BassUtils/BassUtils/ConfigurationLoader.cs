using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

/*
Your app.config should look like this. In the <section> tag, the name can be anything, but there must be
a matching tag somewhere in the <configuration> block. We can use attributes or nodes or a mixture in the
sections, depending on how we define the classes.

On your C# classes you can use
  - the [DefaultValue] attribute
  - attributes understood by the XmlSerializer such as [XmlAttribute]
  - attributes from the System.ComponentModel.DataAnnotations namespace, which is useful for validation,
    for example [Required], [RegularExpression], and [EmailAddress].
  - enumerated members, they will be converted correctly
  - attributes that are themselves classes (nesting).

Restrictions
  - Your *ConfigurationSection classes must implement IConfigurationSectionHandler, which you automatically
    do by inheriting this class. However, it means an alternative implementation whereby you use this
    class as a member variable (composition instead of inheritance) is not worth the effort.

This class is based on the code I found in this article http://www.codeproject.com/Articles/6730/Custom-Objects-From-the-App-Config-file,
which I found via http://codecutout.com/xml-deserialization-from-app-config
But it has been extensively enhanced.


n.b. There is a full working example in the BassUtils.Tests project.


    <?xml version="1.0" encoding="utf-8"?>
    <configuration>
        <configSections>
            <section name="AnyName" type="Foo.Core.FirstConfigurationSection, Foo.Core" />
            <section name="PaymentDetails" type="Foo.Core.SecondConfigurationSection, Foo.Core" />
        </configSections>

        <AnyName FirstName="Philip" Surname="Daniels" />

        <PaymentDetails>
            <CardType>Visa</CardType>
            <Discount>20</Discount>
        </PaymentDetails>
    </configuration>


The corresponding class definitions are:


    public sealed class FirstConfigurationSection : ConfigLoader<FirstConfigurationSection>
    {
        [XmlAttribute]
        public string FirstName { get; set; }

        [XmlAttribute]
        public string Surname { get; set; }
    }

    public sealed class SecondConfigurationSection : ConfigLoader<SecondConfigurationSection>
    {
        public string CardType { get; set; }
        public int Discount { get; set; }
    }


And the configurations can be loaded with

    var config = new SecondConfigurationSection();
    config = config.Load();

 */




namespace BassUtils
{
    /// <summary>
    /// This is designed to handle Custom Configuration Section in an Application Configuration file.
    /// Inherit this class to produce a class which will automatically load the data from the config
    /// file. This can save a huge amount of boilerplate; there is extensive documentation in the
    /// class source file (available on Github) and a full working example in the BassUtils.Test project.
    /// </summary>
    public abstract class ConfigurationLoader : IConfigurationSectionHandler
    {
        /// <summary>
        /// Initialise a new instance of the ConfigLoader.
        /// </summary>
        public ConfigurationLoader()
        {
        }

        /// <summary>
        /// Initialise a new instance of the ConfigLoader.
        /// </summary>
        /// <param name="load">Whether to load the configuration from file.</param>
        public ConfigurationLoader(bool load)
        {
            if (load)
                Load();
        }

        /// <summary>
        /// Initialise a new instance of the ConfigLoader. Use this overload when you have
        /// one type that can be used to load several different configuration sections.
        /// </summary>
        /// <param name="sectionName">The name of the configuration section to load. Can be null,
        /// in which case the name of the first section is used.</param>
        public ConfigurationLoader(string sectionName)
        {
            Load(sectionName);
        }

        /// <summary>
        /// Loads or reloads this configuration from the config file.
        /// Uses the name of the first section associated with this type.
        /// </summary>
        public void Load()
        {
            Load(null);
        }

        /// <summary>
        /// Loads or reloads this configuration from the config file.
        /// </summary>
        /// <param name="sectionName">The name of the configuration section to load.</param>
        public void Load(string sectionName)
        {
            var config = InnerLoad(sectionName);
            PropertyCopier.CopyProperties(config, this);
        }

        /// <summary>
        /// Loads the configuration.
        /// </summary>
        /// <param name="sectionName">The name of the section to load. Can be null, in which
        /// case it will be automatically determined by using the first name in app.config
        /// that is associated with this type.</param>
        /// <exception cref="ConfigurationErrorsException">If there are validation errors or any other errors
        /// when loading the section.</exception>
        /// <returns>Loaded configuration object.</returns>
        protected virtual object InnerLoad(string sectionName)
        {
            // It's weird, but first we have to find our name, then we can call the ConfigurationManager
            // which will in turn call Create().
            try
            {
                if (sectionName == null)
                {
                    sectionName = GetFirstConfigurationSectionName();
                    if (sectionName == null)
                        throw new ConfigurationErrorsException("The configuration section for type " + GetType() + " could not be found.");
                }
                object config = ConfigurationManager.GetSection(sectionName);
                if (config == null)
                    throw new ConfigurationErrorsException("The section " + sectionName + " for type " + GetType() + " could not be found.");
                return config;
            }
            catch (ConfigurationErrorsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msg = "An error occurred while loading the section";
                if (sectionName != null)
                    msg += " " + sectionName;
                var cex = new ConfigurationErrorsException(msg, ex);
                throw cex;
            }
        }

        /// <summary>
        /// A method which is called by the CLR when parsing the App.Config file. If custom sections
        /// are found, then an entry in the configuration file will tell the runtime to call this method,
        /// passing in the XmlNode required.
        /// </summary>
        /// <param name="parent">The configuration settings in a corresponding parent configuration section.</param>
        /// <param name="configContext">An HttpConfigurationContext when Create is called from the ASP.NET configuration system. Otherwise,
        /// this parameter is reserved and is a null reference (Nothing in Visual Basic).</param>
        /// <param name="section">The <see cref="XmlNode"/> that contains the configuration information from the configuration file.
        /// Provides direct access to the XML contents of the configuration section.</param>
        /// <returns>The deserialized representation of the configuration section.</returns>
        /// <exception cref="System.Configuration.ConfigurationException">The Configuration file is not well formed,
        /// or the Custom section is not configured correctly, or the type of configuration handler was not specified correctly
        /// or the type of object was not specified correctly.
        /// </exception>
        public object Create(object parent, object configContext, XmlNode section)
        {
            Type typeOfThis = GetType();

            XmlSerializer ser = null;
            string rootName = section.Name;
            if (rootName == null)
                ser = new XmlSerializer(typeOfThis);
            else
                ser = new XmlSerializer(typeOfThis, new XmlRootAttribute(rootName));

            using (XmlNodeReader rdr = new XmlNodeReader(section))
            {
                var config = ser.Deserialize(rdr);
                ApplyDefaultValues(config);
                var errors = ValidateLoadedObject(config);
                if (errors != null && errors.Count > 0)
                {
                    string msg = ErrorsToString(errors, section);
                    throw new ConfigurationErrorsException(msg);
                }

                return config;
            }
        }

        /// <summary>
        /// Finds the name of the configuration section that is handling this type, or returns null
        /// if no such section can be found.
        /// </summary>
        /// <returns>The name of the ConfigurationSection, or null if no matches are found.</returns>
        protected virtual string GetFirstConfigurationSectionName()
        {
            var configSection = GetFirstSectionForThisType();
            if (configSection == null)
                return null;
            else
                return configSection.SectionInformation.Name;
        }

        /// <summary>
        /// Find the first configuration section designed to handle this type. This is done by scanning
        /// the configSections element in the config file for a section that is implementing this type.
        /// </summary>
        /// <returns>The matching ConfigurationSection object, or null if no matches are found.</returns>
        protected virtual ConfigurationSection GetFirstSectionForThisType()
        {
            // TODO: Search other nodes, see http://stackoverflow.com/questions/10331844/how-to-read-configsections
            var cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (var sec in cfg.Sections.Cast<ConfigurationSection>())
            {
                if (IsSectionForThisType(sec))
                    return sec;
            }

            return null;
        }

        /// <summary>
        /// Determine whether a configuration section is designed to handle this type
        /// by checking the "type" property of the declaration refers to this type.
        /// </summary>
        /// <param name="section">The section to check.</param>
        /// <returns>True if the section is for this type, false otherwise.</returns>
        protected virtual bool IsSectionForThisType(ConfigurationSection section)
        {
            try
            {
                Type thisType = this.GetType();
                var info = section.SectionInformation;
                if (info.Type.StartsWith(thisType.FullName, StringComparison.Ordinal))
                {
                    // Double check.
                    Type sectionType = Type.GetType(info.Type);
                    return thisType == sectionType;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Finds all properties in the object that have a <code>DefaultValueAttribute</code>
        /// on them and sets them if the properties currently have the same value as the
        /// default for their type, e.g. only set the value if numbers are 0, strings are null, etc.
        /// </summary>
        /// <param name="value">Thing to set defaults on.</param>
        protected virtual void ApplyDefaultValues(object value)
        {
            // Find all properties that have a default and set them.
            Type t = value.GetType();

            var properties = from p in t.GetProperties()
                             let attrs = p.GetCustomAttributes(typeof(DefaultValueAttribute), false)
                             where attrs != null && attrs.Count() > 0
                             select new { Property = p, Attribute = attrs.First() as DefaultValueAttribute };

            foreach (var property in properties)
            {
                var currentVal = property.Property.GetValue(value, null);
                var defaultTypeVal = property.Property.PropertyType.IsValueType ? Activator.CreateInstance(property.Property.PropertyType) : null;
                if (Object.Equals(currentVal, defaultTypeVal))
                {
                    property.Property.SetValue(value, property.Attribute.Value, null);
                }
            }
        }

        /// <summary>
        /// Check that the loaded object is valid. You can apply any validation
        /// attribute that you want (even your own, use custom messages etc.).
        /// </summary>
        /// <param name="thing">Deserialized section object.</param>
        protected virtual IList<ValidationResult> ValidateLoadedObject(object thing)
        {
            var context = new ValidationContext(thing, null, null);
            var errors = new List<ValidationResult>();
            Validator.TryValidateObject(thing, context, errors, true);
            return errors;
        }

        /// <summary>
        /// Render any configuration errors down to a useful string.
        /// </summary>
        /// <param name="errors">Set of errors.</param>
        /// <param name="section">The XmlNode section, used to get the name.</param>
        /// <returns>String rep.</returns>
        protected virtual string ErrorsToString(IEnumerable<ValidationResult> errors, XmlNode section)
        {
            var sb = new StringBuilder();

            if (errors != null && errors.Count() > 0)
            {
                sb.AppendFormat("There are errors in your .config file at section {0}.", section.Name);
                sb.AppendLine();
                foreach (var error in errors)
                {
                    sb.AppendFormat("  {0}{1}", error.ErrorMessage, Environment.NewLine);
                }
            }

            return sb.ToString();
        }
    }
}

