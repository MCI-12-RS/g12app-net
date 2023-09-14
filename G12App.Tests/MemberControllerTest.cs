using Microsoft.AspNetCore.Mvc.Testing;
using G12App;
using FluentAssertions;
using System.Text.Json;
using System.Text;
using G12App.Domain.Model;
using AutoMapper.Execution;
using Member = G12App.Domain.Model.Member;

namespace G12App.Tests
{
    public class MemberControllerTest
    {
        private readonly TestsFixture _testsFixture = new TestsFixture();
        private readonly Member defaultMember = new Member()
        {
            Id = 1,
            Name = "Rafael",
            Surname = "Jorge Alves",
            BirthDate = new System.DateTime(1993, 05, 10),
            Email = "rafaeljorgealves@gmail.com",
            Gender = Domain.Model.Enum.Gender.Male,
            HomeAddressId = 1,
            MemberType = Domain.Model.Enum.MemberType.Leadership,
        };

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
        public async Task GetByIdTest()
        {
            //Arrange
            var member = defaultMember;
            var memberJson = new StringContent(
                JsonSerializer.Serialize(member, _testsFixture.JsonSerializerOptions),
                Encoding.UTF8,
                "application/json");

            //Act
            var response = await _testsFixture.HttpClient.GetAsync("/api/member/"+member.Id);
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
                
                Name = "Rafael",
                Surname = "Jorge Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddressId = 1,
                MemberType = Domain.Model.Enum.MemberType.Leadership,
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
                Name = "Rafael",
                Surname = "Jorge Alvessss",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddressId = 1,
                MemberType = Domain.Model.Enum.MemberType.Leadership,
            };

           
            var resultMemberJson = new StringContent(
                JsonSerializer.Serialize(resultMember, _testsFixture.JsonSerializerOptions),
                Encoding.UTF8,
                "application/json");

            //Act
            var response = await _testsFixture.HttpClient.PutAsync("/api/member/"+resultMember.Id, resultMemberJson);

            //Assert
            var result = await response.Content.ReadAsStringAsync();
            result.Should().NotBeNullOrEmpty();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task RemoveTest()
        {
            //Arrange
            var member = defaultMember;

            //Act
            var response = await _testsFixture.HttpClient.DeleteAsync("/api/member"+member.Id);

            //Assert
            var result = await response.Content.ReadAsStringAsync();
            result.Should().BeNullOrEmpty();
            
        }
    }
}