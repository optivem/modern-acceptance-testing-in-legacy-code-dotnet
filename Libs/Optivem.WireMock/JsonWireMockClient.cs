using System.Text.Json;
using System.Text.Json.Serialization;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Optivem.WireMock;

public class JsonWireMockClient
{
    private const string ContentType = "Content-Type";
    private const string ApplicationJson = "application/json";

    private readonly WireMockServer _server;
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonWireMockClient(WireMockServer server)
        : this(server, CreateDefaultJsonOptions())
    {
    }

    public JsonWireMockClient(WireMockServer server, JsonSerializerOptions jsonOptions)
    {
        _server = server;
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

    public void ConfigureGet<T>(string path, int statusCode, T response)
    {
        var responseBody = Serialize(response);

        _server
            .Given(Request.Create().WithPath(path).UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(statusCode)
                    .WithHeader(ContentType, ApplicationJson)
                    .WithBody(responseBody)
            );
    }

    private string Serialize(object obj)
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
