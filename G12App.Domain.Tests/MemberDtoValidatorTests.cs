using G12App.Domain.Model;
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
        public void ShouldGetNoErrorsForValidMember()
        {
            //var member = new Member()
            //{
            //    Id = 1,
            //    Name = "Rafael",
            //    Surname = "Jorge Alves",
            //    BirthDate = new System.DateTime(1993, 05, 10),
            //    Email = "rafaeljorgealves@gmail.com",
            //    Gender = Domain.Model.Enum.Gender.Male,
            //    HomeAddressId = 1,
            //    HomeAddress = new Address()
            //    {
            //        Id = 1,
            //        Street = "Biguas",
            //        Number = "600",
            //        City = "Alvorada",
            //        Neighborhood = "Porto Verde",
            //        Notes = string.Empty,
            //        PostalCode = "94858540",
            //        State = "RS",
            //    },
            //    MemberType = Domain.Model.Enum.MemberType.Leadership,
            //};

            //var result = _validator.ValidateAsync(member);

        }
    }
}