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
                Name = "Rafael Jorge Alves",
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
                }
            };

            // Act
            var result = await _validator.ValidateAsync(member);

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
        }

        public static object[][] ShouldGetErrorForInvalidMember_Data = new object[][]
        {
            //Test 1
            new object[]
            {
                new MemberDto()
                {
                    Id = 1,
                Name = "Rafael",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealvesgmail.com",
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
                },
             //errorMessagem
            MemberDtoValidator.Email_ErrorMessage
            },

            //Test 2
            new object[]
            {
                new MemberDto()
                {
                    Id = 1,
                Name = "",
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
                },
             //errorMessagem
            MemberDtoValidator.Name_ErrorMessage
            },
        };

        [Theory]
        [MemberData(nameof(ShouldGetErrorForInvalidMember_Data))]
        public async Task ShouldGetErrorForInvalidMemberData(MemberDto dto, string errorMessage)
        {
            //Act
            var result = await _validator.ValidateAsync(dto);

            //Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => errorMessage.Equals(x.ErrorMessage));
        }


    }
}