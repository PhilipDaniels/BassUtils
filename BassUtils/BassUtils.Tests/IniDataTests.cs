using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;

namespace BassUtils.Tests
{
    [TestFixture]
    public class IniDataTests
    {
        [Test]
        public void Parse_WhenIniDataIsNull_ThrowsArgumentNullException()
        {
            Action act = () => IniData.Parse(null, true, StringComparer.OrdinalIgnoreCase);

            act.ShouldThrow<ArgumentNullException>()
               .WithMessage("*iniData*")
               .And.ParamName.Should().Be("iniData");
        }

        [Test]
        public void Parse_WhenComparerIsNull_ThrowsArgumentNullException()
        {
            Action act = () => IniData.Parse(" ", true, null);

            act.ShouldThrow<ArgumentNullException>()
               .WithMessage("*stringComparer*")
               .And.ParamName.Should().Be("stringComparer");
        }

        [Test]
        public void Parse_ForEmptyIniData_CreatesNoSections()
        {
            var data = IniData.Parse(" ", false, StringComparer.OrdinalIgnoreCase);
            data.Sections.Should().BeEmpty();
        }

        [Test]
        public void Parse_ForCaseSensitiveComparer_CreatesSeparateSections()
        {
            var data = IniData.Parse("[foo]\n[Foo]", false, StringComparer.Ordinal);
            data.Sections.Select(s => s.Name).Should().BeEquivalentTo("Foo", "foo");
        }

        [Test]
        public void Parse_ForCaseInsensitiveComparer_OverwritesSections()
        {
            var data = IniData.Parse("[foo]\nkey=val\n[Foo]", false, StringComparer.OrdinalIgnoreCase);
            data.Sections.Select(s => s.Name).Should().BeEquivalentTo("Foo");
            data["Foo"].Count.Should().Be(0);
        }

        [Test]
        public void Indexer_ForEmptySectionName_ReturnsTopLevelSection()
        {
            var data = IniData.Parse("a=b\n[foo]\nkey=val\n[Bar]", false, StringComparer.OrdinalIgnoreCase);
            var sec = data[string.Empty];
            sec.Name.Should().BeEmpty("We expect to get an empty section.");
        }

        [Test]
        public void Indexer_ForSectionThatExists_ReturnsSection()
        {
            var data = IniData.Parse("a=b\n[foo]\nkey=val\n[Bar]", false, StringComparer.OrdinalIgnoreCase);
            var sec = data["foo"];
            sec.Name.Should().Be("foo", "We expect to get the 'foo' section.");
            sec.Keys.Should().BeEquivalentTo("key");
        }

        [Test]
        public void Indexer_ForSectionThatDoesNotExist_ThrowsKeyNotFoundException()
        {
            var data = IniData.Parse("a=b\n[foo]key=val\n[Bar]", false, StringComparer.OrdinalIgnoreCase);

            Action act = () => { var sec = data["notexist"]; };

            act.ShouldThrow<KeyNotFoundException>()
               .WithMessage("*section*notexist*");
        }

        [Test]
        public void GetValue_ForNullSection_ThrowsArgumentNullException()
        {
            var data = IniData.Parse(string.Empty, false, StringComparer.OrdinalIgnoreCase);

            Action act = () => { data.GetValue(null, string.Empty, "default"); };

            act.ShouldThrow<ArgumentNullException>()
                .WithMessage("*section*")
                .And.ParamName.Should().Be("section");
        }

        [Test]
        public void GetValue_ForNullKey_ThrowsArgumentNullException()
        {
            var data = IniData.Parse(string.Empty, false, StringComparer.OrdinalIgnoreCase);

            Action act = () => { data.GetValue(string.Empty, null, "default"); };

            act.ShouldThrow<ArgumentNullException>()
                .WithMessage("*key*")
                .And.ParamName.Should().Be("key");
        }

        [Test]
        public void GetValue_IfSectionNotFound_ReturnsDefault()
        {
            var data = IniData.Parse(string.Empty, false, StringComparer.OrdinalIgnoreCase);
            string val = data.GetValue("nosec", string.Empty, "default");
            val.Should().Be("default", "The section 'nosec' does not exist, so we should get back the default");
        }

        [Test]
        public void GetValue_IfKeyNotFound_ReturnsDefault()
        {
            var data = IniData.Parse("[Foo]\na=b", false, StringComparer.OrdinalIgnoreCase);
            string val = data.GetValue("Foo", "nokey", "default");
            val.Should().Be("default", "The key 'nokey' does not exist, so we should get back the default");
        }



        const string LargeInput =
@"
; TEST: Comments are ignored.

; TEST: Keys outside a section result in a section named '' being created.
rootkey1=rootval1
rootkey2=rootval2
rootkey3=some string with spaces
; TEST: Key names are case sensitive (if so configured).
Rootkey3=another string with spaces

[Sec1]
; TEST: You can have keys without values.
a
b

; TEST: Keys in this section are completely separate to those in Sec1.
[Sec2]
a=1
b=4


; TEST: Section names are case sensitive (if so configured).
[sec2]
foo=bar
; TEST: There are tabs on the next line which should be trimmed.
Foo =  bartrim      


; TEST: Section names get trimmed.
[   padded section name   ]
; TEST: Should split on the first index.
multi=fruit=apple,tree=oak
; TEST: Double quotes are preserved in strings.
quux=""some text in quotes""
";


        [Test]
        public void Parse_ForCaseSensitiveComparer_ParsesCorrectly()
        {
            var data = IniData.Parse(LargeInput, false, StringComparer.Ordinal);
            data.Sections.Select(s => s.Name).Should().BeEquivalentTo(string.Empty, "Sec1", "Sec2", "sec2", "padded section name");

            var sec = data[string.Empty];
            sec.Keys.Should().BeEquivalentTo("rootkey1", "rootkey2", "rootkey3", "Rootkey3");
            sec["rootkey1"].Should().Be("rootval1");
            sec["rootkey2"].Should().Be("rootval2");
            sec["rootkey3"].Should().Be("some string with spaces");
            sec["Rootkey3"].Should().Be("another string with spaces");

            sec = data["Sec1"];
            sec.Keys.Should().BeEquivalentTo("a", "b");
            sec["a"].Should().BeEmpty();
            sec["b"].Should().BeEmpty();

            sec = data["Sec2"];
            sec.Keys.Should().BeEquivalentTo("a", "b");
            sec["a"].Should().Be("1");
            sec["b"].Should().Be("4");

            sec = data["sec2"];
            sec.Keys.Should().BeEquivalentTo("foo", "Foo");
            sec["foo"].Should().Be("bar");
            sec["Foo"].Should().Be("bartrim");

            sec = data["padded section name"];
            sec.Keys.Should().BeEquivalentTo("multi", "quux");
            sec["multi"].Should().Be("fruit=apple,tree=oak");
            sec["quux"].Should().Be(@"""some text in quotes""");
        }

        [Test]
        public void Ctor_ForCaseInsensitiveComparer_ParsesCorrectly()
        {
            var data = IniData.Parse(LargeInput, false, StringComparer.OrdinalIgnoreCase);
            data.Sections.Select(s => s.Name).Should().BeEquivalentTo(string.Empty, "Sec1", "sec2", "padded section name");

            var sec = data[string.Empty];
            sec.Keys.Should().BeEquivalentTo("rootkey1", "rootkey2", "rootkey3");
            sec["rootkey1"].Should().Be("rootval1");
            sec["rootkey2"].Should().Be("rootval2");
            sec["Rootkey3"].Should().Be("another string with spaces");

            sec = data["Sec1"];
            sec.Keys.Should().BeEquivalentTo("a", "b");
            sec["a"].Should().BeEmpty();
            sec["b"].Should().BeEmpty();

            sec = data["sec2"];
            sec.Keys.Should().BeEquivalentTo("foo");
            sec["foo"].Should().Be("bartrim");

            sec = data["padded section name"];
            sec.Keys.Should().BeEquivalentTo("multi", "quux");
            sec["multi"].Should().Be("fruit=apple,tree=oak");
            sec["quux"].Should().Be(@"""some text in quotes""");
        }

        [Test]
        public void Ctor_WhenCoalesceIsTrue_CoalescesLines()
        {
            const string input =
@"key=some \
value \
on multiple lines";

            var data = IniData.Parse(input, true, StringComparer.OrdinalIgnoreCase);
            data.GetValue("key").Should().Be("some value on multiple lines");
        }

        [Test]
        public void Ctor_WhenCoalesceIsFalse_DoesNotCoalesceLines()
        {
            const string input =
@"key=some \
value \
on multiple lines";

            var data = IniData.Parse(input, false, StringComparer.OrdinalIgnoreCase);
            data.GetValue("key").Should().Be(@"some \");
            // Hmm, you can have multi-word keys.
            data[string.Empty].Keys.Should().BeEquivalentTo("key", @"value \", @"on multiple lines");
        }
    }
}
