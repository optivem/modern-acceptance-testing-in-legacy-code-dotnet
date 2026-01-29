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

    public Task<Result<VoidValue, ExtErpErrorResponse>> CheckHealth()
        => HttpClient.Get("/health");

    public Task<Result<ExtProductDetailsResponse, ExtErpErrorResponse>> GetProduct(string sku)
        => HttpClient.Get<ExtProductDetailsResponse>($"/api/products/{sku}");
}