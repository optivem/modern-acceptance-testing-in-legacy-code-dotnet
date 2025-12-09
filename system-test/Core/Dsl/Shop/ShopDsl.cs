using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Context;
using Optivem.EShop.SystemTest.Core.Dsl.Shop.Commands;

namespace Optivem.EShop.SystemTest.Core.Dsl.Shop;

public class ShopDsl
{
    private readonly IShopDriver _driver;
    private readonly TestContext _context;

    public ShopDsl(IShopDriver driver, TestContext context)
    {
        _driver = driver;
        _context = context;
    }

    public GoToShop GoToShop() => new(_driver, _context);

    public PlaceOrder PlaceOrder() => new(_driver, _context);

    public CancelOrder CancelOrder() => new(_driver, _context);

    public ViewOrder ViewOrder() => new(_driver, _context);
}
