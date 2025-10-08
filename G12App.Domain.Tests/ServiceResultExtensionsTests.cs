using System.Net;
using FluentAssertions;
using G12App.Domain.Extensions;
using G12App.Domain.Messages;
using Microsoft.AspNetCore.Mvc;

namespace G12App.Domain.Tests
{
    public class ServiceResultExtensionsTests
    {
        [Fact]
        public void AsActionResult_ShouldReturnNoContent_WhenServiceResultIsNull()
        {
            ServiceResult serviceResult = null;

            var result = serviceResult.AsActionResult();

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void AsActionResult_ShouldRespectExplicitStatusCode()
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.Accepted };

            var result = serviceResult.AsActionResult();

            var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
            objectResult.Value.Should().BeSameAs(serviceResult);
        }

        [Fact]
        public void AsActionResult_ShouldReturnOkForValidResults()
        {
            var serviceResult = new ServiceResult<string>("value");

            var result = serviceResult.AsActionResult();

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeSameAs(serviceResult);
        }

        [Fact]
        public void AsActionResult_ShouldReturnBadRequestForInvalidResults()
        {
            var serviceResult = new ServiceResult<string>(null, new[] { new Error("error") });

            var result = serviceResult.AsActionResult();

            var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequest.Value.Should().BeSameAs(serviceResult);
        }
    }
}
