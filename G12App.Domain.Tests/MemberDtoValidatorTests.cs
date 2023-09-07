using FluentAssertions;
using G12App.Domain.DTOs;
using G12App.Domain.Validators;

namespace G12App.Domain.Tests
{
    public class MemberDtoValidatorTests
    {
        private readonly MemberDtoValidator _validator;

        public MemberDtoValidatorTests()
        {
            _validator = new MemberDtoValidator();
        }

        [Fact]
        public async Task ShouldGetNoErrorsForValidMember()
        {
            // Arrange
            var member = new MemberDto()
            {
                Id = 1,
                Name = "Rafael",
                Surname = "Jorge Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddress = new AddressDto()
                {
                    Id = 1,
                    Street = "Biguas",
                    Number = "600",
                    City = "Alvorada",
                    Neighborhood = "Porto Verde",
                    Notes = string.Empty,
                    PostalCode = "94858540",
                    State = "RS",
                },
                MemberType = Domain.Model.Enum.MemberType.Leadership,
            };

            // Act
            var result = await _validator.ValidateAsync(member);

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldGetErrorsForInvalidMemberWithInvalidEmail()
        {
            // Arrange
            var member = new MemberDto()
            {
                Id = 1,
                Name = "Rafael",
                Surname = "Jorge Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "my_invalid_email",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddress = new AddressDto()
                {
                    Id = 1,
                    Street = "Biguas",
                    Number = "600",
                    City = "Alvorada",
                    Neighborhood = "Porto Verde",
                    Notes = string.Empty,
                    PostalCode = "94858540",
                    State = "RS",
                },
                MemberType = Domain.Model.Enum.MemberType.Leadership,
            };

            // Act
            var result = await _validator.ValidateAsync(member);


            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => MemberDtoValidator.Email_ErrorMessage.Equals(x.ErrorMessage));
        }
    }
}