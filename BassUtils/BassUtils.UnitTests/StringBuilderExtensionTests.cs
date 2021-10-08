using System;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BassUtils.UnitTests
{
    [TestClass]
    public class StringBuilderExtensionTests
    {
        [TestMethod]
        public void EndsWithChar_ForNullBuilder_ThrowsArgumentNullException()
        {
            Action act = () => StringBuilderExtensions.EndsWith(null, 'c');

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*builder*")
                .And.ParamName.Should().Be("builder");
        }

        [TestMethod]
        public void EndsWithChar_ForEmptyBuilder_ReturnsFalse()
        {
            var sb = new StringBuilder();
            Assert.IsFalse(sb.EndsWith(','));
            Assert.IsFalse(sb.EndsWith(' '));
        }

        [TestMethod]
        public void EndsWithChar_ForStringsEndingWith_ReturnsTrue()
        {
            var sb = new StringBuilder();
            sb.Append("a");
            Assert.IsTrue(sb.EndsWith('a'));
            sb.Append("b");
            Assert.IsTrue(sb.EndsWith('b'));
        }

        [TestMethod]
        public void EndsWithChar_ForStringsNotEndingWith_ReturnsFalse()
        {
            var sb = new StringBuilder();
            sb.Append("a");
            Assert.IsFalse(sb.EndsWith('x'));
            sb.Append("b");
            Assert.IsTrue(sb.EndsWith('b'));
        }

        [TestMethod]
        public void EndsWithString_ForEmptyBuilder_ReturnsFalse()
        {
            var sb = new StringBuilder();
            Assert.IsFalse(sb.EndsWith("a"));
            Assert.IsFalse(sb.EndsWith(" "));
            Assert.IsFalse(sb.EndsWith(""));
        }

        [TestMethod]
        public void EndsWithString_ForStringsEndingWith_ReturnsTrue()
        {
            var sb = new StringBuilder();
            sb.Append("a");
            Assert.IsTrue(sb.EndsWith("a"));
            sb.Append("b");
            Assert.IsTrue(sb.EndsWith("b"));
            Assert.IsTrue(sb.EndsWith("ab"));
        }

        [TestMethod]
        public void EndsWithString_ForStringsNotEndingWith_ReturnsFalse()
        {
            var sb = new StringBuilder();
            sb.Append("a");
            Assert.IsFalse(sb.EndsWith("x"));
            sb.Append("b");
            Assert.IsFalse(sb.EndsWith('x'));
        }

        [TestMethod]
        public void EndsWithString_ForStringsLongerThanBuilder_ReturnsFalse()
        {
            var sb = new StringBuilder();
            sb.Append("a");
            Assert.IsFalse(sb.EndsWith("abc"));
        }

        [TestMethod]
        public void EndsWithString_AppliesStringComparisonCorrectly()
        {
            var sb = new StringBuilder();
            sb.Append("a");
            Assert.IsFalse(sb.EndsWith("A"));
            Assert.IsTrue(sb.EndsWith("A", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void AppendIfDoesNotEndWithChar_ForNullBuilder_ThrowsArgumentNullException()
        {
            StringBuilder sb = null;
            var ex = Assert.ThrowsException<ArgumentNullException>(() => sb.AppendIfDoesNotEndWith(','));
            Assert.AreEqual("builder", ex.ParamName);
        }

        [TestMethod]
        public void AppendIfDoesNotEndWithChar_AfterAppending_ReturnsOriginalBuilder()
        {
            var sb = new StringBuilder();
            var sb2 = sb.AppendIfDoesNotEndWith(',');
            Assert.AreSame(sb, sb2);
        }

        [TestMethod]
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

        [TestMethod]
        public void AppendIfDoesNotEndWithString_ForNullBuilder_ThrowsArgumentNullException()
        {
            StringBuilder sb = null;
            var ex = Assert.ThrowsException<ArgumentNullException>(() => sb.AppendIfDoesNotEndWith(" "));
            Assert.AreEqual("builder", ex.ParamName);
        }

        [TestMethod]
        public void AppendIfDoesNotEndWithString_AfterAppending_ReturnsOriginalBuilder()
        {
            var sb = new StringBuilder();
            var sb2 = sb.AppendIfDoesNotEndWith(" ");
            Assert.AreSame(sb, sb2);
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void TrimAppend_ForNullBuilder_ThrowsArgumentNullException()
        {
            StringBuilder sb = null;
            var ex = Assert.ThrowsException<ArgumentNullException>(() => sb.TrimAppend(" "));
            Assert.AreEqual("builder", ex.ParamName);
        }

        [TestMethod]
        public void TrimAppend_ForNullString_ThrowsArgumentNullException()
        {
            var sb = new StringBuilder();
            var ex = Assert.ThrowsException<ArgumentNullException>(() => sb.TrimAppend(null));
            Assert.AreEqual("value", ex.ParamName);
        }

        [TestMethod]
        public void TrimAppend_AfterAppending_ReturnsOriginalBuilder()
        {
            var sb = new StringBuilder();
            var sb2 = sb.TrimAppend("");
            Assert.AreSame(sb, sb2);
        }

        [TestMethod]
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

        [TestMethod]
        public void AppendCSV_ForNullBuilder_ThrowsArgumentNullException()
        {
            var options = new CsvOptions();
            Action act = () => StringBuilderExtensions.AppendCsv(null, options);

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*builder*")
                .And.ParamName.Should().Be("builder");
        }

        [TestMethod]
        public void AppendCSV_ForNullOptions_ThrowsArgumentNullException()
        {
            var sb = new StringBuilder();
            CsvOptions options = null;
            Action act = () => StringBuilderExtensions.AppendCsv(sb, options);

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*options*")
                .And.ParamName.Should().Be("options");
        }

        [TestMethod]
        public void AppendCSV_ForDefaultOptions_ActuallyDoesAppend()
        {
            var sb = new StringBuilder();

            sb.AppendCsv("hello", "world").ToString().Should().Be("hello,world");
        }

        [TestMethod]
        public void AppendCSV_ForExplicitOptions_ActuallyDoesAppend()
        {
            var sb = new StringBuilder();

            sb.AppendCsv(CsvOptions.HumanReadableWithSpace, "hello", "world").ToString().Should().Be("hello, world");
        }

        [TestMethod]
        public void AppendCSV_WhenCallIsValid_ReturnsSameBuilder()
        {
            var sb = new StringBuilder();

            var result = sb.AppendCsv("hello", "world");
            result.Should().BeSameAs(sb);
        }
    }
}
