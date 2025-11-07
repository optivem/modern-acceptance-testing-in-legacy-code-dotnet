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
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Validation Error",
                Detail = validationException.Message
            };

            httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }

        if (exception is JsonException jsonException)
        {
            _logger.LogDebug("JsonException: {Message}", jsonException.Message);

            var message = jsonException.Message ?? string.Empty;
            var lowerMessage = message.ToLower();

            // Check if it's related to the productId or quantity field
            string errorMessage;
            if (lowerMessage.Contains("productid") || lowerMessage.Contains("product_id"))
            {
                errorMessage = "Product ID must be an integer";
            }
            else if (lowerMessage.Contains("quantity"))
            {
                errorMessage = "Quantity must be an integer";
            }
            else
            {
                errorMessage = "Invalid request format";
            }

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = errorMessage
            };

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }

        return false;
    }
}
