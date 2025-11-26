namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients;

public static class ClientCloser
{
    public static async Task CloseAsync<T>(T client) where T : IAsyncDisposable
    {
        if (client != null)
        {
            await client.DisposeAsync();
        }
    }

    public static void Close<T>(T client) where T : IDisposable
    {
        client?.Dispose();
    }
}
