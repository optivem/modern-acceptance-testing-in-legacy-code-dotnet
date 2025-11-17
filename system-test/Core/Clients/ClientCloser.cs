namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients;

public static class ClientCloser
{
    public static async Task CloseAsync(IAsyncDisposable? client)
    {
        if (client != null)
        {
            await client.DisposeAsync();
        }
    }
}
