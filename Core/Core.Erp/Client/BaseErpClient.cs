using Optivem.Commons.Http;
using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Client;

public abstract class BaseErpClient : IDisposable
{
    protected readonly JsonHttpClient<ExtErpErrorResponse> HttpClient;
    private readonly HttpClient _rawHttpClient;

    protected BaseErpClient(string baseUrl)
    {
        _rawHttpClient = HttpClientFactory.Create(baseUrl);
        HttpClient = new JsonHttpClient<ExtErpErrorResponse>(_rawHttpClient, baseUrl);
    }

    public void Dispose()
    {
        _rawHttpClient?.Dispose();
    }

    public Result<VoidValue, ExtErpErrorResponse> CheckHealth()
    {
        return HttpClient.Get("/health");
    }


}