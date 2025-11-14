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
        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        var (statusCode, response) = exception switch
        {
            NotExistValidationException => (StatusCodes.Status404NotFound, null),
            ValidationException validationEx => (StatusCodes.Status400BadRequest, 
                new ErrorResponse(validationEx.Message)),
            _ => (StatusCodes.Status500InternalServerError, 
                new ErrorResponse($"Internal server error: {exception.Message}"))
        };

        httpContext.Response.StatusCode = statusCode;

        if (response != null)
        {
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        }

        return true;
    }

    public record ErrorResponse(string Message);
}
