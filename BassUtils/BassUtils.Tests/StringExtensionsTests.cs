﻿using System;
using System.Globalization;
using FluentAssertions;
using NUnit.Framework;

namespace BassUtils.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void SetChar_WhenValueIsNull_ThrowsArgumentNullException()
        {
            string s = null;
            var ex = Assert.Throws<ArgumentNullException>(() => s.SetChar(0, 'a'));
            Assert.AreEqual("value", ex.ParamName);
        }

        [Test]
        public void SetChar_WhenIndexIsLessThanZero_ThrowsArgumentOutOfRangeException()
        {
            string s = "hello";
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => s.SetChar(-1, 'a'));
            Assert.AreEqual("index", ex.ParamName);
        }

        [Test]
        public void SetChar_WhenIndexIsPastTheEndOfTheString_ThrowsArgumentOutOfRangeException()
        {
            string s = "hello";
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => s.SetChar(5, 'a'));
            Assert.AreEqual("index", ex.ParamName);
        }

        [Test]
        public void SetChar_WhenIndexIsOK_ReplacesCharacterCorrectly()
        {
            string s = "hello";
            s = s.SetChar(0, 'a');
            s = s.SetChar(4, 'z');
            Assert.AreEqual("aellz", s);
        }

        [Test]
        public void AppendIfDoesNotEndWithChar_ForNullString_ThrowsArgumentNullException()
        {
            string s = null;
            var ex = Assert.Throws<ArgumentNullException>(() => s.AppendIfDoesNotEndWith(','));
            Assert.AreEqual("value", ex.ParamName);
        }

        [Test]
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

        [Test]
        public void AppendIfDoesNotEndWithString_ForNullString_ThrowsArgumentNullException()
        {
            string s = null;
            var ex = Assert.Throws<ArgumentNullException>(() => s.AppendIfDoesNotEndWith(""));
            Assert.AreEqual("value", ex.ParamName);
        }

        [Test]
        public void AppendIfDoesNotEndWithString_ForNullValueToAppend_ThrowsArgumentNullException()
        {
            string s = "";
            var ex = Assert.Throws<ArgumentNullException>(() => s.AppendIfDoesNotEndWith(null));
            Assert.AreEqual("valueToAppend", ex.ParamName);
        }

        [Test]
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

        [Test]
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

        [Test]
        public void TrimAppend_ForNullValue_ThrowsArgumentNullException()
        {
            string s = null;
            var ex = Assert.Throws<ArgumentNullException>(() => s.TrimAppend(" "));
            Assert.AreEqual("value", ex.ParamName);
        }

        [Test]
        public void TrimAppend_ForNullValueToAppend_ThrowsArgumentNullException()
        {
            string s = "";
            var ex = Assert.Throws<ArgumentNullException>(() => s.TrimAppend(null));
            Assert.AreEqual("valueToAppend", ex.ParamName);
        }

        // For AppendCSV, most of the tests are in the StringBuilderTests class.
        // We can get away with basic testing for the string variants.

        [Test]
        public void AppendCSV_ForNullValue_ThrowsArgumentNullException()
        {
            string s = null;

            Action act = () => StringExtensions.AppendCSV(s, "");

            act.ShouldThrow<ArgumentNullException>()
                    .WithMessage("*value*")
                    .And.ParamName.Should().Be("value");
        }

        [Test]
        public void AppendCSV_ForNullOptions_ThrowsArgumentNullException()
        {
            string s = "";
            CSVOptions options = null;
            var ex = Assert.Throws<ArgumentNullException>(() => s.AppendCSV(options, ""));
            Assert.AreEqual("options", ex.ParamName);
        }

        [Test]
        public void AppendCSV_AgainstEmptyString_DoesNotWriteLeadingSeparator()
        {
            string s = "";
            string result = s.AppendCSV("world");
            Assert.AreEqual("world", result);
        }

        [Test]
        public void AppendCSV_ForNonEmptyString_PreservesOriginalStringAndWritesInitialSeparator()
        {
            string s = "hello";
            string result = s.AppendCSV("world");
            Assert.AreEqual("hello,world", result);
        }

        [Test]
        public void SafeFormat_ForNullFormat_ReturnsNull()
        {
            string s = null;
            string result = s.SafeFormat(CultureInfo.InvariantCulture, 42);
            Assert.IsNull(result);
        }

        [Test]
        public void SafeFormat_ForNullArgs_ReturnsFormatString()
        {
            string s = "hello";
            object[] args = null;
            string result = s.SafeFormat(CultureInfo.InvariantCulture, args);
            Assert.AreEqual(s, result);
        }

        [Test]
        public void SafeFormat_ForEmptyArgs_ReturnsFormatString()
        {
            string s = "hello";
            object[] args = new object[] { };
            string result = s.SafeFormat(CultureInfo.InvariantCulture, args);
            Assert.AreEqual(s, result);
        }

        [Test]
        public void SafeFormat_ForValidFormatAndArgs_ReturnsFormattedExpression()
        {
            string s = "hello {0} again";
            string result = s.SafeFormat(CultureInfo.InvariantCulture, "world");
            Assert.AreEqual("hello world again", result);
        }

        [Test]
        public void SafeFormat_ForInvalidFormatAndArgs_ReturnsFormatString()
        {
            string s = "hello {0} again {1}";
            string result = s.SafeFormat(CultureInfo.InvariantCulture, "world");
            Assert.AreEqual(s, result);
        }

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase("ab ", "ab ")]
        [TestCase("a b", "a b")]
        [TestCase(" ab", " ab")]
        [TestCase("a\nb", "ab")]
        [TestCase("\nab", "ab")]
        [TestCase("ab\n", "ab")]
        [TestCase("a\rb", "ab")]
        [TestCase("\rab", "ab")]
        [TestCase("ab\r", "ab")]
        [TestCase("a\r\nb", "a b")]
        [TestCase("\r\nab", " ab")]
        [TestCase("ab\r\n", "ab ")]
        public void RemoveNewLines_RemovesCorrectly(string input, string expected)
        {
            string result = StringExtensions.RemoveNewLines(input);
            Assert.AreEqual(expected, result);
        }

        [TestCase(null, false)]
        [TestCase("a", true)]
        [TestCase("a", true)]
        [TestCase("abc", true, "a", "b")]
        public void ContainsAll_ReturnsCorrectResult(string input, bool expected, params string[] words)
        {
            bool result = StringExtensions.ContainsAll(input, words);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ToProperCase_ForNullValue_ThrowsArgumentNullException()
        {
            Action act = () => StringExtensions.ToProperCase(null, CultureInfo.InvariantCulture);

            act.ShouldThrow<ArgumentNullException>()
                .WithMessage("*value*")
                .And.ParamName.Should().Be("value");
        }

        [Test]
        public void ToProperCase_ForEmptyString_ReturnsEmptyString()
        {
            string.Empty.ToProperCase(CultureInfo.InvariantCulture).Should().BeEmpty();
        }

        [Test]
        public void ToProperCase_ForWhitespaceString_ReturnsOriginalString()
        {
            const string input = " \r\n";
            input.ToProperCase(CultureInfo.InvariantCulture).Should().Be(input);
        }

        [Test]
        public void ToProperCase_ForVariousInputs_ReturnsExpectedResults()
        {
            "a".ToProperCase(CultureInfo.InvariantCulture).Should().Be("A");
            "abc".ToProperCase(CultureInfo.InvariantCulture).Should().Be("Abc");
            "a b".ToProperCase(CultureInfo.InvariantCulture).Should().Be("A B");
            "Hello world".ToProperCase(CultureInfo.InvariantCulture).Should().Be("Hello World");
        }

        [Test]
        public void SplitCamelCaseIntoWords_ForNullValue_ThrowsArgumentNullException()
        {
            Action act = () => StringExtensions.SplitCamelCaseIntoWords(null);

            act.ShouldThrow<ArgumentNullException>()
                .WithMessage("*value*")
                .And.ParamName.Should().Be("value");
        }

        [Test]
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
