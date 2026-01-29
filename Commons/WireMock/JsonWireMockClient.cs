using System.Text.Json;
using System.Text.Json.Serialization;
using Commons.Util;

namespace Commons.WireMock;

public class JsonWireMockClient : IDisposable
{
    private const string ContentType = "Content-Type";
    private const string ApplicationJson = "application/json";

    private readonly HttpClient _httpClient;
    private readonly string _wireMockBaseUrl;
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonWireMockClient(string baseUrl)
        : this(baseUrl, CreateDefaultJsonOptions())
    {
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    private JsonWireMockClient(string baseUrl, JsonSerializerOptions jsonOptions)
    {
        var uri = new Uri(baseUrl);
        _wireMockBaseUrl = $"http://{uri.Host}:{uri.Port}";
        _httpClient = new HttpClient()
        {
            BaseAddress = uri,
        };

        _jsonOptions = jsonOptions;
    }

    private static JsonSerializerOptions CreateDefaultJsonOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        return options;
    }

    public async Task<Result<VoidValue, string>> StubGetAsync<T>(string path, int statusCode, T response)
    {
        try
        {
            var responseBody = Serialize(response);

            var mappingRequest = new
            {
                request = new
                {
                    method = "GET",
                    urlPath = path
                },
                response = new
                {
                    status = statusCode,
                    headers = new Dictionary<string, string>
                    {
                        { ContentType, ApplicationJson }
                    },
                    body = responseBody
                }
            };

            var requestJson = JsonSerializer.Serialize(mappingRequest, _jsonOptions);
            var content = new StringContent(requestJson, System.Text.Encoding.UTF8, ApplicationJson);

            var apiResponse = await _httpClient.PostAsync($"{_wireMockBaseUrl}/__admin/mappings", content);
            
            if (apiResponse.IsSuccessStatusCode)
            {
                return Result<VoidValue, string>.Success(VoidValue.Empty);
            }
            else
            {
                var errorContent = await apiResponse.Content.ReadAsStringAsync();
                return Result<VoidValue, string>.Failure($"Failed to register stub for GET {path}: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            return Result<VoidValue, string>.Failure($"Failed to configure GET stub for {path}: {ex.Message}");
        }
    }

    private string Serialize<T>(T obj)
    {
        try
        {
            return JsonSerializer.Serialize(obj, _jsonOptions);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to serialize object", ex);
        }
    }
}
