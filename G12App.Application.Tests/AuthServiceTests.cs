using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentAssertions;
using G12App.Application.Services;
using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using Microsoft.Extensions.Options;
using Moq;

namespace G12App.Application.Tests
{
    public class AuthServiceTests
    {
        private const string AdminUserName = "admin";
        private const string AdminPassword = "adminpass";
        private static readonly JWTSettings JwtSettings = new() { SecretKey = "super-secret-key-value-123456789" };

        private static AuthService CreateService(Mock<IMemberService> memberServiceMock)
        {
            var adminUser = new AdminUser
            {
                UserName = AdminUserName,
                Password = AdminPassword
            };

            return new AuthService(
                memberServiceMock.Object,
                Options.Create(JwtSettings),
                Options.Create(adminUser));
        }

        [Fact]
        public async Task Login_WithAdminCredentials_ShouldReturnAdminResponse()
        {
            var memberServiceMock = new Mock<IMemberService>(MockBehavior.Strict);
            var service = CreateService(memberServiceMock);
            var login = new LoginDto { UserName = AdminUserName, Password = AdminPassword };

            var result = await service.Login(login);

            result.IsValid.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.MemberId.Should().Be(-1);
            result.Data.Name.Should().Be(nameof(AdminUser));
            result.Data.Role.Should().Be(Roles.AppSuperAdmin);
            result.Data.Token.Should().NotBeNullOrWhiteSpace();
            result.Data.UserName.Should().Be(AdminUserName);
            memberServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Login_WithMissingCredentials_ShouldReturnBadRequest()
        {
            var memberServiceMock = new Mock<IMemberService>(MockBehavior.Strict);
            var service = CreateService(memberServiceMock);
            var login = new LoginDto { UserName = string.Empty, Password = string.Empty };

            var result = await service.Login(login);

            result.IsValid.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Preencha o usuario e senha.");
            memberServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Login_WithUnknownUser_ShouldReturnBadRequest()
        {
            var login = new LoginDto { UserName = "someone", Password = "pwd" };
            var getMembersResult = new ServiceResult<IEnumerable<MemberDto>>(Enumerable.Empty<MemberDto>());

            var memberServiceMock = new Mock<IMemberService>();
            memberServiceMock
                .Setup(x => x.GetMembers(It.Is<Query<MemberFilter>>(q =>
                        q.Filter.UserName == login.UserName &&
                        q.Pagination.Page == 1 &&
                        q.Pagination.Limit == 1), true))
                .ReturnsAsync(getMembersResult);

            var service = CreateService(memberServiceMock);

            var result = await service.Login(login);

            result.IsValid.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Usuário não encontrado.");
            memberServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ShouldReturnBadRequest()
        {
            var login = new LoginDto { UserName = "user", Password = "wrong" };
            var user = new MemberDto
            {
                Id = 10,
                Name = "User Test",
                Password = "secret",
            };

            var memberServiceMock = new Mock<IMemberService>();
            memberServiceMock
                .Setup(x => x.GetMembers(It.IsAny<Query<MemberFilter>>(), true))
                .ReturnsAsync(new ServiceResult<IEnumerable<MemberDto>>(new[] { user }));

            var service = CreateService(memberServiceMock);

            var result = await service.Login(login);

            result.IsValid.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Senha incorreta.");
            memberServiceMock.Verify(x => x.GetMembers(It.IsAny<Query<MemberFilter>>(), true), Times.Once);
        }

        [Fact]
        public async Task Login_WithValidMember_ShouldReturnTokenAndMemberInfo()
        {
            var login = new LoginDto { UserName = "member", Password = "pass" };
            var user = new MemberDto
            {
                Id = 42,
                Name = "Member Test",
                Password = "pass",
                Role = Roles.AppAdmin,
                ComputedG12Leader = "Leader Name",
                ComputedG12LeaderSpouseId = 77,
                ComputedG12LeaderId = 99,
                SpouseId = 12,
            };

            var memberServiceMock = new Mock<IMemberService>();
            memberServiceMock
                .Setup(x => x.GetMembers(It.Is<Query<MemberFilter>>(q =>
                        q.Filter.UserName == login.UserName &&
                        q.Pagination.Page == 1 &&
                        q.Pagination.Limit == 1), true))
                .ReturnsAsync(new ServiceResult<IEnumerable<MemberDto>>(new[] { user }));

            var service = CreateService(memberServiceMock);

            var result = await service.Login(login);

            result.IsValid.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.MemberId.Should().Be(user.Id);
            result.Data.Name.Should().Be(user.Name);
            result.Data.Role.Should().Be(user.Role);
            result.Data.UserName.Should().Be(login.UserName);
            result.Data.G12LeadershipName.Should().Be(user.ComputedG12Leader);
            result.Data.SpouseId.Should().Be(user.SpouseId);
            result.Data.Token.Should().NotBeNullOrWhiteSpace();
            memberServiceMock.VerifyAll();
        }
    }
}
