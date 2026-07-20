using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManagement.Application.Exceptions;

namespace TaskManagement.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var (statusCode, message) = context.Exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, context.Exception.Message),
                BadRequestException => (StatusCodes.Status400BadRequest, context.Exception.Message),
                UnauthorizedException => (StatusCodes.Status401Unauthorized, context.Exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };

            context.Result = new ObjectResult(new { error = message })
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
