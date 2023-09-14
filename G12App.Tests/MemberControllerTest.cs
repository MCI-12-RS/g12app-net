using Microsoft.AspNetCore.Mvc.Testing;
using G12App;
using FluentAssertions;
using System.Text.Json;
using System.Text;
using G12App.Domain.Model;

namespace G12App.Tests
{
    public class MemberControllerTest
    {
        private readonly TestsFixture _testsFixture = new TestsFixture();

        [Fact]
        public async Task GetTest()
        {
            //Act
            var response = await _testsFixture.HttpClient.GetAsync("/api/member");

            //Assert
            var result = await response.Content.ReadAsStringAsync();
            result.Should().NotBeNullOrEmpty(result);
        }

        [Fact]
        public async Task CreateTest()
        {
            //Arrange
            var member = new Member();
            var memberJson = new StringContent(
                JsonSerializer.Serialize(member, _testsFixture.JsonSerializerOptions),
                Encoding.UTF8,
                "application/json");

            //Act
            var response = await _testsFixture.HttpClient.PostAsync("/api/member", memberJson);

            //Assert
            var result = await response.Content.ReadAsStringAsync();
            result.Should().NotBeNullOrEmpty();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}