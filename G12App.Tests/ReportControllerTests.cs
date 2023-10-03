using FluentAssertions;
using G12App.Domain.DTOs;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace G12App.Tests
{
    public class ReportControllerTests
    {
        private readonly TestsFixture _testsFixture = new TestsFixture();
        private readonly Report toCreateReport = new Report()
        {
            Subject = "Valid subject",
            Date = new DateTime(2023, 9, 28),
            Visitors = 1,
            Members = 1,
            CellId = 1,
            LeadershipId = 1,
            HostId = 1,
            CellType = Domain.Model.Enum.CellType.Discipleship
        };

        [Fact]
        public async Task GetTest()
        {
            var dfReport = toCreateReport;
            var reportJson = new StringContent(
                JsonSerializer.Serialize(dfReport, _testsFixture.JsonSerializerOptions),
                Encoding.UTF8,
                "application/json");

            var createResponse = await _testsFixture.HttpClient.PostAsync("/api/report/", reportJson);
            
            var query = new Query<ReportFilter>()
            {
                Pagination = new Pagination(1, 10),
                Filter = new ReportFilter()
            };
            var queryJson = new StringContent(
                JsonSerializer.Serialize(query, _testsFixture.JsonSerializerOptions),
                Encoding.UTF8,
                "application/json");

            //Act
            var response = await _testsFixture.HttpClient.PostAsync("/api/report/search", queryJson);

            //Assert
            var result = await response.Content.ReadAsStringAsync();
            result.Should().NotBeNullOrEmpty();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateTest()
        {
            //Arrange
            var dfReport = toCreateReport;
            var reportJson = new StringContent(
                JsonSerializer.Serialize(dfReport, _testsFixture.JsonSerializerOptions),
                Encoding.UTF8,
                "application/json");

            //Act
            var response = await _testsFixture.HttpClient.PostAsync("/api/report/", reportJson);

            //Assert
            var result = await response.Content.ReadAsStringAsync();
            result.Should().NotBeNullOrEmpty();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete()
        {
            var dfReport = toCreateReport;
            var reportJson = new StringContent(
                JsonSerializer.Serialize(dfReport, _testsFixture.JsonSerializerOptions),
                Encoding.UTF8,
                "application/json");

            var createResponse = await _testsFixture.HttpClient.PostAsync("/api/report/", reportJson);
            var postResponse = await createResponse.Content.ReadFromJsonAsync<ServiceResult<ReportDTO>>(_testsFixture.JsonSerializerOptions);

        }
    }
}
