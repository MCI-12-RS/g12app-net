using FluentAssertions;
using G12App.Domain.DTOs;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace G12App.Tests
{
    public class CellControllerTests
    {
        private readonly TestsFixture _testsFixture = new TestsFixture();
        private readonly CellDTO dfcell = new CellDTO()
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

        [Fact]
        public async Task GetCellTests()
        {
            //Act
            var response = await _testsFixture.HttpClient.GetAsync("api/cell");
            var result = await response.Content.ReadAsStringAsync();

            //Assert
            result.Should().NotBeNullOrEmpty();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetByIdTest()
        {
            //Arrange
            var cell = dfcell;

            //Act
            var response = await _testsFixture.HttpClient.GetAsync("api/cell/"+cell.Id);
            var result = await response.Content.ReadAsStringAsync();

            //Assert
            result.Should().NotBeNullOrEmpty();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateTest()
        {
            //Arrange
            var cell = new Cell()
            {
                LeadId = 1,
                HostId = 1,
                TimothyId = 1,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
            };

            var cellJson = new StringContent(
                JsonSerializer.Serialize(cell, _testsFixture.JsonSerializerOptions),
                Encoding.UTF8,
                "application/json");

            //Act
            var response = await _testsFixture.HttpClient.PostAsync("api/cell/", cellJson);
            var result = response.Content.ReadAsStringAsync();

            //Assert
            result.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task RemoveTest()
        {
            //Arrange
            var cell = new Cell()
            {
                LeadId = 1,
                HostId = 1,
                TimothyId = 1,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
            };

            var cellJson = new StringContent(
                JsonSerializer.Serialize(cell, _testsFixture.JsonSerializerOptions),
                Encoding.UTF8,
                "application/json");
            var postResponse = await _testsFixture.HttpClient.PostAsync("api/cell/", cellJson);
            var jsonCell = await postResponse.Content.ReadFromJsonAsync<ServiceResult<CellDTO>>(_testsFixture.JsonSerializerOptions);

            //Act
            var response = await _testsFixture.HttpClient.DeleteAsync("api/cell/" + jsonCell.Data.Id);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
