using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BassUtils.UnitTests
{
    [TestClass]
    public class IniSectionTests
    {
        [TestMethod]
        public void Ctor_WhenNameIsNull_ThrowsArgumentNullException()
        {
            Action act = () => new IniSection(null, StringComparer.OrdinalIgnoreCase);
            Assert.ThrowsException<ArgumentNullException>(act);
        }

        [TestMethod]
        public void Ctor_WhenKeyComparerIsNull_ThrowsArgumentNullException()
        {
            Action act = () => new IniSection(string.Empty, null);
            Assert.ThrowsException<ArgumentNullException>(act);
        }

        [TestMethod]
        public void Ctor_WhenNameIsEmpty_CreatesSectionWithEmptyName()
        {
            var sec = new IniSection(string.Empty, StringComparer.OrdinalIgnoreCase);
            Assert.AreEqual(sec.Name, string.Empty);
        }

        [TestMethod]
        public void Ctor_WhenNameIsWhitespace_CreatesSectionWithEmptyName()
        {
            var sec = new IniSection(" ", StringComparer.OrdinalIgnoreCase);
            Assert.AreEqual(sec.Name, string.Empty);
        }

        [TestMethod]
        public void Ctor_WhenNameIsSomething_SetsNameProperty()
        {
            var sec = new IniSection("my name", StringComparer.OrdinalIgnoreCase);
            Assert.AreEqual(sec.Name, "my name");
        }

        [TestMethod]
        public void Ctor_WhenCalled_InitializesOtherPropertiesToEmptyState()
        {
            var sec = new IniSection(string.Empty, StringComparer.OrdinalIgnoreCase);
            Assert.IsFalse(sec.Keys.Any());
            Assert.IsFalse(sec.Values.Any());
            Assert.AreEqual(sec.Count, 0, "There should not be any items in the section after creation.");
            Assert.AreEqual(sec.Count(), 0, "Enumeration should not return any items.");
        }

        [TestMethod]
        public void Indexer_ForKeyThatExists_ReturnsValue()
        {
            var sec = new IniSection(string.Empty, StringComparer.Ordinal);
            sec["Foo"] = "value1";
            Assert.AreEqual(sec["Foo"], "value1");
        }

        [TestMethod]
        public void Indexer_ForKeyThatDoesNotExist_ThrowsKeyNotFoundException()
        {
            var sec = new IniSection("secName", StringComparer.OrdinalIgnoreCase);

            Action act = () => { string val = sec["notexist"]; };
            Assert.ThrowsException<KeyNotFoundException>(act);
        }

        [TestMethod]
        public void ContainsKey_ForCaseSensitiveComparer_DistinguishesCase()
        {
            var sec = new IniSection(string.Empty, StringComparer.Ordinal);
            sec["Foo"] = "value1";
            Assert.IsFalse(sec.ContainsKey("foo"), "Case is different, so key name should not match");
            Assert.IsTrue(sec.ContainsKey("Foo"));
        }

        [TestMethod]
        public void ContainsKey_ForCaseInsensitiveComparer_DoesNotDistinguishCase()
        {
            var sec = new IniSection(string.Empty, StringComparer.OrdinalIgnoreCase);
            sec["Foo"] = "value1";
            Assert.IsTrue(sec.ContainsKey("foo"), "Case is different, but comparer is case insensitve, so should find the key");
            Assert.IsTrue(sec.ContainsKey("Foo"));
        }

        [TestMethod]
        public void TryGetValue_ForCaseSensitiveComparer_DistinguishesCase()
        {
            var sec = new IniSection(string.Empty, StringComparer.Ordinal);
            sec["Foo"] = "value1";

            string v;
            Assert.IsTrue(sec.TryGetValue("Foo", out v));
            Assert.AreEqual(v, "value1");

            Assert.IsFalse(sec.TryGetValue("foo", out v));
        }

        [TestMethod]
        public void TryGetValue_ForCaseInsensitiveComparer_DoesNotDistinguishCase()
        {
            var sec = new IniSection(string.Empty, StringComparer.OrdinalIgnoreCase);
            sec["Foo"] = "value1";

            string v;
            Assert.IsTrue(sec.TryGetValue("Foo", out v));
            Assert.AreEqual(v, "value1");

            v = null;
            Assert.IsTrue(sec.TryGetValue("Foo", out v));
            Assert.AreEqual(v, "value1");
        }

        [TestMethod]
        public void Contains_ForCaseSensitiveComparer_DistinguishesCase()
        {
            var sec = new IniSection(string.Empty, StringComparer.Ordinal);
            sec["Foo"] = "value1";

            var pair = new KeyValuePair<string, string>("Foo", "value1");
            Assert.IsTrue(sec.Contains(pair));

            pair = new KeyValuePair<string, string>("foo", "value1");
            Assert.IsFalse(sec.Contains(pair));
        }

        [TestMethod]
        public void Contains_ForCaseInsensitiveComparer_DoesNotDistinguishCase()
        {
            var sec = new IniSection(string.Empty, StringComparer.OrdinalIgnoreCase);
            sec["Foo"] = "value1";

            var pair = new KeyValuePair<string, string>("Foo", "value1");
            Assert.IsTrue(sec.Contains(pair));

            pair = new KeyValuePair<string, string>("foo", "value1");
            Assert.IsTrue(sec.Contains(pair));
        }

        [TestMethod]
        public void Add_ForCaseSensitiveComparer_CreatesMultipleKeys()
        {
            var sec = new IniSection(string.Empty, StringComparer.Ordinal);
            sec["Foo"] = "value1";
            sec["foo"] = "value2";

            Assert.AreEqual(sec.Keys.Count(), 2);
            sec.Keys.Should().BeEquivalentTo("Foo", "foo");

            Assert.AreEqual(sec["Foo"], "value1");
            Assert.AreEqual(sec["foo"], "value2");
        }

        [TestMethod]
        public void Add_ForCaseInsensitiveComparer_LatestValueWins()
        {
            var sec = new IniSection(string.Empty, StringComparer.OrdinalIgnoreCase);
            sec["Foo"] = "value1";
            sec["foo"] = "value2";

            sec.Keys.Should().BeEquivalentTo("Foo");
            Assert.AreEqual(sec["foo"], "value2");
        }
    }
}
