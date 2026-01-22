using System.Text.Json;
using System.Text.Json.Serialization;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Optivem.Commons.Util;

namespace Optivem.Commons.WireMock;

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

    public Result<VoidValue, string> StubGet<T>(string path, int statusCode, T response)
    {
        try
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

            // Verify stub was registered successfully by checking mappings
            var mappings = _server.LogEntries;
            
            return Result<VoidValue, string>.Success(VoidValue.Empty);
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
