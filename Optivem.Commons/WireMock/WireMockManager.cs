using WireMock.Server;

namespace Optivem.Commons.WireMock;

public class WireMockManager : IDisposable
{
    private readonly WireMockServer _server;

    public WireMockManager(string host, int port)
    {
        _server = WireMockServer.Start(port);
    }

    public WireMockManager(WireMockServer server)
    {
        _server = server;
    }

    public WireMockServer Server => _server;

    public void ResetAll()
    {
        _server.Reset();
    }

    public void Dispose()
    {
        ResetAll();
        _server?.Stop();
    }
}
