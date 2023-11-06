using FluentAssertions;
using FluentValidation;
using G12App.Domain.DTOs;
using G12App.Domain.Validators;

namespace G12App.Domain.Tests
{
    public class ReportDtoValidatorTests
    {
        private readonly ReportDTOValidator _validator;

        public ReportDtoValidatorTests()
        {
            _validator = new ReportDTOValidator();
        }

        [Fact]
        public async Task ShouldGetNoErrorsForValidReport()
        {
            //Arrange
            var report = new ReportDTO()
            {
                ReportId = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            //Act
            var result = await _validator.ValidateAsync(report);

            //Arrange
            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
        }

        public static object[][] ShouldGetErrorsForInvalidReport_Data = new object[][]
        {
            //Test 1 
            new object[]
            {
                new ReportDTO()
                {
                    ReportId = 1,
                    Subject = "",
                    Date = new DateTime(2023, 9, 28),
                    Visitors = 1,
                    CellId = 1,
                    LeadershipId = 1,
                    HostId = 1
                },
                //Error message
                ReportDTOValidator.Subject_ErrorMessage
            },

            //Test 2
            new object[]
            {
                new ReportDTO()
                {
                    ReportId = 1,
                    Subject = "Valid subject",
                    Date = DateTime.MinValue,
                    Visitors = 1,
                    CellId = 1,
                    LeadershipId = 1,
                    HostId = 1
                },
                //Error message
                ReportDTOValidator.Date_ErrorMessage
            },
            
            //Test 3
            new object[]
            {
                new ReportDTO()
                {
                    ReportId = 1,
                    Subject = "Valid subject",
                    Date = new DateTime(2023, 9, 28),
                    Visitors = 1,
                    CellId = 0,
                    LeadershipId = 1,
                    HostId = 1
                },
                //Error message
                ReportDTOValidator.Id_ErrorMessage
            },

            //Test 4
            new object[]
            {
                new ReportDTO()
                {
                    ReportId = 1,
                    Subject = "Valid subject",
                    Date = new DateTime(2023, 9, 28),
                    Visitors = 1,
                    CellId = 1,
                    LeadershipId = 0,
                    HostId = 1
                },
                //Error message
                ReportDTOValidator.Id_ErrorMessage
            }
        };

        [Theory]
        [MemberData(nameof(ShouldGetErrorsForInvalidReport_Data))]
        public async Task ShouldGetErrorForInvalidReportData(ReportDTO dto, string errorMessage)
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
