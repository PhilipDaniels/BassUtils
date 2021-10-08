using System;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BassUtils.UnitTests
{
    [TestClass]
    public class TextWriterExtensionTests
    {
        [TestMethod]
        public void AppendCsv_ForNullWriter_ThrowsArgumentNullException()
        {
            Action act = () => TextWriterExtensions.AppendCsv(null);

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*writer*")
                .And.ParamName.Should().Be("writer");
        }

        [TestMethod]
        public void AppendCsv_ForNullOptions_ThrowsArgumentNullException()
        {
            using (var sw = new StringWriter())
            {
                Action act = () => sw.AppendCsv(null, false);

                act.Should().Throw<ArgumentNullException>()
                    .WithMessage("*options*")
                    .And.ParamName.Should().Be("options");
            }
        }

        [TestMethod]
        public void AppendCsv_ForNullArgumentsAndAnyAppendInitial_ReturnsEmptyString()
        {
            using (var sw = new StringWriter())
            {
                var options = CsvOptions.Default.Clone();
                options.WriteLeadingSeparator = true;
                var result = sw.AppendCsv(options).ToString();
                result.Should().BeEmpty();
            }

            using (var sw = new StringWriter())
            {
                var options = CsvOptions.Default.Clone();
                options.WriteLeadingSeparator = false;
                var result = sw.AppendCsv(options).ToString();
                result.Should().BeEmpty();
            }
        }

        [TestMethod]
        public void AppendCsv_ForEmptyArgumentsAndAnyAppendInitial_ReturnsEmptyString()
        {
            var args = new object[] { };

            using (var sw = new StringWriter())
            {
                var options = CsvOptions.Default.Clone();
                options.WriteLeadingSeparator = true;
                var result = sw.AppendCsv(options, args).ToString();
                result.Should().BeEmpty();
            }

            using (var sw = new StringWriter())
            {
                var options = CsvOptions.Default.Clone();
                options.WriteLeadingSeparator = false;
                var result = sw.AppendCsv(options, args).ToString();
                result.Should().BeEmpty();
            }
        }

        [TestMethod]
        public void AppendCsv_WhenAlwaysWriteDelimiterIsTrueAndDelimiterIsNull_ThrowsArgumentException()
        {
            using (var sw = new StringWriter())
            {
                var options = new CsvOptions() { AlwaysWriteDelimiter = true, Delimiter = null };
                Action act = () => sw.AppendCsv(options);

                act.Should().Throw<ArgumentException>()
                    .WithMessage("*AlwaysWriteDelimiter*Delimiter*non-null*")
                    .And.ParamName.Should().Be("options");
            }
        }

        [TestMethod]
        public void AppendCsv_WhenCallIsValid_ReturnsSameWriter()
        {
            using (var sw = new StringWriter())
            {
                var writer = sw.AppendCsv();
                writer.Should().BeSameAs(sw);
            }
        }

        [TestMethod]
        public void AppendCsv_WhenInitialSeparatorRequiredIsTrue_PrependsSeparator()
        {
            using (var sw = new StringWriter())
            {
                var options = CsvOptions.Default.Clone();
                options.WriteLeadingSeparator = true;
                var result = sw.AppendCsv(options, "a").ToString();
                result.Should().Be(",a");
            }
        }

        [TestMethod]
        public void AppendCsv_WhenInitialSeparatorRequiredIsFalse_DoesNotPrependSeparator()
        {
            using (var sw = new StringWriter())
            {
                var options = CsvOptions.Default.Clone();
                options.WriteLeadingSeparator = false;
                var result = sw.AppendCsv(options, "a").ToString();
                result.Should().Be("a");
            }
        }

        [TestMethod]
        public void AppendCsv_WhenSkipNullValuesIsTrue_SkipsOverNullArguments()
        {
            using (var sw = new StringWriter())
            {
                var options = new CsvOptions() { SkipNullValues = true };
                var result = sw.AppendCsv(options, "a", null, "b").ToString();
                result.Should().Be("a,b");
            }
        }

        [TestMethod]
        public void AppendCsv_WhenSkipNullValuesIsFalse_WritesEmptyStringForNullArguments()
        {
            using (var sw = new StringWriter())
            {
                var options = new CsvOptions() { SkipNullValues = false };
                var result = sw.AppendCsv(options, "a", null, "b").ToString();
                result.Should().Be("a,,b");
            }
        }

        [TestMethod]
        public void AppendCsv_WhenSkipEmptyValuesIsTrue_SkipsOverNullArguments()
        {
            using (var sw = new StringWriter())
            {
                var options = new CsvOptions() { SkipEmptyValues = true };
                var result = sw.AppendCsv(options, "a", "", "b").ToString();
                result.Should().Be("a,b");
            }
        }

        [TestMethod]
        public void AppendCsv_WhenSkipEmptyValuesIsFalse_WritesEmptyStringForNullArguments()
        {
            using (var sw = new StringWriter())
            {
                var options = new CsvOptions() { SkipEmptyValues = false };
                var result = sw.AppendCsv(options, "a", "", "b").ToString();
                result.Should().Be("a,,b");
            }
        }

        [TestMethod]
        public void AppendCsv_WhenTrimStringsIsFalse_IncludesWhitespaceInTheOutput()
        {
            using (var sw = new StringWriter())
            {
                var options = new CsvOptions() { TrimStrings = false };
                var result = sw.AppendCsv(options, "a ", " b ").ToString();
                result.Should().Be("a , b ");
            }
        }

        [TestMethod]
        public void AppendCsv_WhenTrimStringsIsTrue_TrimsArgumentsBeforeAppending()
        {
            using (var sw = new StringWriter())
            {
                var options = new CsvOptions() { TrimStrings = true };
                var result = sw.AppendCsv(options, "a ", " b ").ToString();
                result.Should().Be("a,b");
            }
        }

        [TestMethod]
        public void AppendCsv_WhenTrimStringsIsTrueAndSkipEmptyValuesIsTrue_SkipsWhitespaceStrings()
        {
            using (var sw = new StringWriter())
            {
                var options = new CsvOptions() { TrimStrings = true, SkipEmptyValues = true };
                var result = sw.AppendCsv(options, "a ", "  ", " b ").ToString();
                result.Should().Be("a,b");
            }
        }

        const string WordWithDoubleQuote = "O\"Malley";
        const string QuotedWordWithDoubleQuote = "\"O\"\"Malley\"";  // double and wrap.

        [TestMethod]
        public void AppendCsv_WhenDelimiterIsNonNullAndAppearsInArg_DelimiterIsDoubledAndArgIsDelimited()
        {
            using (var sw = new StringWriter())
            {
                var result = sw.AppendCsv(WordWithDoubleQuote).ToString();
                result.Should().Be(QuotedWordWithDoubleQuote);
            }
        }

        [TestMethod]
        public void AppendCsv_WhenDelimiterIsNonNullAndDoesNotAppearInArg_ResultIsNotDelimited()
        {
            using (var sw = new StringWriter())
            {
                var result = sw.AppendCsv("a").ToString();
                result.Should().Be("a");
            }
        }

        [TestMethod]
        public void AppendCsv_WhenDelimiterIsNull_ArgsIsNotQuoted()
        {
            using (var sw = new StringWriter())
            {
                var options = new CsvOptions() { Delimiter = null };
                var result = sw.AppendCsv(options, "a").ToString();
                result.Should().Be("a");
            }
        }

        [TestMethod]
        public void AppendCsv_WhenAlwaysWriteDelimiterIsTrueAndDelimiterIsNotInArgument_DelimitsTheArgument()
        {
            using (var sw = new StringWriter())
            {
                var options = new CsvOptions() { AlwaysWriteDelimiter = true };
                var result = sw.AppendCsv(options, "a").ToString();
                result.Should().Be("\"a\"");
            }
        }

        [TestMethod]
        public void AppendCsv_WhenCharactersForcingDelimiterIsNull_DoesNotDelimit()
        {
            using (var sw = new StringWriter())
            {
                var options = new CsvOptions() { CharactersForcingDelimiter = null };
                var result = sw.AppendCsv(options, WordWithDoubleQuote).ToString();
                result.Should().Be(WordWithDoubleQuote);
            }
        }

        [TestMethod]
        public void AppendCsv_WhenOneOfCharactersForcingDelimiterAppearsInArg_Delimits()
        {
            using (var sw = new StringWriter())
            {
                // Make it explicit rather than relying on the defaults.
                var options = new CsvOptions() { CharactersForcingDelimiter = new char[] { 'e', 'o' } };
                var result = sw.AppendCsv(options, "hello").ToString();
                result.Should().Be("\"hello\"");
            }
        }

        [TestMethod]
        public void AppendCsv_ForDefaultOptions_WritesCompliantToRFC()
        {
            using (var sw = new StringWriter())
            {
                var result = sw.AppendCsv("a", ",", "b", null, "c", "", WordWithDoubleQuote, "\n").ToString();
                result.Should().Be("a,\",\",b,,c,," + QuotedWordWithDoubleQuote + ",\"\n\"");
            }
        }

        [TestMethod]
        public void AppendCsv_ForHumanReadableOptions_CrunchesDown()
        {
            using (var sw = new StringWriter())
            {
                var result = sw.AppendCsv(CsvOptions.HumanReadable, "30", null, "Station Road  ", "", " Beverley", null, "HU17 2AA").ToString();
                result.Should().Be("30,Station Road,Beverley,HU17 2AA");
            }
        }

        [TestMethod]
        public void AppendCsv_ForHumanReadableOptionsWithSpace_CrunchesDown()
        {
            using (var sw = new StringWriter())
            {
                var result = sw.AppendCsv(CsvOptions.HumanReadableWithSpace, "30", null, "Station Road  ", "", " Beverley", null, "HU17 2AA").ToString();
                result.Should().Be("30, Station Road, Beverley, HU17 2AA");
            }
        }

        [Ignore]
        [TestMethod]
        public void AppendCsv_CanGenerateExcelFriendlyFile()
        {
            using (var sw = new StreamWriter(@"C:\temp\example.Csv"))
            {
                sw.AppendCsv("Alpha", "Beta", "Gamma");
                sw.WriteLine();
                sw.AppendCsv("a1", "b1", "c1");
                sw.WriteLine();
                sw.AppendCsv("a2", ",", "c2");                      // line with comma
                sw.WriteLine();
                sw.AppendCsv("a3", "Really \"Very\" Good", "c3");   // line with double quote
                sw.WriteLine();
                sw.AppendCsv("hello\r\nworld", "b4", "c4");         // line with newline
                sw.WriteLine();
                sw.AppendCsv(null, "", "c5");                       // line with empty and null
                sw.WriteLine();
                sw.Flush();
            }
        }

    }
}
