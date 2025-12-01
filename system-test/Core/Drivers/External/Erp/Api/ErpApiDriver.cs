using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api.Client;

namespace Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;

public class ErpApiDriver : IDisposable
{
    private readonly ErpApiClient _erpApiClient;

    public ErpApiDriver(string baseUrl)
    {
        _erpApiClient = new ErpApiClient(baseUrl);
    }

    public Result<VoidResult> GoToErp()
    {
        return _erpApiClient.Health.CheckHealth();
    }

    public Result<VoidResult> CreateProduct(string sku, string price)
    {
        return _erpApiClient.Products.CreateProduct(sku, price);
    }

    public void Dispose()
    {
        _erpApiClient?.Dispose();
    }
}
