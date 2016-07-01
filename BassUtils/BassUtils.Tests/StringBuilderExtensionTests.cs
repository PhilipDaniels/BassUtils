using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace BassUtils.Tests
{
    [TestFixture]
    public class StringBuilderExtensionTests
    {
        [Test]
        public void EndsWithChar_ForNullBuilder_ThrowsArgumentNullException()
        {
            Action act = () => StringBuilderExtensions.EndsWith(null, 'c');

            act.ShouldThrow<ArgumentNullException>()
                .WithMessage("*builder*")
                .And.ParamName.Should().Be("builder");
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
            Assert.IsTrue(sb.EndsWith('b'));
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
            var options = new CSVOptions();
            Action act = () => StringBuilderExtensions.AppendCSV(null, options);

            act.ShouldThrow<ArgumentNullException>()
                .WithMessage("*builder*")
                .And.ParamName.Should().Be("builder");
        }

        [Test]
        public void AppendCSV_ForNullOptions_ThrowsArgumentNullException()
        {
            var sb = new StringBuilder();
            CSVOptions options = null;
            Action act = () => StringBuilderExtensions.AppendCSV(sb, options);

            act.ShouldThrow<ArgumentNullException>()
                .WithMessage("*options*")
                .And.ParamName.Should().Be("options");
        }

        [Test]
        public void AppendCSV_ForDefaultOptions_ActuallyDoesAppend()
        {
            var sb = new StringBuilder();

            sb.AppendCSV("hello", "world").ToString().Should().Be("hello,world");
        }

        [Test]
        public void AppendCSV_ForExplicitOptions_ActuallyDoesAppend()
        {
            var sb = new StringBuilder();

            sb.AppendCSV(CSVOptions.HumanReadableWithSpace, "hello", "world").ToString().Should().Be("hello, world");
        }

        [Test]
        public void AppendCSV_WhenCallIsValid_ReturnsSameBuilder()
        {
            var sb = new StringBuilder();

            var result = sb.AppendCSV("hello", "world");
            result.Should().BeSameAs(sb);
        }
    }
}
