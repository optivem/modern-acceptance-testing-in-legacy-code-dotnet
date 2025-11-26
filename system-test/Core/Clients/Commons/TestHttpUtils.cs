using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Dtos;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Results;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;

public static class TestHttpUtils
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static T ReadResponse<T>(HttpResponseMessage httpResponse)
    {
        var responseBody = httpResponse.Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<T>(responseBody, JsonOptions)!;
    }

    public static List<string> GetErrorMessages(HttpResponseMessage httpResponse)
    {
        var problemDetail = ReadResponse<ProblemDetailResponse>(httpResponse);
        var errors = new List<string>();

        if (problemDetail.Detail != null)
        {
            errors.Add(problemDetail.Detail);
        }

        if (problemDetail.Errors != null)
        {
            foreach (var error in problemDetail.Errors)
            {
                if (error.Message != null)
                {
                    errors.Add(error.Message);
                }
            }
        }

        return errors;
    }

    public static Result<T> GetOkResultOrFailure<T>(HttpResponseMessage httpResponse)
    {
        return GetResultOrFailure<T>(httpResponse, HttpStatusCode.OK);
    }

    public static Result<object?> GetOkResultOrFailure(HttpResponseMessage httpResponse)
    {
        return GetResultOrFailure(httpResponse, HttpStatusCode.OK);
    }

    public static Result<T> GetCreatedResultOrFailure<T>(HttpResponseMessage httpResponse)
    {
        return GetResultOrFailure<T>(httpResponse, HttpStatusCode.Created);
    }

    public static Result<object?> GetCreatedResultOrFailure(HttpResponseMessage httpResponse)
    {
        return GetResultOrFailure(httpResponse, HttpStatusCode.Created);
    }

    public static Result<object?> GetNoContentResultOrFailure(HttpResponseMessage httpResponse)
    {
        var isSuccess = HasStatusCode(httpResponse, HttpStatusCode.NoContent);

        if (!isSuccess)
        {
            var errorMessages = GetErrorMessages(httpResponse);
            return Result.Failure<object?>(errorMessages);
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
            throw new Exception($"Failed to create URI for path: {path}", ex);
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
            return Result.Failure<T>(errorMessages);
        }

        var response = ReadResponse<T>(httpResponse);
        return Result.Success(response);
    }

    private static Result<object?> GetResultOrFailure(HttpResponseMessage httpResponse, HttpStatusCode successStatus)
    {
        var isSuccess = HasStatusCode(httpResponse, successStatus);

        if (!isSuccess)
        {
            var errorMessages = GetErrorMessages(httpResponse);
            return Result.Failure<object?>(errorMessages);
        }

        return Result.Success();
    }
}
