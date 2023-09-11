using FluentAssertions;
using G12App.Domain.DTOs;
using G12App.Domain.Validators;

namespace G12App.Domain.Tests
{
    public class CellDtoValidatorTests
    {
        private readonly CellDtoValidator _validator;

        public CellDtoValidatorTests()
        {
            _validator = new CellDtoValidator();
        }
        [Fact]
        public async Task ShouldGetNoErrorsForValidCell()
        {
            //Arrange
            var cell = new CellDTO()
            {
                Id = 1,
                LeadId = 1,
                HostId = 1,
                TimothyId = 1,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
            };

            //Act
            var result = await _validator.ValidateAsync(cell);

            //Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
        }
        [Fact]
        public async Task ShouldGetErrorsForValidCellWithNullId()
        {
            //Arrange
            var cell = new CellDTO()
            {
                Id = null,
                LeadId = 1,
                HostId = 1,
                TimothyId = 1,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
            };

            //Act
            var result = await _validator.ValidateAsync(cell);

            //Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => CellDtoValidator.LeadID_ErrorMessage.Equals(x.ErrorMessage));
        }
    }
}
