using G12App.Domain.Messages;
using Microsoft.AspNetCore.Mvc;

namespace G12App.Domain.Extensions
{
    public static class ServiceResultExtensions
    {
        public static IActionResult AsActionResult(this ServiceResult serviceResult)
        {
            if (serviceResult == null)
                return new NoContentResult();

            if (serviceResult.StatusCode.HasValue)
            {
                var result = new ObjectResult(serviceResult);
                result.StatusCode = (int)serviceResult.StatusCode;
                return result;
            }

            return serviceResult.IsValid ? new OkObjectResult(serviceResult) : new BadRequestObjectResult(serviceResult);
        }
    }
}
