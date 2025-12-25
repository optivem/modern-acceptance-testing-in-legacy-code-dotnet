namespace Optivem.EShop.SystemTest.Core.Common;

public class SystemConfiguration
{
    private readonly string shopUiBaseUrl;
    private readonly string shopApiBaseUrl;
    private readonly string erpBaseUrl;
    private readonly string taxBaseUrl;
    private readonly string clockBaseUrl;

    public SystemConfiguration(string shopUiBaseUrl, string shopApiBaseUrl, string erpBaseUrl, string taxBaseUrl, string clockBaseUrl)
    {
        this.shopUiBaseUrl = shopUiBaseUrl;
        this.shopApiBaseUrl = shopApiBaseUrl;
        this.erpBaseUrl = erpBaseUrl;
        this.taxBaseUrl = taxBaseUrl;
        this.clockBaseUrl = clockBaseUrl;
    }

    public string ShopUiBaseUrl => shopUiBaseUrl;
    public string ShopApiBaseUrl => shopApiBaseUrl;
    public string ErpBaseUrl => erpBaseUrl;
    public string TaxBaseUrl => taxBaseUrl;
    public string ClockBaseUrl => clockBaseUrl;
}
