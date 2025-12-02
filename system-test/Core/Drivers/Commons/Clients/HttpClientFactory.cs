namespace Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;

public static class HttpClientFactory
{
    public static HttpClient Create(string baseUrl)
    {
        return new HttpClient { BaseAddress = new Uri(baseUrl) };
    }
}
