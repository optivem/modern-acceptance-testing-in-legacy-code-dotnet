using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;

public class TestHttpClient
{
    private const string ContentType = "Content-Type";
    private const string ApplicationJson = "application/json";

    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public TestHttpClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl;
    }

    public HttpResponseMessage Get(string path)
    {
        var uri = GetUri(path);
        var response = _httpClient.GetAsync(uri).Result;
        return response;
    }

    public HttpResponseMessage Post(string path, object requestBody)
    {
        var uri = GetUri(path);
        var jsonBody = JsonSerializer.Serialize(requestBody, JsonOptions);
        var content = new StringContent(jsonBody, Encoding.UTF8, ApplicationJson);
        var response = _httpClient.PostAsync(uri, content).Result;
        return response;
    }

    public HttpResponseMessage Post(string path)
    {
        var uri = GetUri(path);
        var content = new StringContent("", Encoding.UTF8, ApplicationJson);
        var response = _httpClient.PostAsync(uri, content).Result;
        return response;
    }

    private Uri GetUri(string path)
    {
        return TestHttpUtils.GetUri(_baseUrl, path);
    }
}
