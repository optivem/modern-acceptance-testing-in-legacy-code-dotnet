using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Commons.Util;

namespace Commons.Http;

public class JsonHttpClient<E> : IDisposable
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

    public async Task<Result<T, E>> Get<T>(string path)
    {
        var httpResponse = await DoGet(path);
        return await GetResultOrFailure<T>(httpResponse);
    }

    public async Task<Result<VoidValue, E>> Get(string path)
    {
        var httpResponse = await DoGet(path);
        return await GetResultOrFailure<VoidValue>(httpResponse);
    }

    public async Task<Result<T, E>> Post<T>(string path, object request)
    {
        var httpResponse = await DoPost(path, request);
        return await GetResultOrFailure<T>(httpResponse);
    }

    public async Task<Result<VoidValue, E>> Post(string path, object request)
    {
        var httpResponse = await DoPost(path, request);
        return await GetResultOrFailure<VoidValue>(httpResponse);
    }

    public async Task<Result<VoidValue, E>> Post(string path)
    {
        var httpResponse = await DoPost(path);
        return await GetResultOrFailure<VoidValue>(httpResponse);
    }

    public async Task<Result<T, E>> Put<T>(string path, object request)
    {
        var httpResponse = await DoPut(path, request);
        return await GetResultOrFailure<T>(httpResponse);
    }

    public async Task<Result<VoidValue, E>> Put(string path, object request)
    {
        var httpResponse = await DoPut(path, request);
        return await GetResultOrFailure<VoidValue>(httpResponse);
    }

    public async Task<Result<T, E>> Delete<T>(string path)
    {
        var httpResponse = await DoDelete(path);
        return await GetResultOrFailure<T>(httpResponse);
    }

    public async Task<Result<VoidValue, E>> Delete(string path)
    {
        var httpResponse = await DoDelete(path);
        return await GetResultOrFailure<VoidValue>(httpResponse);
    }

    private async Task<HttpResponseMessage> DoGet(string path)
    {
        var uri = GetUri(path);
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);
        return await SendRequest(httpRequest);
    }

    #region Helpers

    private Uri GetUri(string path)
    {
        return new Uri(_baseUrl + path);
    }

    private async Task<HttpResponseMessage> DoPost(string path, object request)
    {
        var uri = GetUri(path);
        var jsonBody = SerializeRequest(request);
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(jsonBody, Encoding.UTF8, ApplicationJson)
        };
        return await SendRequest(httpRequest);
    }

    private async Task<HttpResponseMessage> DoPost(string path)
    {
        var uri = GetUri(path);
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(string.Empty, Encoding.UTF8, ApplicationJson)
        };
        return await SendRequest(httpRequest);
    }

    private async Task<HttpResponseMessage> DoPut(string path, object request)
    {
        var uri = GetUri(path);
        var jsonBody = SerializeRequest(request);
        var httpRequest = new HttpRequestMessage(HttpMethod.Put, uri)
        {
            Content = new StringContent(jsonBody, Encoding.UTF8, ApplicationJson)
        };
        return await SendRequest(httpRequest);
    }

    private async Task<HttpResponseMessage> DoDelete(string path)
    {
        var uri = GetUri(path);
        var httpRequest = new HttpRequestMessage(HttpMethod.Delete, uri);
        return await SendRequest(httpRequest);
    }

    private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage httpRequest)
    {
        return await _httpClient.SendAsync(httpRequest);
    }

    private static string SerializeRequest(object request)
    {
        return JsonSerializer.Serialize(request, JsonOptions);
    }

    private async Task<T> ReadResponse<T>(HttpResponseMessage httpResponse)
    {
        var responseBody = await httpResponse.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseBody, JsonOptions)!;
    }

    private async Task<Result<T, E>> GetResultOrFailure<T>(HttpResponseMessage httpResponse)
    {
        if (!httpResponse.IsSuccessStatusCode)
        {
            var error = await ReadResponse<E>(httpResponse);
            return Result<T, E>.Failure(error);
        }

        if (typeof(T) == typeof(VoidValue) || httpResponse.StatusCode == HttpStatusCode.NoContent)
        {
            return Result<T, E>.Success(default!);
        }

        var response = await ReadResponse<T>(httpResponse);
        return Result<T, E>.Success(response);
    }

    #endregion
}
