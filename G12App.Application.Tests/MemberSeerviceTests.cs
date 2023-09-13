using Moq;
using G12App.Domain.Interfaces.Repositories;
using AutoMapper;
using G12App.Domain.Model;
using G12App.Domain.DTOs;
using G12App.Application.Services;
using FluentAssertions;

namespace G12App.Application.Tests
{
    public class MemberSeerviceTests
    {
        [Fact]
        public async void AddMemberSeerviceIsOk()
        {
            var member = new Member()
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

            var memberDto = new MemberDto()
            {
                Id = 1,
                Name = "Rafael",
                Surname = "Jorge Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                //HomeAddressId = 1,
                MemberType = Domain.Model.Enum.MemberType.Leadership,
            };

            //mock
            var mock = new Mock<IMemberRepository>();
            var mapperMock = new Mock<IMapper>();

            mock.Setup(x => x.Create(It.IsAny<Member>())).ReturnsAsync(member);
            mapperMock.Setup(x => x.Map<Member>(memberDto)).Returns(member);
            mapperMock.Setup(x => x.Map<MemberDto>(member)).Returns(memberDto);

            //Act
            var service = new MemberService(mock.Object, mapperMock.Object);
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
                Name = "Rafael",
                Surname = "Jorge Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddressId = 1,
                MemberType = Domain.Model.Enum.MemberType.Leadership,
            };

            var memberDto = new MemberDto()
            {
                Id = 1,
                Name = "Rafael",
                Surname = "Jorge Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                //HomeAddressId = 1,
                MemberType = Domain.Model.Enum.MemberType.Leadership,
            };
            //mock
            var mock = new Mock<IMemberRepository>();
            var mapperMock = new Mock<IMapper>();

            mock.Setup(x => x.GetMemberByIdAsync(member.Id)).ReturnsAsync(member);
            mock.Setup(x => x.Remove(It.IsAny<Member>())).ReturnsAsync(member);
            mapperMock.Setup(x => x.Map<MemberDto>(member)).Returns(memberDto);

            //Act
            var service = new MemberService(mock.Object, mapperMock.Object);
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
                Name = "Rafael",
                Surname = "Jorge Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddressId = 1,
                MemberType = Domain.Model.Enum.MemberType.Leadership,
            };

            var memberDto = new MemberDto()
            {
                Id = 1,
                Name = "Rafael",
                Surname = "Jorge Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                //HomeAddressId = 1,
                MemberType = Domain.Model.Enum.MemberType.Leadership,
            };

            //mock
            var mock = new Mock<IMemberRepository>();
            var mapperMock = new Mock<IMapper>();

            mock.Setup(x => x.GetMemberByIdAsync(member.Id)).ReturnsAsync(member);
            mapperMock.Setup(x => x.Map<MemberDto>(member)).Returns(memberDto);

            //Act
            var service = new MemberService(mock.Object, mapperMock.Object);
            var result = await service.GetMemberById(member.Id);

            //Assert
            mock.Verify(x => x.GetMemberByIdAsync(member.Id));
            mapperMock.Verify(x => x.Map<MemberDto>(member));
            result.Data.Should().Be(memberDto);
        }
    }
}
