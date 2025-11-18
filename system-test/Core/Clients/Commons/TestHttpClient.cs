using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;

public class TestHttpClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new global::System.Text.Json.Serialization.JsonStringEnumConverter() }
    };

    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public TestHttpClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl;
        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<HttpResponseMessage> GetAsync(string path)
    {
        return await _httpClient.GetAsync(path);
    }

    public async Task<HttpResponseMessage> PostAsync<T>(string path, T requestBody)
    {
        return await _httpClient.PostAsJsonAsync(path, requestBody);
    }

    public async Task<HttpResponseMessage> PostAsync(string path)
    {
        return await _httpClient.PostAsync(path, null);
    }

    public void AssertOk(HttpResponseMessage httpResponse)
    {
        AssertStatus(httpResponse, HttpStatusCode.OK);
    }

    public void AssertCreated(HttpResponseMessage httpResponse)
    {
        AssertStatus(httpResponse, HttpStatusCode.Created);
    }

    public void AssertNoContent(HttpResponseMessage httpResponse)
    {
        AssertStatus(httpResponse, HttpStatusCode.NoContent);
    }

    public void AssertUnprocessableEntity(HttpResponseMessage httpResponse)
    {
        AssertStatus(httpResponse, (HttpStatusCode)422);
    }

    private void AssertStatus(HttpResponseMessage httpResponse, HttpStatusCode expectedStatus)
    {
        var actualStatus = httpResponse.StatusCode;
        
        if (actualStatus != expectedStatus)
        {
            var responseBody = httpResponse.Content.ReadAsStringAsync().Result;
            Assert.Fail($"Expected status {(int)expectedStatus} but got {(int)actualStatus}. Response body: {responseBody}");
        }
    }

    public async Task<T> ReadBodyAsync<T>(HttpResponseMessage httpResponse)
    {
        var responseBody = await httpResponse.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseBody, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize response to {typeof(T).Name}");
    }
}
