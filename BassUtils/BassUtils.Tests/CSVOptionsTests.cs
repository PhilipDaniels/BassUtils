using System;
using FluentAssertions;
using NUnit.Framework;

namespace BassUtils.Tests
{
    [TestFixture]
    public class CSVOptionsTests
    {

        Action[] Mutators(CSVOptions options)
        {
            return new Action[]
            {
                () => options.WriteLeadingSeparator = true,
                () => options.AlwaysWriteDelimiter = true,
                () => options.CharactersForcingDelimiter = null,
                () => options.Delimiter = ".",
                () => options.Separator = ":",
                () => options.SkipEmptyValues = true,
                () => options.SkipNullValues = true,
                () => options.TrimStrings = true,
                () => options.WriteLeadingSeparator = true
            };
        }

        [Test]
        public void CrunchingOptions_AreReadOnly()
        {
            foreach (var act in Mutators(CSVOptions.CrunchingOptions))
            {
                act.ShouldThrow<InvalidOperationException>()
                    .WithMessage("*CSVOptions*cannot be changed*");
            }
        }

        [Test]
        public void DefaultOptions_AreReadOnly()
        {
            foreach (var act in Mutators(CSVOptions.Default))
            {
                act.ShouldThrow<InvalidOperationException>()
                    .WithMessage("*CSVOptions*cannot be changed*");
            }
        }

        [Test]
        public void HumanReadableOptions_AreReadOnly()
        {
            foreach (var act in Mutators(CSVOptions.HumanReadable))
            {
                act.ShouldThrow<InvalidOperationException>()
                    .WithMessage("*CSVOptions*cannot be changed*");
            }
        }

        [Test]
        public void HumanReadableWithSpaceOptions_AreReadOnly()
        {
            foreach (var act in Mutators(CSVOptions.HumanReadableWithSpace))
            {
                act.ShouldThrow<InvalidOperationException>()
                    .WithMessage("*CSVOptions*cannot be changed*");
            }
        }

        [Test]
        public void ConstructedOptions_AreWriteable()
        {
            var options = new CSVOptions();
            foreach (var act in Mutators(options))
            {
                act.ShouldNotThrow<InvalidOperationException>();
            }
        }

        [Test]
        public void Clone_CopiesAllPropertiesButsSetsReadOnlyToFalse()
        {
            CSVOptions.Default.IsReadOnly.Should().BeTrue();

            var options = CSVOptions.Default.Clone();

            options.IsReadOnly.Should().BeFalse();
            options.Separator.Should().Be(CSVOptions.Default.Separator);
            options.WriteLeadingSeparator.Should().Be(CSVOptions.Default.WriteLeadingSeparator);
            options.Delimiter.Should().Be(CSVOptions.Default.Delimiter);
            options.CharactersForcingDelimiter.Should().Equal(CSVOptions.Default.CharactersForcingDelimiter);
            options.AlwaysWriteDelimiter.Should().Be(CSVOptions.Default.AlwaysWriteDelimiter);
            options.SkipNullValues.Should().Be(CSVOptions.Default.SkipNullValues);
            options.SkipEmptyValues.Should().Be(CSVOptions.Default.SkipEmptyValues);
            options.TrimStrings.Should().Be(CSVOptions.Default.TrimStrings);
        }
    }
}
