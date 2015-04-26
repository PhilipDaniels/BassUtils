using System;
using System.Reflection;
using NUnit.Framework;

namespace BassUtils.Tests
{
    [TestFixture]
    public class AppDomainExtensionTests
    {
        [Test]
        public void IsAlreadyLoaded_ForThisAssembly_ReturnsTrue()
        {
            var domain = AppDomain.CurrentDomain;
            string path = Assembly.GetExecutingAssembly().Location;

            Assert.IsTrue(domain.IsLoaded(path));
        }
    }
}
