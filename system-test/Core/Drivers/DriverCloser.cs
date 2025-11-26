namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers;

public static class DriverCloser
{
    public static void Close(IDisposable? driver)
    {
        driver?.Dispose();
    }
}
