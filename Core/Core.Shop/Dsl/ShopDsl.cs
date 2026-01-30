using Optivem.EShop.SystemTest.Core.Common;
using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Api;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Ui;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;
using Optivem.Testing;
using Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl;

public class ShopDsl : IAsyncDisposable
{
    private readonly IShopDriver _driver;
    private readonly UseCaseContext _context;

    private ShopDsl(IShopDriver driver, UseCaseContext context)
    {
        _driver = driver;
        _context = context;
    }

    public static async Task<ShopDsl> CreateAsync(string uiBaseUrl, string apiBaseUrl, Channel channel, UseCaseContext context)
    {
        var driver = await CreateDriverAsync(uiBaseUrl, apiBaseUrl, channel);
        return new ShopDsl(driver, context);
    }

    private static async Task<IShopDriver> CreateDriverAsync(string uiBaseUrl, string apiBaseUrl, Channel channel)
    {
        return channel.Type switch
        {
            ChannelType.UI => await ShopUiDriver.CreateAsync(uiBaseUrl),
            ChannelType.API => new ShopApiDriver(apiBaseUrl),
            _ => throw new InvalidOperationException($"Unknown channel: {channel}")
        };
    }

    public async ValueTask DisposeAsync()
    {
        if (_driver != null)
            await _driver.DisposeAsync();
    }

    public GoToShop GoToShop() => new(_driver, _context);

    public PlaceOrder PlaceOrder() => new(_driver, _context);

    public CancelOrder CancelOrder() => new(_driver, _context);

    public ViewOrder ViewOrder() => new(_driver, _context);

    public BrowseCoupons BrowseCoupons() => new(_driver, _context);

    public PublishCoupon PublishCoupon() => new(_driver, _context);
}
