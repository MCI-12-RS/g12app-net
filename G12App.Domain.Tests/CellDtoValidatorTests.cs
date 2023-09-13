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


        public static object[][] ShouldGetErrorForInvalidCell_Data = new object[][]
        {
            // Test 1
            new object[] {

                // dto
                new CellDTO()
                {
                    Id = 1,
                    LeadId = 0,
                    HostId = 1,
                    TimothyId = 1,
                    WeekDay = System.DayOfWeek.Tuesday,
                    PlaceId = 1,
                    CellType = Domain.Model.Enum.CellType.Evangelistic,
                    ReportMandatory = true,
                },
                
                // errorMessage
                CellDtoValidator.LeadID_ErrorMessage
            },

            // Test 2
            new object[] {
                // dto
                new CellDTO()
                {
                    Id = 1,
                    LeadId = 1,
                    HostId = 0,
                    TimothyId = 1,
                    WeekDay = System.DayOfWeek.Tuesday,
                    PlaceId = 1,
                    CellType = Domain.Model.Enum.CellType.Evangelistic,
                    ReportMandatory = true,
                },
                
                // errorMessage
                CellDtoValidator.HostID_ErrorMessage
            },

            // Test 3
            new object[] {
                // dto
                new CellDTO()
                {
                    Id = 1,
                    LeadId = 1,
                    HostId = 1,
                    TimothyId = 0,
                    WeekDay = System.DayOfWeek.Tuesday,
                    PlaceId = 1,
                    CellType = Domain.Model.Enum.CellType.Evangelistic,
                    ReportMandatory = true,
                },
                
                // errorMessage
                CellDtoValidator.TimothyID_ErrorMessage
            },

            // Test 4
            new object[] {
                // dto
                new CellDTO()
                {
                    Id = 1,
                    LeadId = 1,
                    HostId = 1,
                    TimothyId = 1,
                    WeekDay = System.DayOfWeek.Tuesday,
                    PlaceId = 0,
                    CellType = Domain.Model.Enum.CellType.Evangelistic,
                    ReportMandatory = true,
                },
                
                // errorMessage
                CellDtoValidator.PlaceID_ErrorMessage
            }
        };

        [Theory]
        [MemberData(nameof(ShouldGetErrorForInvalidCell_Data))]
        public async Task ShouldGetErrorForInvalidCell_MemberData(CellDTO dto, string errorMessage)
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
