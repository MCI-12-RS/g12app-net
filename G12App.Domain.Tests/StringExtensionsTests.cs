using FluentAssertions;
using G12App.Domain.Extensions;

namespace G12App.Domain.Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("Joao da Silva", "Joao Silva")]
        [InlineData("  Maria   Aparecida   Souza  ", "Maria Souza")]
        public void FormatFirstLastName_ShouldReturnFirstAndLastNames(string input, string expected)
        {
            input.FormatFirstLastName().Should().Be(expected);
        }

        [Fact]
        public void FormatFirstLastName_ShouldReturnEmptyString_WhenInputIsNullOrWhitespace()
        {
            string empty = " ";
            string nullValue = null;

            empty.FormatFirstLastName().Should().Be(string.Empty);
            nullValue.FormatFirstLastName().Should().Be(string.Empty);
        }

        [Fact]
        public void FormatFirstLastName_ShouldRepeatSingleName()
        {
            "Joao".FormatFirstLastName().Should().Be("Joao Joao");
        }
    }
}
