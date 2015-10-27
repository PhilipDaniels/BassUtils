using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace BassUtils.Tests
{
    [TestFixture]
    public class IniSectionTests
    {
        [Test]
        public void Ctor_WhenNameIsNull_ThrowsArgumentNullException()
        {
            Action act = () => new IniSection(null, StringComparer.OrdinalIgnoreCase);

            act.ShouldThrow<ArgumentNullException>()
               .WithMessage("*name*")
               .And.ParamName.Should().Be("name");
        }

        [Test]
        public void Ctor_WhenKeyComparerIsNull_ThrowsArgumentNullException()
        {
            Action act = () => new IniSection(string.Empty, null);

            act.ShouldThrow<ArgumentNullException>()
               .WithMessage("*keyComparer*")
               .And.ParamName.Should().Be("keyComparer");
        }

        [Test]
        public void Ctor_WhenNameIsEmpty_CreatesSectionWithEmptyName()
        {
            var sec = new IniSection(string.Empty, StringComparer.OrdinalIgnoreCase);
            sec.Name.Should().BeEmpty();
        }

        [Test]
        public void Ctor_WhenNameIsWhitespace_CreatesSectionWithEmptyName()
        {
            var sec = new IniSection(" ", StringComparer.OrdinalIgnoreCase);
            sec.Name.Should().BeEmpty();
        }

        [Test]
        public void Ctor_WhenNameIsSomething_SetsNameProperty()
        {
            var sec = new IniSection("my name", StringComparer.OrdinalIgnoreCase);
            sec.Name.Should().Be("my name");
        }

        [Test]
        public void Ctor_WhenCalled_InitializesOtherPropertiesToEmptyState()
        {
            var sec = new IniSection(string.Empty, StringComparer.OrdinalIgnoreCase);
            sec.Keys.Should().BeEmpty();
            sec.Values.Should().BeEmpty();
            sec.Count.Should().Be(0, "There should not be any items in the section after creation.");
            sec.Count().Should().Be(0, "Enumeration should not return any items.");
        }

        [Test]
        public void Indexer_ForKeyThatExists_ReturnsValue()
        {
            var sec = new IniSection(string.Empty, StringComparer.Ordinal);
            sec["Foo"] = "value1";
            sec["Foo"].Should().Be("value1");
        }

        [Test]
        public void Indexer_ForKeyThatDoesNotExist_ThrowsKeyNotFoundException()
        {
            var sec = new IniSection("secName", StringComparer.OrdinalIgnoreCase);

            Action act = () => { string val = sec["notexist"]; };

            act.ShouldThrow<KeyNotFoundException>()
               .WithMessage("*section*secName*does not contain*notexist*");
        }

        [Test]
        public void ContainsKey_ForCaseSensitiveComparer_DistinguishesCase()
        {
            var sec = new IniSection(string.Empty, StringComparer.Ordinal);
            sec["Foo"] = "value1";
            sec.ContainsKey("foo").Should().BeFalse("Case is different, so key name should not match");
            sec.ContainsKey("Foo").Should().BeTrue();
        }

        [Test]
        public void ContainsKey_ForCaseInsensitiveComparer_DoesNotDistinguishCase()
        {
            var sec = new IniSection(string.Empty, StringComparer.OrdinalIgnoreCase);
            sec["Foo"] = "value1";
            sec.ContainsKey("foo").Should().BeTrue("Case is different, but comparer is case insensitve, so should find the key");
            sec.ContainsKey("Foo").Should().BeTrue();
        }

        [Test]
        public void TryGetValue_ForCaseSensitiveComparer_DistinguishesCase()
        {
            var sec = new IniSection(string.Empty, StringComparer.Ordinal);
            sec["Foo"] = "value1";

            string v;
            sec.TryGetValue("Foo", out v).Should().BeTrue();
            v.Should().Be("value1");

            sec.TryGetValue("foo", out v).Should().BeFalse();
        }

        [Test]
        public void TryGetValue_ForCaseInsensitiveComparer_DoesNotDistinguishCase()
        {
            var sec = new IniSection(string.Empty, StringComparer.OrdinalIgnoreCase);
            sec["Foo"] = "value1";

            string v;
            sec.TryGetValue("Foo", out v).Should().BeTrue();
            v.Should().Be("value1");

            v = null;
            sec.TryGetValue("foo", out v).Should().BeTrue();
            v.Should().Be("value1");
        }

        [Test]
        public void Contains_ForCaseSensitiveComparer_DistinguishesCase()
        {
            var sec = new IniSection(string.Empty, StringComparer.Ordinal);
            sec["Foo"] = "value1";

            var pair = new KeyValuePair<string, string>("Foo", "value1");
            sec.Contains(pair).Should().BeTrue();

            pair = new KeyValuePair<string, string>("foo", "value1");
            sec.Contains(pair).Should().BeFalse();
        }

        [Test]
        public void Contains_ForCaseInsensitiveComparer_DoesNotDistinguishCase()
        {
            var sec = new IniSection(string.Empty, StringComparer.OrdinalIgnoreCase);
            sec["Foo"] = "value1";

            var pair = new KeyValuePair<string, string>("Foo", "value1");
            sec.Contains(pair).Should().BeTrue();

            pair = new KeyValuePair<string, string>("foo", "value1");
            sec.Contains(pair).Should().BeTrue();
        }

        [Test]
        public void Add_ForCaseSensitiveComparer_CreatesMultipleKeys()
        {
            var sec = new IniSection(string.Empty, StringComparer.Ordinal);
            sec["Foo"] = "value1";
            sec["foo"] = "value2";
            sec.Keys.Should().BeEquivalentTo("Foo", "foo");
            sec["Foo"].Should().Be("value1");
            sec["foo"].Should().Be("value2");
        }

        [Test]
        public void Add_ForCaseInsensitiveComparer_LatestValueWins()
        {
            var sec = new IniSection(string.Empty, StringComparer.OrdinalIgnoreCase);
            sec["Foo"] = "value1";
            sec["foo"] = "value2";
            sec.Keys.Should().BeEquivalentTo("Foo");
            sec["foo"].Should().Be("value2");
        }
    }
}
