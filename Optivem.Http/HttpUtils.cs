using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Optivem.Results;

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

    public static List<string> GetErrorMessages(HttpResponseMessage httpResponse)
    {
        try
        {
            var problemDetail = ReadResponse<ProblemDetailResponse>(httpResponse);
            var errorMessages = new List<string>();

            if (problemDetail.Errors != null && problemDetail.Errors.Any())
            {
                errorMessages.AddRange(problemDetail.Errors.Select(e => e.Message ?? string.Empty));
            }
            else if (!string.IsNullOrEmpty(problemDetail.Detail))
            {
                errorMessages.Add(problemDetail.Detail);
            }

            return errorMessages;
        }
        catch
        {
            return new List<string>();
        }
    }

    public static Result<T> GetOkResultOrFailure<T>(HttpResponseMessage httpResponse)
    {
        return GetResultOrFailure<T>(httpResponse, HttpStatusCode.OK);
    }

    public static Result<VoidResult> GetOkResultOrFailure(HttpResponseMessage httpResponse)
    {
        return GetResultOrFailure(httpResponse, HttpStatusCode.OK);
    }

    public static Result<T> GetCreatedResultOrFailure<T>(HttpResponseMessage httpResponse)
    {
        return GetResultOrFailure<T>(httpResponse, HttpStatusCode.Created);
    }

    public static Result<VoidResult> GetCreatedResultOrFailure(HttpResponseMessage httpResponse)
    {
        return GetResultOrFailure(httpResponse, HttpStatusCode.Created);
    }

    public static Result<VoidResult> GetNoContentResultOrFailure(HttpResponseMessage httpResponse)
    {
        var isSuccess = HasStatusCode(httpResponse, HttpStatusCode.NoContent);

        if (!isSuccess)
        {
            var errorMessages = GetErrorMessages(httpResponse);
            return Result.Failure(errorMessages);
        }

        return Result.Success();
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

    private static Result<T> GetResultOrFailure<T>(HttpResponseMessage httpResponse, HttpStatusCode successStatus)
    {
        var isSuccess = HasStatusCode(httpResponse, successStatus);

        if (!isSuccess)
        {
            var errorMessages = GetErrorMessages(httpResponse);
            return Result<T>.FailureResult(errorMessages);
        }

        var response = ReadResponse<T>(httpResponse);
        return Result<T>.SuccessResult(response);
    }

    private static Result<VoidResult> GetResultOrFailure(HttpResponseMessage httpResponse, HttpStatusCode successStatus)
    {
        var isSuccess = HasStatusCode(httpResponse, successStatus);

        if (!isSuccess)
        {
            var errorMessages = GetErrorMessages(httpResponse);
            return Result.Failure(errorMessages);
        }

        return Result.Success();
    }
}
