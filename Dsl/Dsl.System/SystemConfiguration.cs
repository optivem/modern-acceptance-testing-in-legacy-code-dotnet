using Microsoft.CodeAnalysis.CSharp.Syntax;
using Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core;

public class SystemConfiguration
{
    private readonly string shopUiBaseUrl;
    private readonly string shopApiBaseUrl;
    private readonly string erpBaseUrl;
    private readonly string taxBaseUrl;
    private readonly string clockBaseUrl;
    private readonly ExternalSystemMode externalSystemMode;

    public SystemConfiguration(string shopUiBaseUrl, string shopApiBaseUrl, string erpBaseUrl, string taxBaseUrl, string clockBaseUrl, ExternalSystemMode externalSystemMode)
    {
        this.shopUiBaseUrl = shopUiBaseUrl;
        this.shopApiBaseUrl = shopApiBaseUrl;
        this.erpBaseUrl = erpBaseUrl;
        this.taxBaseUrl = taxBaseUrl;
        this.clockBaseUrl = clockBaseUrl;
        this.externalSystemMode = externalSystemMode;
    }

    public string ShopUiBaseUrl => shopUiBaseUrl;
    public string ShopApiBaseUrl => shopApiBaseUrl;
    public string ErpBaseUrl => erpBaseUrl;
    public string TaxBaseUrl => taxBaseUrl;
    public string ClockBaseUrl => clockBaseUrl;
    public ExternalSystemMode ExternalSystemMode => externalSystemMode;
}
