using G12App.Domain.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace G12App.API.Middleware
{
    public class ErrorMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorMiddleware> _logger;

        public ErrorMiddleware(ILogger<ErrorMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(context, ex);
            }
        }

        private async Task HandleErrorAsync(HttpContext context, Exception exception)
        {
            var result = new ServiceResult("An exception occurred while processing this request.")
            {
                Exception = exception,
            };

            _logger.LogCritical("Error: {exception.Message} | Stack: {stackTrace}", exception.Message, exception.StackTrace);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}
