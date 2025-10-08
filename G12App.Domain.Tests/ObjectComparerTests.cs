using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using G12App.Domain.Helpers;

namespace G12App.Domain.Tests
{
    public class ObjectComparerTests
    {
        [Fact]
        public void CompareObjects_WithIdenticalObjects_ShouldReturnEmptyList()
        {
            var first = CreateSample();
            var second = CreateSample();

            var differences = ObjectComparer.CompareObjects(first, second);

            differences.Should().BeEmpty();
        }

        [Fact]
        public void CompareObjects_WithPrimitiveDifference_ShouldReturnSingleDifference()
        {
            var first = CreateSample();
            var second = CreateSample();
            second.Id = first.Id + 1;

            var differences = ObjectComparer.CompareObjects(first, second);

            differences.Should().ContainSingle();
            differences[0].PropertyPath.Should().Be(nameof(Sample.Id));
            differences[0].Value1.Should().Be(first.Id);
            differences[0].Value2.Should().Be(second.Id);
        }

        [Fact]
        public void CompareObjects_WithNestedDifference_ShouldReturnPropertyPath()
        {
            var first = CreateSample();
            var second = CreateSample();
            second.Address.City = "New City";

            var differences = ObjectComparer.CompareObjects(first, second);

            differences.Should().ContainSingle(d => d.PropertyPath == $"{nameof(Sample.Address)}.{nameof(Address.City)}");
        }

        [Fact]
        public void CompareObjects_WithCollectionDifference_ShouldTrackIndex()
        {
            var first = CreateSample();
            var second = CreateSample();
            second.Tags = new List<string> { "alpha", "delta", "omega" };

            var differences = ObjectComparer.CompareObjects(first, second);

            differences.Should().Contain(d => d.PropertyPath == $"{nameof(Sample.Tags)}[1]" && (string)d.Value1 == "beta" && (string)d.Value2 == "delta");
            differences.Should().Contain(d => d.PropertyPath == $"{nameof(Sample.Tags)}[2]" && d.Value1 == null && (string)d.Value2 == "omega");
        }

        [Fact]
        public void CompareObjects_WhenOneObjectIsNull_ShouldReturnDifferenceForRoot()
        {
            var first = CreateSample();

            var differences = ObjectComparer.CompareObjects(first, null);

            differences.Should().ContainSingle();
            differences[0].PropertyPath.Should().BeEmpty();
            differences[0].Value1.Should().Be(first);
            differences[0].Value2.Should().BeNull();
        }

        private static Sample CreateSample()
        {
            return new Sample
            {
                Id = 1,
                Name = "Sample Name",
                Address = new Address
                {
                    City = "City",
                    Number = 100
                },
                Tags = new List<string> { "alpha", "beta" }
            };
        }

        private class Sample
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Address Address { get; set; }
            public List<string> Tags { get; set; }
        }

        private class Address
        {
            public string City { get; set; }
            public int Number { get; set; }
        }
    }
}
