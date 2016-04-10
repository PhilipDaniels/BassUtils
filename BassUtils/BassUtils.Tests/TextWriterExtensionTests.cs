using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using FluentAssertions;

namespace BassUtils.Tests
{
    [TestFixture]
    public class TextWriterExtensionTests
    {
        [Test]
        public void AppendCSV_ForNullWriter_ThrowsArgumentNullException()
        {
            Action act = () => TextWriterExtensions.AppendCSV(null);

            act.ShouldThrow<ArgumentNullException>()
                .WithMessage("*writer*")
                .And.ParamName.Should().Be("writer");
        }

        [Test]
        public void AppendCSV_ForNullOptions_ThrowsArgumentNullException()
        {
            using (var sw = new StringWriter())
            {
                Action act = () => sw.AppendCSV(null, false);

                act.ShouldThrow<ArgumentNullException>()
                    .WithMessage("*options*")
                    .And.ParamName.Should().Be("options");
            }
        }

        [Test]
        public void AppendCSV_ForNullArgumentsAndAnyAppendInitial_ReturnsEmptyString()
        {
            using (var sw = new StringWriter())
            {
                var options = CSVOptions.Default.Clone();
                options.WriteLeadingSeparator = true;
                var result = sw.AppendCSV(options).ToString();
                result.Should().BeEmpty();
            }

            using (var sw = new StringWriter())
            {
                var options = CSVOptions.Default.Clone();
                options.WriteLeadingSeparator = false;
                var result = sw.AppendCSV(options).ToString();
                result.Should().BeEmpty();
            }
        }

        [Test]
        public void AppendCSV_ForEmptyArgumentsAndAnyAppendInitial_ReturnsEmptyString()
        {
            var args = new object[] { };

            using (var sw = new StringWriter())
            {
                var options = CSVOptions.Default.Clone();
                options.WriteLeadingSeparator = true;
                var result = sw.AppendCSV(options, args).ToString();
                result.Should().BeEmpty();
            }

            using (var sw = new StringWriter())
            {
                var options = CSVOptions.Default.Clone();
                options.WriteLeadingSeparator = false;
                var result = sw.AppendCSV(options, args).ToString();
                result.Should().BeEmpty();
            }
        }

        [Test]
        public void AppendCSV_WhenAlwaysWriteDelimiterIsTrueAndDelimiterIsNull_ThrowsArgumentException()
        {
            using (var sw = new StringWriter())
            {
                var options = new CSVOptions() { AlwaysWriteDelimiter = true, Delimiter = null };
                Action act = () => sw.AppendCSV(options);

                act.ShouldThrow<ArgumentException>()
                    .WithMessage("*AlwaysWriteDelimiter*Delimiter*non-null*")
                    .And.ParamName.Should().Be("options");
            }
        }

        [Test]
        public void AppendCSV_WhenCallIsValid_ReturnsSameWriter()
        {
            using (var sw = new StringWriter())
            {
                var writer = sw.AppendCSV();
                writer.Should().BeSameAs(sw);
            }
        }

        [Test]
        public void AppendCSV_WhenInitialSeparatorRequiredIsTrue_PrependsSeparator()
        {
            using (var sw = new StringWriter())
            {
                var options = CSVOptions.Default.Clone();
                options.WriteLeadingSeparator = true;
                var result = sw.AppendCSV(options, "a").ToString();
                result.Should().Be(",a");
            }
        }

        [Test]
        public void AppendCSV_WhenInitialSeparatorRequiredIsFalse_DoesNotPrependSeparator()
        {
            using (var sw = new StringWriter())
            {
                var options = CSVOptions.Default.Clone();
                options.WriteLeadingSeparator = false;
                var result = sw.AppendCSV(options, "a").ToString();
                result.Should().Be("a");
            }
        }

        [Test]
        public void AppendCSV_WhenSkipNullValuesIsTrue_SkipsOverNullArguments()
        {
            using (var sw = new StringWriter())
            {
                var options = new CSVOptions() { SkipNullValues = true };
                var result = sw.AppendCSV(options, "a", null, "b").ToString();
                result.Should().Be("a,b");
            }
        }

        [Test]
        public void AppendCSV_WhenSkipNullValuesIsFalse_WritesEmptyStringForNullArguments()
        {
            using (var sw = new StringWriter())
            {
                var options = new CSVOptions() { SkipNullValues = false };
                var result = sw.AppendCSV(options, "a", null, "b").ToString();
                result.Should().Be("a,,b");
            }
        }

        [Test]
        public void AppendCSV_WhenSkipEmptyValuesIsTrue_SkipsOverNullArguments()
        {
            using (var sw = new StringWriter())
            {
                var options = new CSVOptions() { SkipEmptyValues = true };
                var result = sw.AppendCSV(options, "a", "", "b").ToString();
                result.Should().Be("a,b");
            }
        }

        [Test]
        public void AppendCSV_WhenSkipEmptyValuesIsFalse_WritesEmptyStringForNullArguments()
        {
            using (var sw = new StringWriter())
            {
                var options = new CSVOptions() { SkipEmptyValues = false };
                var result = sw.AppendCSV(options, "a", "", "b").ToString();
                result.Should().Be("a,,b");
            }
        }

        [Test]
        public void AppendCSV_WhenTrimStringsIsFalse_IncludesWhitespaceInTheOutput()
        {
            using (var sw = new StringWriter())
            {
                var options = new CSVOptions() { TrimStrings = false };
                var result = sw.AppendCSV(options, "a ", " b ").ToString();
                result.Should().Be("a , b ");
            }
        }

        [Test]
        public void AppendCSV_WhenTrimStringsIsTrue_TrimsArgumentsBeforeAppending()
        {
            using (var sw = new StringWriter())
            {
                var options = new CSVOptions() { TrimStrings = true };
                var result = sw.AppendCSV(options, "a ", " b ").ToString();
                result.Should().Be("a,b");
            }
        }

        [Test]
        public void AppendCSV_WhenTrimStringsIsTrueAndSkipEmptyValuesIsTrue_SkipsWhitespaceStrings()
        {
            using (var sw = new StringWriter())
            {
                var options = new CSVOptions() { TrimStrings = true, SkipEmptyValues = true };
                var result = sw.AppendCSV(options, "a ", "  ", " b ").ToString();
                result.Should().Be("a,b");
            }
        }

        const string WordWithDoubleQuote = "O\"Malley";
        const string QuotedWordWithDoubleQuote = "\"O\"\"Malley\"";  // double and wrap.

        [Test]
        public void AppendCSV_WhenDelimiterIsNonNullAndAppearsInArg_DelimiterIsDoubledAndArgIsDelimited()
        {
            using (var sw = new StringWriter())
            {
                var result = sw.AppendCSV(WordWithDoubleQuote).ToString();
                result.Should().Be(QuotedWordWithDoubleQuote);
            }
        }

        [Test]
        public void AppendCSV_WhenDelimiterIsNonNullAndDoesNotAppearInArg_ResultIsNotDelimited()
        {
            using (var sw = new StringWriter())
            {
                var result = sw.AppendCSV("a").ToString();
                result.Should().Be("a");
            }
        }

        [Test]
        public void AppendCSV_WhenDelimiterIsNull_ArgsIsNotQuoted()
        {
            using (var sw = new StringWriter())
            {
                var options = new CSVOptions() { Delimiter = null };
                var result = sw.AppendCSV(options, "a").ToString();
                result.Should().Be("a");
            }
        }

        [Test]
        public void AppendCSV_WhenAlwaysWriteDelimiterIsTrueAndDelimiterIsNotInArgument_DelimitsTheArgument()
        {
            using (var sw = new StringWriter())
            {
                var options = new CSVOptions() { AlwaysWriteDelimiter = true };
                var result = sw.AppendCSV(options, "a").ToString();
                result.Should().Be("\"a\"");
            }
        }

        [Test]
        public void AppendCSV_WhenCharactersForcingDelimiterIsNull_DoesNotDelimit()
        {
            using (var sw = new StringWriter())
            {
                var options = new CSVOptions() { CharactersForcingDelimiter = null };
                var result = sw.AppendCSV(options, WordWithDoubleQuote).ToString();
                result.Should().Be(WordWithDoubleQuote);
            }
        }

        [Test]
        public void AppendCSV_WhenOneOfCharactersForcingDelimiterAppearsInArg_Delimits()
        {
            using (var sw = new StringWriter())
            {
                // Make it explicit rather than relying on the defaults.
                var options = new CSVOptions() { CharactersForcingDelimiter = new char[] { 'e', 'o' } };
                var result = sw.AppendCSV(options, "hello").ToString();
                result.Should().Be("\"hello\"");
            }
        }

        [Test]
        public void AppendCSV_ForDefaultOptions_WritesCompliantToRFC()
        {
            using (var sw = new StringWriter())
            {
                var result = sw.AppendCSV("a", ",", "b", null, "c", "", WordWithDoubleQuote, "\n").ToString();
                result.Should().Be("a,\",\",b,,c,," + QuotedWordWithDoubleQuote + ",\"\n\"");
            }
        }

        [Test]
        public void AppendCSV_ForHumanReadableOptions_CrunchesDown()
        {
            using (var sw = new StringWriter())
            {
                var result = sw.AppendCSV(CSVOptions.HumanReadable, "30", null, "Station Road  ", "", " Beverley", null, "HU17 2AA").ToString();
                result.Should().Be("30,Station Road,Beverley,HU17 2AA");
            }
        }

        [Test]
        public void AppendCSV_ForHumanReadableOptionsWithSpace_CrunchesDown()
        {
            using (var sw = new StringWriter())
            {
                var result = sw.AppendCSV(CSVOptions.HumanReadableWithSpace, "30", null, "Station Road  ", "", " Beverley", null, "HU17 2AA").ToString();
                result.Should().Be("30, Station Road, Beverley, HU17 2AA");
            }
        }

        [Ignore]
        [Test]
        public void AppendCSV_CanGenerateExcelFriendlyFile()
        {
            using (var sw = new StreamWriter(@"C:\temp\example.csv"))
            {
                sw.AppendCSV("Alpha", "Beta", "Gamma");
                sw.WriteLine();
                sw.AppendCSV("a1", "b1", "c1");
                sw.WriteLine();
                sw.AppendCSV("a2", ",", "c2");                      // line with comma
                sw.WriteLine();
                sw.AppendCSV("a3", "Really \"Very\" Good", "c3");   // line with double quote
                sw.WriteLine();
                sw.AppendCSV("hello\r\nworld", "b4", "c4");         // line with newline
                sw.WriteLine();
                sw.AppendCSV(null, "", "c5");                       // line with empty and null
                sw.WriteLine();
                sw.Flush();
            }
        }
    }
}
