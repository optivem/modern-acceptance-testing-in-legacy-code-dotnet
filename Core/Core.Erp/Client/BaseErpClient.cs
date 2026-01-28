using Commons.Http;
using Commons.Util;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Client;

public abstract class BaseErpClient : IDisposable
{
    protected readonly JsonHttpClient<ExtErpErrorResponse> HttpClient;

    protected BaseErpClient(string baseUrl)
    {
        HttpClient = new JsonHttpClient<ExtErpErrorResponse>(baseUrl);
    }

    public void Dispose()
    {
        HttpClient?.Dispose();
    }

    public Result<VoidValue, ExtErpErrorResponse> CheckHealth()
    {
        return HttpClient.Get("/health");
    }

    public Result<ExtProductDetailsResponse, ExtErpErrorResponse> GetProduct(string? sku)
    {
        return HttpClient.Get<ExtProductDetailsResponse>($"/api/products/{sku}");
    }
}