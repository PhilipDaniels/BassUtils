using System;
using System.Globalization;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BassUtils.UnitTests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void SetChar_WhenValueIsNull_ThrowsArgumentNullException()
        {
            string s = null;
            var ex = Assert.ThrowsException<ArgumentNullException>(() => s.SetChar(0, 'a'));
            Assert.AreEqual("value", ex.ParamName);
        }

        [TestMethod]
        public void SetChar_WhenIndexIsLessThanZero_ThrowsArgumentOutOfRangeException()
        {
            string s = "hello";
            var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() => s.SetChar(-1, 'a'));
            Assert.AreEqual("index", ex.ParamName);
        }

        [TestMethod]
        public void SetChar_WhenIndexIsPastTheEndOfTheString_ThrowsArgumentOutOfRangeException()
        {
            string s = "hello";
            var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() => s.SetChar(5, 'a'));
            Assert.AreEqual("index", ex.ParamName);
        }

        [TestMethod]
        public void SetChar_WhenIndexIsOK_ReplacesCharacterCorrectly()
        {
            string s = "hello";
            s = s.SetChar(0, 'a');
            s = s.SetChar(4, 'z');
            Assert.AreEqual("aellz", s);
        }

        [TestMethod]
        public void AppendIfDoesNotEndWithChar_ForNullString_ThrowsArgumentNullException()
        {
            string s = null;
            var ex = Assert.ThrowsException<ArgumentNullException>(() => s.AppendIfDoesNotEndWith(','));
            Assert.AreEqual("value", ex.ParamName);
        }

        [TestMethod]
        public void AppendIfDoesNotEndWithChar_AppendsOnlyWhenNeeded()
        {
            string s = "";
            s = s.AppendIfDoesNotEndWith('a');
            Assert.AreEqual("a", s);
            s = s.AppendIfDoesNotEndWith('a');
            Assert.AreEqual("a", s);
            s = s.AppendIfDoesNotEndWith('b');
            Assert.AreEqual("ab", s);
        }

        [TestMethod]
        public void AppendIfDoesNotEndWithString_ForNullString_ThrowsArgumentNullException()
        {
            string s = null;
            var ex = Assert.ThrowsException<ArgumentNullException>(() => s.AppendIfDoesNotEndWith(""));
            Assert.AreEqual("value", ex.ParamName);
        }

        [TestMethod]
        public void AppendIfDoesNotEndWithString_ForNullValueToAppend_ThrowsArgumentNullException()
        {
            string s = "";
            var ex = Assert.ThrowsException<ArgumentNullException>(() => s.AppendIfDoesNotEndWith(null));
            Assert.AreEqual("valueToAppend", ex.ParamName);
        }

        [TestMethod]
        public void AppendIfDoesNotEndWithString_AppendsOnlyWhenNeeded()
        {
            string s = "";
            s = s.AppendIfDoesNotEndWith("a");
            Assert.AreEqual("a", s);
            s = s.AppendIfDoesNotEndWith("a");
            Assert.AreEqual("a", s);
            s = s.AppendIfDoesNotEndWith("b");
            Assert.AreEqual("ab", s);
            s = s.AppendIfDoesNotEndWith("ab");
            Assert.AreEqual("ab", s);
        }

        [TestMethod]
        public void AppendIfDoesNotEndWithString_AppliesStringComparisonCorrectly()
        {
            string s = "";
            s = s.AppendIfDoesNotEndWith("a");
            Assert.AreEqual("a", s);
            s = s.AppendIfDoesNotEndWith("a");
            Assert.AreEqual("a", s);
            s = s.AppendIfDoesNotEndWith("A");
            Assert.AreEqual("aA", s);
            s = s.AppendIfDoesNotEndWith("a", StringComparison.OrdinalIgnoreCase);
            Assert.AreEqual("aA", s);
        }

        [TestMethod]
        public void TrimAppend_ForNullValue_ThrowsArgumentNullException()
        {
            string s = null;
            var ex = Assert.ThrowsException<ArgumentNullException>(() => s.TrimAppend(" "));
            Assert.AreEqual("value", ex.ParamName);
        }

        [TestMethod]
        public void TrimAppend_ForNullValueToAppend_ThrowsArgumentNullException()
        {
            string s = "";
            var ex = Assert.ThrowsException<ArgumentNullException>(() => s.TrimAppend(null));
            Assert.AreEqual("valueToAppend", ex.ParamName);
        }

        // For AppendCSV, most of the tests are in the StringBuilderTests class.
        // We can get away with basic testing for the string variants.

        [TestMethod]
        public void AppendCSV_ForNullValue_ThrowsArgumentNullException()
        {
            string s = null;

            Action act = () => StringExtensions.AppendCsv(s, "");

            act.Should().Throw<ArgumentNullException>()
                    .WithMessage("*value*")
                    .And.ParamName.Should().Be("value");
        }

        [TestMethod]
        public void AppendCSV_ForNullOptions_ThrowsArgumentNullException()
        {
            string s = "";
            CsvOptions options = null;
            var ex = Assert.ThrowsException<ArgumentNullException>(() => s.AppendCsv(options, ""));
            Assert.AreEqual("options", ex.ParamName);
        }

        [TestMethod]
        public void AppendCSV_AgainstEmptyString_DoesNotWriteLeadingSeparator()
        {
            string s = "";
            string result = s.AppendCsv("world");
            Assert.AreEqual("world", result);
        }

        [TestMethod]
        public void AppendCSV_ForNonEmptyString_PreservesOriginalStringAndWritesInitialSeparator()
        {
            string s = "hello";
            string result = s.AppendCsv("world");
            Assert.AreEqual("hello,world", result);
        }

        [TestMethod]
        public void SafeFormat_ForNullFormat_ReturnsNull()
        {
            string s = null;
            string result = s.SafeFormat(CultureInfo.InvariantCulture, 42);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void SafeFormat_ForNullArgs_ReturnsFormatString()
        {
            string s = "hello";
            object[] args = null;
            string result = s.SafeFormat(CultureInfo.InvariantCulture, args);
            Assert.AreEqual(s, result);
        }

        [TestMethod]
        public void SafeFormat_ForEmptyArgs_ReturnsFormatString()
        {
            string s = "hello";
            object[] args = new object[] { };
            string result = s.SafeFormat(CultureInfo.InvariantCulture, args);
            Assert.AreEqual(s, result);
        }

        [TestMethod]
        public void SafeFormat_ForValidFormatAndArgs_ReturnsFormattedExpression()
        {
            string s = "hello {0} again";
            string result = s.SafeFormat(CultureInfo.InvariantCulture, "world");
            Assert.AreEqual("hello world again", result);
        }

        [TestMethod]
        public void SafeFormat_ForInvalidFormatAndArgs_ReturnsFormatString()
        {
            string s = "hello {0} again {1}";
            string result = s.SafeFormat(CultureInfo.InvariantCulture, "world");
            Assert.AreEqual(s, result);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", "")]
        [DataRow(" ", " ")]
        [DataRow("ab ", "ab ")]
        [DataRow("a b", "a b")]
        [DataRow(" ab", " ab")]
        [DataRow("a\nb", "ab")]
        [DataRow("\nab", "ab")]
        [DataRow("ab\n", "ab")]
        [DataRow("a\rb", "ab")]
        [DataRow("\rab", "ab")]
        [DataRow("ab\r", "ab")]
        [DataRow("a\r\nb", "a b")]
        [DataRow("\r\nab", " ab")]
        [DataRow("ab\r\n", "ab ")]
        public void RemoveNewLines_RemovesCorrectly(string input, string expected)
        {
            string result = StringExtensions.RemoveNewLines(input);
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("a", true)]
        [DataRow("a", true)]
        [DataRow("abc", true, "a", "b")]
        public void ContainsAll_ReturnsCorrectResult(string input, bool expected, params string[] words)
        {
            bool result = StringExtensions.ContainsAll(input, words);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ToProperCase_ForNullValue_ThrowsArgumentNullException()
        {
            Action act = () => StringExtensions.ToProperCase(null, CultureInfo.InvariantCulture);

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*value*")
                .And.ParamName.Should().Be("value");
        }

        [TestMethod]
        public void ToProperCase_ForEmptyString_ReturnsEmptyString()
        {
            string.Empty.ToProperCase(CultureInfo.InvariantCulture).Should().BeEmpty();
        }

        [TestMethod]
        public void ToProperCase_ForWhitespaceString_ReturnsOriginalString()
        {
            const string input = " \r\n";
            input.ToProperCase(CultureInfo.InvariantCulture).Should().Be(input);
        }

        [TestMethod]
        public void ToProperCase_ForVariousInputs_ReturnsExpectedResults()
        {
            "a".ToProperCase(CultureInfo.InvariantCulture).Should().Be("A");
            "abc".ToProperCase(CultureInfo.InvariantCulture).Should().Be("Abc");
            "a b".ToProperCase(CultureInfo.InvariantCulture).Should().Be("A B");
            "Hello world".ToProperCase(CultureInfo.InvariantCulture).Should().Be("Hello World");
        }

        [TestMethod]
        public void SplitCamelCaseIntoWords_ForNullValue_ThrowsArgumentNullException()
        {
            Action act = () => StringExtensions.SplitCamelCaseIntoWords(null);

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*value*")
                .And.ParamName.Should().Be("value");
        }

        [TestMethod]
        public void SplitCamelCaseIntoWords_ForVariousInputs_ReturnsExpectedResults()
        {
            string.Empty.SplitCamelCaseIntoWords().Should().Be(string.Empty);
            " ".SplitCamelCaseIntoWords().Should().Be(string.Empty, "Result is expected to be trimmed");
            "abc".SplitCamelCaseIntoWords().Should().Be("abc");
            "HelloWorld  ".SplitCamelCaseIntoWords().Should().Be("Hello World");
            "HelloWorld again ".SplitCamelCaseIntoWords().Should().Be("Hello World again");
            "TheQualityOfMercy".SplitCamelCaseIntoWords().Should().Be("The Quality Of Mercy");
        }

    }
}
