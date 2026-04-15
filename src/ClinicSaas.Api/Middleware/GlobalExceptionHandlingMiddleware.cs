using ClinicSaas.Api.Contracts;
using System.Net;

namespace ClinicSaas.Api.Middleware;

public sealed class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception.");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var payload = ApiResponseFactory.Failure("An unexpected error occurred.");
            await context.Response.WriteAsJsonAsync(payload);
        }
    }
}
