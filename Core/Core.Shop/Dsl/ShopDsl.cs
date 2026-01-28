using Optivem.EShop.SystemTest.Core.Common;
using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Api;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Ui;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;
using Optivem.Testing;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl;

public class ShopDsl : IDisposable
{
    private readonly IShopDriver _driver;
    private readonly UseCaseContext _context;

    public ShopDsl(string uiBaseUrl, string apiBaseUrl, Channel channel, UseCaseContext context)
    {
        _driver = CreateDriver(uiBaseUrl, apiBaseUrl, channel);
        _context = context;
    }

    private static IShopDriver CreateDriver(string uiBaseUrl, string apiBaseUrl, Channel channel)
    {
        return channel.Type switch
        {
            ChannelType.UI => new ShopUiDriver(uiBaseUrl),
            ChannelType.API => new ShopApiDriver(apiBaseUrl),
            _ => throw new InvalidOperationException($"Unknown channel: {channel}")
        };
    }

    public void Dispose()
    {
        _driver?.Dispose();
    }

    public GoToShop GoToShop() => new(_driver, _context);

    public PlaceOrder PlaceOrder() => new(_driver, _context);

    public CancelOrder CancelOrder() => new(_driver, _context);

    public ViewOrder ViewOrder() => new(_driver, _context);

    public BrowseCoupons BrowseCoupons() => new(_driver, _context);

    public PublishCoupon PublishCoupon() => new(_driver, _context);
}
