using FluentAssertions;
using G12App.Domain.DTOs;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using G12App.Domain.Model.Enum;
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
            //Arrange
            var dfReport = toCreateReport;
            var reportJson = new StringContent(
                JsonSerializer.Serialize(dfReport, _testsFixture.JsonSerializerOptions),
                Encoding.UTF8,
                "application/json");

            var createResponse = await _testsFixture.HttpClient.PostAsync("/api/report/", reportJson);
            var postResponse = await createResponse.Content.ReadFromJsonAsync<ServiceResult<ReportDTO>>(_testsFixture.JsonSerializerOptions);

            //Act
            var result = await _testsFixture.HttpClient.DeleteAsync("/api/report/" + postResponse.Data.ReportId);

            //Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Update()
        {
            var dfReport = toCreateReport;
            var dfRepJson = new StringContent(
               JsonSerializer.Serialize(dfReport, _testsFixture.JsonSerializerOptions),
               Encoding.UTF8,
               "application/json");
            var createResponse = await _testsFixture.HttpClient.PostAsync("/api/report/", dfRepJson);
            var postResponse = await createResponse.Content.ReadFromJsonAsync<ServiceResult<ReportDTO>>(_testsFixture.JsonSerializerOptions);

            var repToUpdate = new ReportDTO()
            {
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
                Members = 1,
                CellId = 1,
                LeadershipId = 1,
                HostId = 1,
                CellType = CellType.Evangelistic
            };
            var toUpRepJson = new StringContent(
               JsonSerializer.Serialize(repToUpdate, _testsFixture.JsonSerializerOptions),
               Encoding.UTF8,
               "application/json");

            //Act
            var result = await _testsFixture.HttpClient.PutAsync("/api/report/" + postResponse.Data.ReportId, toUpRepJson);
            var resultJson = await result.Content.ReadFromJsonAsync<ServiceResult<ReportDTO>>(_testsFixture.JsonSerializerOptions);

            //Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            resultJson.Data.CellType.Should().Be(CellType.Evangelistic);
        }

        [Fact]
        public async Task GetById()
        { //Arrange
            var dfReport = toCreateReport;
            var dfRepJson = new StringContent(
               JsonSerializer.Serialize(dfReport, _testsFixture.JsonSerializerOptions),
               Encoding.UTF8,
               "application/json");
            var createResponse = await _testsFixture.HttpClient.PostAsync("/api/report/", dfRepJson);
            var postResponse = await createResponse.Content.ReadFromJsonAsync<ServiceResult<ReportDTO>>(_testsFixture.JsonSerializerOptions);

            //Act
            var result = await _testsFixture.HttpClient.GetAsync("/api/report/" + postResponse.Data.ReportId);
            var resultJson = await result.Content.ReadFromJsonAsync<ServiceResult<ReportDTO>>(_testsFixture.JsonSerializerOptions);

            //Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            resultJson.Data.CellType.Should().Be(CellType.Discipleship);
        }
    }
}
