using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Optivem.AtddAccelerator.EShop.Monolith.Core.Exceptions;
using System.Text.Json;

namespace Optivem.AtddAccelerator.EShop.Monolith.Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails;

        switch (exception)
        {
            case ValidationException validationEx:
                problemDetails = new ProblemDetails
                {
                    Type = "https://api.optivem.com/errors/validation-error",
                    Title = "Validation Error",
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Detail = validationEx.Message
                };
                problemDetails.Extensions["timestamp"] = DateTime.UtcNow;
                httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                break;

            case NotExistValidationException notExistEx:
                problemDetails = new ProblemDetails
                {
                    Type = "https://api.optivem.com/errors/resource-not-found",
                    Title = "Resource Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = notExistEx.Message
                };
                problemDetails.Extensions["timestamp"] = DateTime.UtcNow;
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                break;

            default:
                _logger.LogError(exception, "Unexpected error occurred");
                problemDetails = new ProblemDetails
                {
                    Type = "https://api.optivem.com/errors/internal-server-error",
                    Title = "Internal Server Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = $"Internal server error: {exception.Message}"
                };
                problemDetails.Extensions["timestamp"] = DateTime.UtcNow;
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
