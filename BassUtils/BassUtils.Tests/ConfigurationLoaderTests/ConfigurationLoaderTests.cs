using System;
using System.Configuration;
using System.Linq;
using NUnit.Framework;

namespace BassUtils.Tests.ConfigurationLoaderTests
{
    [TestFixture]
    public class ConfigurationLoaderTests
    {
        [Test]
        public void Load_CanLoadAttributesCorrectly()
        {
            try
            {
                var config = new FirstConfigurationSection(true);
                Assert.AreEqual("Philip", config.FirstName);
                Assert.AreEqual("Daniels", config.Surname);
            }
            catch (ConfigurationErrorsException cex)
            {
                // This will not happen during normal test running. But if you are playing with the tests to understand
                // what you can do with this class it is useful to look at the message on this exception.
                string msg = cex.Message;
            }
        }

        [Test]
        public void Load_CanLoadNodesAndAttributesCorrectly()
        {
            try
            {
                var config = new SecondConfigurationSection(true);
                Assert.AreEqual("CreditCards", config.Mechanism);
                Assert.AreEqual("Visa", config.CardType);
                Assert.AreEqual(20, config.Discount);
                Assert.AreEqual(PaymentClass.Invoice, config.PaymentClass);
                Assert.AreEqual("Something.exe", config.ProgramName);
            }
            catch (ConfigurationErrorsException cex)
            {
                // This will not happen during normal test running. But if you are playing with the tests to understand
                // what you can do with this class it is useful to look at the message on this exception.
                string msg = cex.Message;
            }
        }

        [Test]
        public void Load_CanLoadSectionsByName_AndNestedTypes()
        {
            try
            {
                var config = new ContinentConfigurationSection("Continent");
                Assert.AreEqual("North America", config.Name);
                Assert.AreEqual(3, config.Countries.Count);

                var usa = config.Countries.Single(c => c.Name.Equals("USA", StringComparison.OrdinalIgnoreCase));
                var canada = config.Countries.Single(c => c.Name.Equals("Canada", StringComparison.OrdinalIgnoreCase));
                var mexico = config.Countries.Single(c => c.Name.Equals("Mexico", StringComparison.OrdinalIgnoreCase));

                Assert.AreEqual(320, usa.PopulationInMillions);
                Assert.AreEqual(36, canada.PopulationInMillions);
                Assert.AreEqual(118, mexico.PopulationInMillions);
                Assert.AreEqual(9147593, usa.AreaInSquareKm);
                Assert.AreEqual(9984670, canada.AreaInSquareKm);
                Assert.AreEqual(1972550, mexico.AreaInSquareKm);


                config = new ContinentConfigurationSection("Continent2");
                Assert.AreEqual("South America", config.Name);
                Assert.AreEqual(2, config.Countries.Count);

                var brazil = config.Countries.Single(c => c.Name.Equals("Brazil", StringComparison.OrdinalIgnoreCase));
                var argentina = config.Countries.Single(c => c.Name.Equals("Argentina", StringComparison.OrdinalIgnoreCase));

                Assert.AreEqual(203, brazil.PopulationInMillions);
                Assert.AreEqual(43, argentina.PopulationInMillions);
                Assert.AreEqual(8515767, brazil.AreaInSquareKm);
                Assert.AreEqual(2780400, argentina.AreaInSquareKm);
            }
            catch (ConfigurationErrorsException cex)
            {
                // This will not happen during normal test running. But if you are playing with the tests to understand
                // what you can do with this class it is useful to look at the message on this exception.
                string msg = cex.Message;
            }
        }
    }
}
