namespace Optivem.EShop.SystemTest.Core.Common;

public class SystemConfiguration
{
    private readonly string shopUiBaseUrl;
    private readonly string shopApiBaseUrl;
    private readonly string erpBaseUrl;
    private readonly string taxBaseUrl;

    public SystemConfiguration(string shopUiBaseUrl, string shopApiBaseUrl, string erpBaseUrl, string taxBaseUrl)
    {
        this.shopUiBaseUrl = shopUiBaseUrl;
        this.shopApiBaseUrl = shopApiBaseUrl;
        this.erpBaseUrl = erpBaseUrl;
        this.taxBaseUrl = taxBaseUrl;
    }

    public string ShopUiBaseUrl => shopUiBaseUrl;
    public string ShopApiBaseUrl => shopApiBaseUrl;
    public string ErpBaseUrl => erpBaseUrl;
    public string TaxBaseUrl => taxBaseUrl;
}
