using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Commons.Util;

namespace Commons.Http;

public class JsonHttpClient<E>
{
    private const string ApplicationJson = "application/json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false) }
    };

    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public JsonHttpClient(string baseUrl)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
        _baseUrl = baseUrl;
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    public Result<T, E> Get<T>(string path)
    {
        var httpResponse = DoGet(path);
        return GetResultOrFailure<T>(httpResponse);
    }

    public Result<VoidValue, E> Get(string path)
    {
        var httpResponse = DoGet(path);
        return GetResultOrFailure<VoidValue>(httpResponse);
    }

    public Result<T, E> Post<T>(string path, object request)
    {
        var httpResponse = DoPost(path, request);
        return GetResultOrFailure<T>(httpResponse);
    }

    public Result<VoidValue, E> Post(string path, object request)
    {
        var httpResponse = DoPost(path, request);
        return GetResultOrFailure<VoidValue>(httpResponse);
    }

    public Result<VoidValue, E> Post(string path)
    {
        var httpResponse = DoPost(path);
        return GetResultOrFailure<VoidValue>(httpResponse);
    }

    public Result<T, E> Put<T>(string path, object request)
    {
        var httpResponse = DoPut(path, request);
        return GetResultOrFailure<T>(httpResponse);
    }

    public Result<VoidValue, E> Put(string path, object request)
    {
        var httpResponse = DoPut(path, request);
        return GetResultOrFailure<VoidValue>(httpResponse);
    }

    public Result<T, E> Delete<T>(string path)
    {
        var httpResponse = DoDelete(path);
        return GetResultOrFailure<T>(httpResponse);
    }

    public Result<VoidValue, E> Delete(string path)
    {
        var httpResponse = DoDelete(path);
        return GetResultOrFailure<VoidValue>(httpResponse);
    }

    private HttpResponseMessage DoGet(string path)
    {
        var uri = GetUri(path);
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);
        return SendRequest(httpRequest);
    }

    #region Helpers

    private Uri GetUri(string path)
    {
        return new Uri(_baseUrl + path);
    }

    private HttpResponseMessage DoPost(string path, object request)
    {
        var uri = GetUri(path);
        var jsonBody = SerializeRequest(request);
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(jsonBody, Encoding.UTF8, ApplicationJson)
        };
        return SendRequest(httpRequest);
    }

    private HttpResponseMessage DoPost(string path)
    {
        var uri = GetUri(path);
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(string.Empty, Encoding.UTF8, ApplicationJson)
        };
        return SendRequest(httpRequest);
    }

    private HttpResponseMessage DoPut(string path, object request)
    {
        var uri = GetUri(path);
        var jsonBody = SerializeRequest(request);
        var httpRequest = new HttpRequestMessage(HttpMethod.Put, uri)
        {
            Content = new StringContent(jsonBody, Encoding.UTF8, ApplicationJson)
        };
        return SendRequest(httpRequest);
    }

    private HttpResponseMessage DoDelete(string path)
    {
        var uri = GetUri(path);
        var httpRequest = new HttpRequestMessage(HttpMethod.Delete, uri);
        return SendRequest(httpRequest);
    }

    private HttpResponseMessage SendRequest(HttpRequestMessage httpRequest)
    {
        return _httpClient.Send(httpRequest);
    }

    private static string SerializeRequest(object request)
    {
        return JsonSerializer.Serialize(request, JsonOptions);
    }

    private T ReadResponse<T>(HttpResponseMessage httpResponse)
    {
        var responseBody = httpResponse.Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<T>(responseBody, JsonOptions)!;
    }

    private Result<T, E> GetResultOrFailure<T>(HttpResponseMessage httpResponse)
    {
        if (!httpResponse.IsSuccessStatusCode)
        {
            var error = ReadResponse<E>(httpResponse);
            return Result<T, E>.Failure(error);
        }

        if (typeof(T) == typeof(VoidValue) || httpResponse.StatusCode == HttpStatusCode.NoContent)
        {
            return Result<T, E>.Success(default!);
        }

        var response = ReadResponse<T>(httpResponse);
        return Result<T, E>.Success(response);
    }

    #endregion
}
