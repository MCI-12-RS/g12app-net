using FluentAssertions;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using G12App.Infrastructure.Data.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace G12App.Infrastructure.Data.Tests
{
    public class MemberRepositoryTests
    {
        private readonly TestFixture _fixture = new TestFixture();
        private readonly MemberRepository _repository;
        private readonly Member _defaultMember = new Member()
        {
            Name = "Rafael Alves",
            BirthDate = new System.DateOnly(1993, 05, 10),
            Email = "rafaeljorgealves@gmail.com",
            Gender = Domain.Model.Enum.Gender.Male,
            HomeAddressId = 1,
        };
        private readonly Query query = new Query();

        public MemberRepositoryTests()
        {
            _repository = new MemberRepository(_fixture.Context, _fixture.Mapper);
        }

        [Fact]
        public async Task CreateTest()
        {
            //Arrange
            var createdMember = new Member()
            {
                Name = "Rafael Alves",
                BirthDate = new System.DateOnly(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddressId = 1,
            };

            //Act
            var result = await _repository.Create(createdMember);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdTest()
        {
            //Assert
            var member = await _repository.Create(_defaultMember);

            //Act
            var result = await _repository.GetMemberByIdAsync(member.Id);

            //Assert
            result.Should().Be(member);
        }

        [Fact]
        public async Task RemoveTest()
        {
            //Arrange
            var member = await _repository.Create(_defaultMember);

            //Act
            var result = await _repository.Remove(member);

            //Assert
            result.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task GetMembersTest()
        {
            //Arrange
            var member = await _repository.Create(_defaultMember);
            var query = new Query<MemberFilter>();

            //Act
            var result = await _repository.GetMembersAsync(query);

            //Assert
            result.Should().NotBeNull();
        }
    }
}
