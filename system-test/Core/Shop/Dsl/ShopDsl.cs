using Optivem.EShop.SystemTest.Core.Shop.Channels;
using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;
using Optivem.Testing.Channels;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl;

public class ShopDsl : IDisposable
{
    private readonly IShopDriver _driver;
    private readonly Context _context;

    public ShopDsl(Channel channel, Context context)
    {
        _driver = channel.CreateShopDriver();
        _context = context;
    }

    public GoToShop GoToShop() => new(_driver, _context);

    public PlaceOrder PlaceOrder() => new(_driver, _context);

    public CancelOrder CancelOrder() => new(_driver, _context);

    public ViewOrder ViewOrder() => new(_driver, _context);

    public void Dispose()
    {
        _driver?.Dispose();
    }
}
