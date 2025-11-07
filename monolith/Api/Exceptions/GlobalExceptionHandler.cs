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

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is NotExistValidationException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return true;
        }

        if (exception is ValidationException validationException)
        {
            var errorResponse = new { Message = validationException.Message };
            httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
            return true;
        }

        if (exception is JsonException jsonException)
        {
            _logger.LogDebug("JsonException: {Message}", jsonException.Message);

            var errorResponse = TryParseFieldError(jsonException.Message);
            if (errorResponse != null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
                return true;
            }

            // Fallback to generic error
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new { Message = "Invalid request format" }, cancellationToken);
            return true;
        }

        return false;
    }

    private object? TryParseFieldError(string? message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return null;
        }

        var lowerMessage = message.ToLower();

        // Map field names to error messages
        var fieldErrorMessages = new Dictionary<string, string>
        {
            { "productid", "Product ID must be an integer" },
            { "quantity", "Quantity must be an integer" }
        };

        foreach (var (fieldName, errorMessage) in fieldErrorMessages)
        {
            if (lowerMessage.Contains(fieldName))
            {
                return new { Message = errorMessage };
            }
        }

        return null;
    }
}
