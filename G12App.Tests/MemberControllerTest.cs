using FluentAssertions;
using G12App.Domain.DTOs;
using G12App.Domain.Messages;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Member = G12App.Domain.Model.Member;

namespace G12App.Tests
{
    public class MemberControllerTest
    {
        private readonly TestsFixture _testsFixture = new TestsFixture();
        private readonly Member defaultMember = new Member()
        {
            Id = 1,
            Name = "Rafael Alves",
            BirthDate = new System.DateTime(1993, 05, 10),
            Email = "rafaeljorgealves@gmail.com",
            Gender = Domain.Model.Enum.Gender.Male,
            HomeAddressId = 1,
        };

        [Fact]
        public async Task GetTest()
        {
            var queryJson = new StringContent(
                JsonSerializer.Serialize(new Query<MemberFilter>(), _testsFixture.JsonSerializerOptions),
                Encoding.UTF8,
                "application/json");

            //Act
            var response = await _testsFixture.HttpClient.PostAsync("/api/member/search", queryJson);

            //Assert
            var result = await response.Content.ReadAsStringAsync();
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetByIdTest()
        {
            //Arrange
            var member = defaultMember;
            var memberJson = new StringContent(
                JsonSerializer.Serialize(member, _testsFixture.JsonSerializerOptions),
                Encoding.UTF8,
                "application/json");

            //Act
            var response = await _testsFixture.HttpClient.GetAsync("/api/member/" + member.Id);
            var result = await response.Content.ReadAsStringAsync();

            //Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task CreateTest()
        {
            //Arrange
            var member = new Member()
            {

                Name = "Rafael Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddressId = 1,
            };
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

        [Fact]
        public async Task UpdateTest()
        {
            //Arrange
            var dfmember = defaultMember;
            var resultMember = new Member()
            {
                Id = 1,
                Name = "Rafael Alvessss",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddressId = 1,
            };


            var resultMemberJson = new StringContent(
                JsonSerializer.Serialize(resultMember, _testsFixture.JsonSerializerOptions),
                Encoding.UTF8,
                "application/json");

            //Act
            var response = await _testsFixture.HttpClient.PutAsync("/api/member/" + resultMember.Id, resultMemberJson);

            //Assert
            var result = await response.Content.ReadAsStringAsync();
            result.Should().NotBeNullOrEmpty();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task RemoveTest()
        {
            //Arrange
            var member = new Member()
            {

                Name = "Rafael",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddressId = 1,
            };
            var memberJson = new StringContent(
                JsonSerializer.Serialize(member, _testsFixture.JsonSerializerOptions),
                Encoding.UTF8,
                "application/json");
            var postResponse = await _testsFixture.HttpClient.PostAsync("/api/member", memberJson);
            var postResponseString = await postResponse.Content.ReadAsStringAsync();
            var jsonMember = await postResponse.Content.ReadFromJsonAsync<ServiceResult<MemberDto>>(_testsFixture.JsonSerializerOptions);

            //Act

            var removeResponse = await _testsFixture.HttpClient.DeleteAsync("/api/member/" + jsonMember.Data.Id);

            //Assert
            var result = await removeResponse.Content.ReadAsStringAsync();
            removeResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}