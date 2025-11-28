namespace Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;

public static class Closer
{
    public static void Close(IDisposable? client)
    {
        try
        {
            client?.Dispose();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error closing resource", ex);
        }
    }
}
