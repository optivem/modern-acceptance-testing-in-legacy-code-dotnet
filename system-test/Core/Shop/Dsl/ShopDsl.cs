using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Api;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Ui;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;
using Optivem.Testing.Channels;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl;

public class ShopDsl : IDisposable
{
    private readonly IShopDriver _driver;
    private readonly UseCaseContext _context;

    public ShopDsl(Channel channel, UseCaseContext context, SystemConfiguration configuration)
    {
        _driver = CreateDriver(channel, configuration);
        _context = context;
    }

    private static IShopDriver CreateDriver(Channel channel, SystemConfiguration configuration)
    {
        return channel.Type switch
        {
            ChannelType.UI => new ShopUiDriver(configuration.ShopUiBaseUrl),
            ChannelType.API => new ShopApiDriver(configuration.ShopApiBaseUrl),
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
}
