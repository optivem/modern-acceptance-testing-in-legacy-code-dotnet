using System.Net.Http.Headers;
using System.Text;

namespace Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;

public class HttpGateway
{
    private const string ContentType = "Content-Type";
    private const string ApplicationJson = "application/json";

    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public HttpGateway(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl;
    }

    public HttpResponseMessage Get(string path)
    {
        var uri = GetUri(path);
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        return SendRequest(request);
    }

    public HttpResponseMessage Post(string path, object requestBody)
    {
        var uri = GetUri(path);
        var jsonBody = HttpUtils.SerializeRequest(requestBody);

        var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(jsonBody, Encoding.UTF8, ApplicationJson)
        };

        return SendRequest(request);
    }

    public HttpResponseMessage Post(string path)
    {
        var uri = GetUri(path);

        var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(string.Empty, Encoding.UTF8, ApplicationJson)
        };

        return SendRequest(request);
    }

    private Uri GetUri(string path)
    {
        return HttpUtils.GetUri(_baseUrl, path);
    }

    private HttpResponseMessage SendRequest(HttpRequestMessage httpRequest)
    {
        try
        {
            return _httpClient.Send(httpRequest);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to send HTTP request", ex);
        }
    }
}
