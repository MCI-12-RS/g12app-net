using FluentAssertions;
using G12App.Domain.DTOs;
using G12App.Domain.Validators;

namespace G12App.Domain.Tests
{
    public class AddressDtoValidatorTests
    {
        private readonly AddressDtoValidator _validator;

        public AddressDtoValidatorTests()
        {
            _validator = new AddressDtoValidator();
        }

        [Fact]
        public async Task ShouldGetNoErrorsForValidAddress()
        {
            //Arrange
            var address = new AddressDto()
            {
                Id = 1,
                Street = "Biguas",
                Number = "600",
                City = "Alvorada",
                Neighborhood = "Porto Verde",
                Notes = string.Empty,
                PostalCode = "94858540",
                State = "RS",
            };

            //Act
            var result = await _validator.ValidateAsync(address);

            //Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldGetErrorsForInvalidAddress()
        {
            //Arrange
            var address = new AddressDto()
            {
                Id = 1,
                Street = null,
                Number = "600",
                City = "Alvorada",
                Neighborhood = "Porto Verde",
                Notes = string.Empty,
                PostalCode = "94858540",
                State = "RS",
            };

            //Act
            var result = await _validator.ValidateAsync(address);

            //Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => AddressDtoValidator.Street_ErrorMessage.Equals(x.ErrorMessage));
        }
    }
}
