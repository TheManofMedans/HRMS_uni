using System.Net;
using HRMS.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace HRMS.API.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
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
                await HandleExceptionAsync(context,ex);
            }

        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var (StatusCode, title) = ex switch
            {
                NotFoundException => (HttpStatusCode.NotFound, "Resource Not Found"),
                ConflictException => (HttpStatusCode.Conflict, "Conflict"),
                RepeatDataException => (HttpStatusCode.Conflict,"Unauthorized Repeated Data"),
                ValidationException => (HttpStatusCode.BadRequest,"Invalid Request"),
                _ => (HttpStatusCode.InternalServerError,"An unexpected Error Occured!")
            };
            if (StatusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(ex, "Unhandled Exception on {Method} {path}", context.Request.Method, context.Request.Path);
            }
            else
            {
                _logger.LogInformation("{ExceptionType} {Message}",ex.GetType().Name,ex.Message);
            }
            var problemDetails = new ProblemDetails
            {
                Status = (int)StatusCode,
                Title = title,
                Detail = StatusCode == HttpStatusCode.InternalServerError ? "An unexpected error occurred. Please try again later." :
                ex.Message,
                Instance = context.Request.Path
            };
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = problemDetails.Status.Value;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
