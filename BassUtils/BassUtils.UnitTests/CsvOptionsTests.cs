using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BassUtils.UnitTests
{
    [TestClass]
    public class CsvOptionsTests
    {
        Action[] Mutators(CsvOptions options)
        {
            return new Action[]
            {
                () => options.WriteLeadingSeparator = true,
                () => options.AlwaysWriteDelimiter = true,
                () => options.CharactersForcingDelimiter = null,
                () => options.Delimiter = ".",
                () => options.Separator = ":",
                () => options.SkipEmptyValues = true,
                () => options.SkipNullValues = true,
                () => options.TrimStrings = true,
                () => options.WriteLeadingSeparator = true
            };
        }

        [TestMethod]
        public void DefaultOptions_AreReadOnly()
        {
            foreach (var act in Mutators(CsvOptions.Default))
            {
                Assert.ThrowsException<InvalidOperationException>(() => act());
            }
        }

        [TestMethod]
        public void HumanReadableOptions_AreReadOnly()
        {
            foreach (var act in Mutators(CsvOptions.HumanReadable))
            {
                Assert.ThrowsException<InvalidOperationException>(() => act());
            }
        }

        [TestMethod]
        public void HumanReadableWithSpaceOptions_AreReadOnly()
        {
            foreach (var act in Mutators(CsvOptions.HumanReadableWithSpace))
            {
                Assert.ThrowsException<InvalidOperationException>(() => act());
            }
        }

        [TestMethod]
        public void ConstructedOptions_AreWriteable()
        {
            var options = new CsvOptions();
            foreach (var act in Mutators(options))
            {
                act();
            }
        }

        [TestMethod]
        public void Clone_CopiesAllPropertiesButsSetsReadOnlyToFalse()
        {
            Assert.IsTrue(CsvOptions.Default.IsReadOnly);

            var options = CsvOptions.Default.Clone();

            Assert.IsFalse(options.IsReadOnly);
            Assert.AreEqual(options.Separator, CsvOptions.Default.Separator);
            Assert.AreEqual(options.WriteLeadingSeparator, CsvOptions.Default.WriteLeadingSeparator);
            Assert.AreEqual(options.Delimiter, CsvOptions.Default.Delimiter);
            Assert.AreEqual(options.CharactersForcingDelimiter, CsvOptions.Default.CharactersForcingDelimiter);
            Assert.AreEqual(options.AlwaysWriteDelimiter, CsvOptions.Default.AlwaysWriteDelimiter);
            Assert.AreEqual(options.SkipNullValues, CsvOptions.Default.SkipNullValues);
            Assert.AreEqual(options.SkipEmptyValues, CsvOptions.Default.SkipEmptyValues);
            Assert.AreEqual(options.TrimStrings, CsvOptions.Default.TrimStrings);
        }
    }
}
