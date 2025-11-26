using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Erp;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Tax;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Api;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Ui;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients;

public static class ClientFactory
{
    public static ShopUiClient CreateShopUiClient()
    {
        return new ShopUiClient(TestConfiguration.ShopUiBaseUrl);
    }

    public static ShopApiClient CreateShopApiClient()
    {
        return new ShopApiClient(TestConfiguration.ShopApiBaseUrl);
    }

    public static ErpApiClient CreateErpApiClient()
    {
        return new ErpApiClient(TestConfiguration.ErpApiBaseUrl);
    }

    public static TaxApiClient CreateTaxApiClient()
    {
        return new TaxApiClient(TestConfiguration.TaxApiBaseUrl);
    }
}
