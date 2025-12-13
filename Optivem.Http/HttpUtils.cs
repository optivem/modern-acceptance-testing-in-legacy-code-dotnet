using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Optivem.Lang;

namespace Optivem.Http;

public static class HttpUtils
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false) }
    };

    public static T ReadResponse<T>(HttpResponseMessage httpResponse)
    {
        try
        {
            var responseBody = httpResponse.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<T>(responseBody, JsonOptions)!;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to deserialize response to {typeof(T).Name}", ex);
        }
    }

    public static string SerializeRequest(object request)
    {
        try
        {
            return JsonSerializer.Serialize(request, JsonOptions);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to serialize request object", ex);
        }
    }

    public static Error GetError(HttpResponseMessage httpResponse)
    {
        try
        {
            var problemDetail = ReadResponse<ProblemDetailResponse>(httpResponse);
            var message = problemDetail.Detail ?? "Request failed";

            if (problemDetail.Errors != null && problemDetail.Errors.Any())
            {
                var fieldErrors = problemDetail.Errors
                    .Select(e => new Error.FieldError(e.Field ?? "unknown", e.Message ?? string.Empty, e.Code))
                    .ToList();
                return Error.Of(message, fieldErrors.AsReadOnly());
            }

            return Error.Of(message);
        }
        catch
        {
            return Error.Of("Request failed");
        }
    }

    public static Result<T, Error> GetOkResultOrFailure<T>(HttpResponseMessage httpResponse)
    {
        return GetResultOrFailure<T>(httpResponse, HttpStatusCode.OK);
    }

    public static Result<VoidValue, Error> GetOkResultOrFailure(HttpResponseMessage httpResponse)
    {
        return GetResultOrFailure(httpResponse, HttpStatusCode.OK);
    }

    public static Result<T, Error> GetCreatedResultOrFailure<T>(HttpResponseMessage httpResponse)
    {
        return GetResultOrFailure<T>(httpResponse, HttpStatusCode.Created);
    }

    public static Result<VoidValue, Error> GetCreatedResultOrFailure(HttpResponseMessage httpResponse)
    {
        return GetResultOrFailure(httpResponse, HttpStatusCode.Created);
    }

    public static Result<VoidValue, Error> GetNoContentResultOrFailure(HttpResponseMessage httpResponse)
    {
        var isSuccess = HasStatusCode(httpResponse, HttpStatusCode.NoContent);

        if (!isSuccess)
        {
            var error = GetError(httpResponse);
            return Results.Failure<VoidValue>(error);
        }

        return Results.Success();
    }

    public static Uri GetUri(string baseUrl, string path)
    {
        try
        {
            return new Uri(baseUrl + path);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create URI for path: {path}", ex);
        }
    }

    private static bool HasStatusCode(HttpResponseMessage httpResponse, HttpStatusCode statusCode)
    {
        return httpResponse.StatusCode == statusCode;
    }

    private static Result<T, Error> GetResultOrFailure<T>(HttpResponseMessage httpResponse, HttpStatusCode successStatus)
    {
        var isSuccess = HasStatusCode(httpResponse, successStatus);

        if (!isSuccess)
        {
            var error = GetError(httpResponse);
            return Results.Failure<T>(error);
        }

        var response = ReadResponse<T>(httpResponse);
        return Results.Success(response);
    }

    private static Result<VoidValue, Error> GetResultOrFailure(HttpResponseMessage httpResponse, HttpStatusCode successStatus)
    {
        var isSuccess = HasStatusCode(httpResponse, successStatus);

        if (!isSuccess)
        {
            var error = GetError(httpResponse);
            return Results.Failure<VoidValue>(error);
        }

        return Results.Success();
    }
}
