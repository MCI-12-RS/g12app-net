using Moq;
using G12App.Domain.Interfaces.Repositories;
using AutoMapper;
using G12App.Domain.Model;
using G12App.Domain.DTOs;
using G12App.Application.Services;
using FluentAssertions;
using G12App.Domain.Interfaces.Services;

namespace G12App.Application.Tests
{
    public class MemberServiceTests
    {
        [Fact]
        public async void AddMemberSeerviceIsOk()
        {
            var member = new Member()
            {
                Id = 1,
                Name = "Rafael Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddressId = 1,
            };

            var memberDto = new MemberDto()
            {
                Id = 1,
                Name = "Rafael Jorge Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
            };

            //mock
            var mock = new Mock<IMemberRepository>();
            var mapperMock = new Mock<IMapper>();

            mock.Setup(x => x.Create(It.IsAny<Member>())).ReturnsAsync(member);
            mapperMock.Setup(x => x.Map<Member>(memberDto)).Returns(member);
            mapperMock.Setup(x => x.Map<MemberDto>(member)).Returns(memberDto);

            //Act
            var service = new MemberService(mock.Object, mapperMock.Object, Mock.Of<IEntityChangeTrackingService>());
            var result = await service.Add(memberDto);

            //Assert
            mapperMock.Verify(x => x.Map<Member>(memberDto));
            mock.Verify(x => x.Create(member));
            mapperMock.Verify(x => x.Map<MemberDto>(member));
        }

        [Fact]
        public async void RemoveMemberServiceIsOk()
        {
            var member = new Member()
            {
                Id = 1,
                Name = "Rafael Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddressId = 1,
            };

            var memberDto = new MemberDto()
            {
                Id = 1,
                Name = "Rafael Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
            };
            //mock
            var mock = new Mock<IMemberRepository>();
            var mapperMock = new Mock<IMapper>();

            mock.Setup(x => x.GetMemberByIdAsync(member.Id)).ReturnsAsync(member);
            mock.Setup(x => x.Remove(It.IsAny<Member>())).ReturnsAsync(member);
            mapperMock.Setup(x => x.Map<MemberDto>(member)).Returns(memberDto);

            //Act
            var service = new MemberService(mock.Object, mapperMock.Object, Mock.Of<IEntityChangeTrackingService>());
            var result = await service.Remove(member.Id);

            //Assert
            mock.Verify(x => x.Remove(member));
        }

        [Fact]
        public async void GetMemberByIdIsOk()
        {
            var member = new Member()
            {
                Id = 1,
                Name = "Rafael Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddressId = 1,
            };

            var memberDto = new MemberDto()
            {
                Id = 1,
                Name = "Rafael Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                //HomeAddressId = 1,
            };

            //mock
            var mock = new Mock<IMemberRepository>();
            var mapperMock = new Mock<IMapper>();

            mock.Setup(x => x.GetMemberByIdAsync(member.Id)).ReturnsAsync(member);
            mapperMock.Setup(x => x.Map<MemberDto>(member)).Returns(memberDto);

            //Act
            var service = new MemberService(mock.Object, mapperMock.Object, Mock.Of<IEntityChangeTrackingService>());
            var result = await service.GetMemberById(member.Id);

            //Assert
            mock.Verify(x => x.GetMemberByIdAsync(member.Id));
            mapperMock.Verify(x => x.Map<MemberDto>(member));
            result.Data.Should().Be(memberDto);
        }

        [Fact]
        public async void UpdateMemberIsOk()
        {
            var member = new Member()
            {
                Id = 1,
                Name = "Rafael Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddressId = 1,
            };

            var memberDto = new MemberDto()
            {
                Id = 1,
                Name = "Rafael Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
            };

            //mock
            var mock = new Mock<IMemberRepository>();
            var mapperMock = new Mock<IMapper>();

            mapperMock.Setup(x => x.Map<MemberDto>(member)).Returns(memberDto);
            mock.Setup(x => x.Update(It.IsAny<Member>())).ReturnsAsync(member);
            mapperMock.Setup(x => x.Map<Member>(memberDto)).Returns(member);

            //Act
            var service = new MemberService(mock.Object, mapperMock.Object, Mock.Of<IEntityChangeTrackingService>());
            var result = await service.Update(memberDto, member.Id);

            //Assert
            mock.Verify(x => x.Update(member));
        }
    }
}
