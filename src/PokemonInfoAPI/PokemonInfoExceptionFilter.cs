using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;

namespace PokemonInfoAPI
{
	public class PokemonInfoExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<PokemonInfoExceptionFilterAttribute> _logger;

        public PokemonInfoExceptionFilterAttribute(ILogger<PokemonInfoExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext actionExecutedContext)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            var message = string.Empty;

            var exceptionType = actionExecutedContext.Exception.GetType();
            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = "Access to the Pokemon API is not authorized.";
                status = HttpStatusCode.Unauthorized;
            }
            else
            {
                message = "Unhandled exception occurred while executing request";
                status = HttpStatusCode.InternalServerError;
            }
            actionExecutedContext.Result = new JsonResult( new
            {
                Content = new StringContent(message, System.Text.Encoding.UTF8, "text/plain"),
                StatusCode = status
            });

            // Log the exception
            _logger.LogError("Unhandled exception occurred while executing request: {ex}", actionExecutedContext.Exception);

            actionExecutedContext.ExceptionHandled = true;
            base.OnException(actionExecutedContext);
        }
    }
}
