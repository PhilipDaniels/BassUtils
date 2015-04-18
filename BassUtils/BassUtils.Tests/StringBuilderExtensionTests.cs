using System;
using System.Text;
using NUnit.Framework;

namespace BassUtils.Tests
{
    [TestFixture]
    public class StringBuilderExtensionTests
    {
        [Test]
        public void EndsWithChar_ForNullBuilder_ThrowsArgumentNullException()
        {
            StringBuilder sb = null;
            var ex = Assert.Throws<ArgumentNullException>(() => sb.EndsWith(','));
            Assert.AreEqual("builder", ex.ParamName);
        }

        [Test]
        public void EndsWithChar_ForEmptyBuilder_ReturnsFalse()
        {
            var sb = new StringBuilder();
            Assert.IsFalse(sb.EndsWith(','));
            Assert.IsFalse(sb.EndsWith(' '));
        }

        [Test]
        public void EndsWithChar_ForStringsEndingWith_ReturnsTrue()
        {
            var sb = new StringBuilder();
            sb.Append("a");
            Assert.IsTrue(sb.EndsWith('a'));
            sb.Append("b");
            Assert.IsTrue(sb.EndsWith('b'));
        }

        [Test]
        public void EndsWithChar_ForStringsNotEndingWith_ReturnsFalse()
        {
            var sb = new StringBuilder();
            sb.Append("a");
            Assert.IsFalse(sb.EndsWith('x'));
            sb.Append("b");
            Assert.IsFalse(sb.EndsWith('B'));
        }

        [Test]
        public void EndsWithString_ForNullBuilder_ThrowsArgumentNullException()
        {
            StringBuilder sb = null;
            var ex = Assert.Throws<ArgumentNullException>(() => sb.EndsWith(""));
            Assert.AreEqual("builder", ex.ParamName);
        }

        [Test]
        public void EndsWithString_ForEmptyBuilder_ReturnsFalse()
        {
            var sb = new StringBuilder();
            Assert.IsFalse(sb.EndsWith("a"));
            Assert.IsFalse(sb.EndsWith(" "));
            Assert.IsFalse(sb.EndsWith(""));
        }

        [Test]
        public void EndsWithString_ForStringsEndingWith_ReturnsTrue()
        {
            var sb = new StringBuilder();
            sb.Append("a");
            Assert.IsTrue(sb.EndsWith("a"));
            sb.Append("b");
            Assert.IsTrue(sb.EndsWith("b"));
            Assert.IsTrue(sb.EndsWith("ab"));
        }

        [Test]
        public void EndsWithString_ForStringsNotEndingWith_ReturnsFalse()
        {
            var sb = new StringBuilder();
            sb.Append("a");
            Assert.IsFalse(sb.EndsWith("x"));
            sb.Append("b");
            Assert.IsFalse(sb.EndsWith('x'));
        }

        [Test]
        public void EndsWithString_ForStringsLongerThanBuilder_ReturnsFalse()
        {
            var sb = new StringBuilder();
            sb.Append("a");
            Assert.IsFalse(sb.EndsWith("abc"));
        }

        [Test]
        public void EndsWithString_AppliesStringComparisonCorrectly()
        {
            var sb = new StringBuilder();
            sb.Append("a");
            Assert.IsFalse(sb.EndsWith("A"));
            Assert.IsTrue(sb.EndsWith("A", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void AppendIfDoesNotEndWithChar_ForNullBuilder_ThrowsArgumentNullException()
        {
            StringBuilder sb = null;
            var ex = Assert.Throws<ArgumentNullException>(() => sb.AppendIfDoesNotEndWith(','));
            Assert.AreEqual("builder", ex.ParamName);
        }

        [Test]
        public void AppendIfDoesNotEndWithChar_AfterAppending_ReturnsOriginalBuilder()
        {
            var sb = new StringBuilder();
            var sb2 = sb.AppendIfDoesNotEndWith(',');
            Assert.AreSame(sb, sb2);
        }

        [Test]
        public void AppendIfDoesNotEndWithChar_AppendsOnlyWhenNeeded()
        {
            var sb = new StringBuilder();
            sb.AppendIfDoesNotEndWith('a');
            Assert.AreEqual("a", sb.ToString());
            sb.AppendIfDoesNotEndWith('a');
            Assert.AreEqual("a", sb.ToString());
            sb.AppendIfDoesNotEndWith('b');
            Assert.AreEqual("ab", sb.ToString());
        }

        [Test]
        public void AppendIfDoesNotEndWithString_ForNullBuilder_ThrowsArgumentNullException()
        {
            StringBuilder sb = null;
            var ex = Assert.Throws<ArgumentNullException>(() => sb.AppendIfDoesNotEndWith(" "));
            Assert.AreEqual("builder", ex.ParamName);
        }

        [Test]
        public void AppendIfDoesNotEndWithString_AfterAppending_ReturnsOriginalBuilder()
        {
            var sb = new StringBuilder();
            var sb2 = sb.AppendIfDoesNotEndWith(" ");
            Assert.AreSame(sb, sb2);
        }

        [Test]
        public void AppendIfDoesNotEndWithString_AppendsOnlyWhenNeeded()
        {
            var sb = new StringBuilder();
            sb.AppendIfDoesNotEndWith("a");
            Assert.AreEqual("a", sb.ToString());
            sb.AppendIfDoesNotEndWith("a");
            Assert.AreEqual("a", sb.ToString());
            sb.AppendIfDoesNotEndWith("b");
            Assert.AreEqual("ab", sb.ToString());
            sb.AppendIfDoesNotEndWith("ab");
            Assert.AreEqual("ab", sb.ToString());
        }

        [Test]
        public void AppendIfDoesNotEndWithString_AppliesStringComparisonCorrectly()
        {
            var sb = new StringBuilder();
            sb.AppendIfDoesNotEndWith("a");
            Assert.AreEqual("a", sb.ToString());
            sb.AppendIfDoesNotEndWith("a");
            Assert.AreEqual("a", sb.ToString());
            sb.AppendIfDoesNotEndWith("A");
            Assert.AreEqual("aA", sb.ToString());
            sb.AppendIfDoesNotEndWith("a", StringComparison.OrdinalIgnoreCase);
            Assert.AreEqual("aA", sb.ToString());
        }

        [Test]
        public void TrimAppend_ForNullBuilder_ThrowsArgumentNullException()
        {
            StringBuilder sb = null;
            var ex = Assert.Throws<ArgumentNullException>(() => sb.TrimAppend(" "));
            Assert.AreEqual("builder", ex.ParamName);
        }

        [Test]
        public void TrimAppend_ForNullString_ThrowsArgumentNullException()
        {
            var sb = new StringBuilder();
            var ex = Assert.Throws<ArgumentNullException>(() => sb.TrimAppend(null));
            Assert.AreEqual("value", ex.ParamName);
        }

        [Test]
        public void TrimAppend_AfterAppending_ReturnsOriginalBuilder()
        {
            var sb = new StringBuilder();
            var sb2 = sb.TrimAppend("");
            Assert.AreSame(sb, sb2);
        }

        [Test]
        public void TrimAppend_TrimsThenAppendsCorrectly()
        {
            var sb = new StringBuilder();
            sb.TrimAppend(" ");
            Assert.AreEqual("", sb.ToString());
            sb.TrimAppend("a");
            Assert.AreEqual("a", sb.ToString());
            sb.TrimAppend("  a  ");
            Assert.AreEqual("aa", sb.ToString());
            sb.TrimAppend("\n\nbb\n\n\t");
            Assert.AreEqual("aabb", sb.ToString());
        }

        [Test]
        public void AppendCSV_ForNullBuilder_ThrowsArgumentNullException()
        {
            StringBuilder sb = null;
            CSVOptions options = new CSVOptions();
            var ex = Assert.Throws<ArgumentNullException>(() => sb.AppendCSV(options));
            Assert.AreEqual("builder", ex.ParamName);
        }

        [Test]
        public void AppendCSV_ForNullOptions_ThrowsArgumentNullException()
        {
            var sb = new StringBuilder();
            CSVOptions options = null;
            var ex = Assert.Throws<ArgumentNullException>(() => sb.AppendCSV(options));
            Assert.AreEqual("options", ex.ParamName);
        }

        [Test]
        public void AppendCSV_ForNullArguments_ReturnsEmptyString()
        {
            var sb = new StringBuilder();
            var options = new CSVOptions();
            sb.AppendCSV(options);
            Assert.AreEqual("", sb.ToString());
        }

        [Test]
        public void AppendCSV_ForEmptyArguments_ReturnsEmptyString()
        {
            var sb = new StringBuilder();
            var options = new CSVOptions();
            var args = new object[] { };
            sb.AppendCSV(options, args);
            Assert.AreEqual("", sb.ToString());
        }

        [Test]
        public void AppendCSV_WhenCallIsValid_ReturnsSameBuilder()
        {
            var sb = new StringBuilder();
            var options = new CSVOptions();
            var args = new object[] { };
            var sb2 = sb.AppendCSV(options, args);
            Assert.AreSame(sb, sb2);
        }

        [Test]
        public void AppendCSV_WithNoArguments_ReturnsEmptyString()
        {
            // Separate test, because it seems to confuse NUnit if mixed in with AppendCSV_ForVariousInputs_ReturnsExpectedResult.
            var sb = new StringBuilder();
            var options = new CSVOptions();
            sb.AppendCSV(options);
            Assert.AreSame("", sb.ToString());

            sb = new StringBuilder();
            sb.AppendCSV(CSVOptions.CrunchingOptions);
            Assert.AreSame("", sb.ToString());
        }

        const string NTERM = null;
        const string A = "a";
        const string AWRP = "\"a\"";
        const string NWRP = "\"\"";
        const string B = "b";
        const string BWRP = "\"b\"";

        [TestCase(NWRP, NTERM)]
        [TestCase(NWRP, "")]
        [TestCase(AWRP, A)]
        public void AppendCSV_ForOneArgumentWithDefaultOptions_ReturnsFullResults(string expected, string term)
        {
            var sb = new StringBuilder();
            var options = new CSVOptions();
            sb.AppendCSV(options, term);
            Assert.AreEqual(expected, sb.ToString());
        }

        [TestCase("", NTERM)]
        [TestCase("", "")]
        [TestCase(A, A)]
        public void AppendCSV_ForOneArgumentWithCrunchingOptions_ReturnsCrunchedResults(string expected, string term)
        {
            var sb = new StringBuilder();
            sb.AppendCSV(CSVOptions.CrunchingOptions, term);
            Assert.AreEqual(expected, sb.ToString());
        }

        [TestCase(NWRP + "," + NWRP, NTERM, NTERM)]
        [TestCase(NWRP + "," + NWRP, NTERM, "")]
        [TestCase(NWRP + "," + NWRP, "", NTERM)]
        [TestCase(NWRP + "," + NWRP, "", "")]
        [TestCase(AWRP + "," + AWRP, A, A)]
        [TestCase(AWRP + "," + NWRP, A, "")]
        [TestCase(NWRP + "," + AWRP, "", A)]
        public void AppendCSV_ForTwoArgumentsWithDefaultOptions_ReturnsFullResults(string expected, string term1, string term2)
        {
            var sb = new StringBuilder();
            var options = new CSVOptions();
            sb.AppendCSV(options, term1, term2);
            Assert.AreEqual(expected, sb.ToString());
        }

        [TestCase("", NTERM, NTERM)]
        [TestCase("", NTERM, "")]
        [TestCase("", "", NTERM)]
        [TestCase("", "", "")]
        [TestCase(A + "," + A, A, A)]
        [TestCase(A, A, "")]
        [TestCase(A, "", A)]
        public void AppendCSV_ForTwoArgumentsWithCrunchingOptions_ReturnsCrunchedResults(string expected, string term1, string term2)
        {
            var sb = new StringBuilder();
            sb.AppendCSV(CSVOptions.CrunchingOptions, term1, term2);
            Assert.AreEqual(expected, sb.ToString());
        }

        [TestCase(NWRP + "," + NWRP + "," + NWRP, "", "", "")]
        public void AppendCSV_ForThreeArgumentsWithDefaultOptions_ReturnsFullResults(string expected, string term1, string term2, string term3)
        {
            var sb = new StringBuilder();
            var options = new CSVOptions();
            sb.AppendCSV(options, term1, term2, term3);
            Assert.AreEqual(expected, sb.ToString());
        }

        [TestCase("", "", "", "")]
        public void AppendCSV_ForThreeArgumentsWithCrunchingOptions_CrunchesAllToEmptyString(string expected, string term1, string term2, string term3)
        {
            var sb = new StringBuilder();
            sb.AppendCSV(CSVOptions.CrunchingOptions, term1, term2, term3);
            Assert.AreEqual(expected, sb.ToString());
        }

        [Test]
        public void AppendCSV_IfDelimiterAppearsInTerm_DoublesDelimiter()
        {
            var sb = new StringBuilder();
            string term = "hello\"world";
            string expected = "\"" + term.Replace("\"", "\"\"") + "\"";
            sb.AppendCSV(term);
            Assert.AreEqual(expected, sb.ToString());
        }

        [Test]
        public void AppendCSV_ForNullDelimiterAndSeparator_JustAppendsTerms()
        {
            var sb = new StringBuilder();
            var options = new CSVOptions() { Separator = null, Delimiter = null };
            sb.AppendCSV(options, "a", "b", "c");
            Assert.AreEqual("abc", sb.ToString());

        }

        [Test]
        public void AppendCSV_WhenUsedAgainstNonEmptyBuilder_PreservesTheInitialData()
        {
            // Most of the existing tests above are based on an initially empty builder,
            // although there are a few mixed in where the builder is non-empty.
            // Let's be explicit about this case.
            var sb = new StringBuilder("hello");
            sb.AppendCSV("world");
            Assert.AreEqual("hello,\"world\"", sb.ToString());
        }
    }
}
