using Optivem.EShop.SystemTest.Core.Channels;
using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Dsl.Commons;
using Optivem.EShop.SystemTest.Core.Dsl.Shop;
using Optivem.Testing.Channels;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests;

public class ShopSmokeTest : IDisposable
{
    private IShopDriver? _shopDriver;
    private ShopDsl? _shop;

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public void ShouldBeAbleToGoToShop(Channel channel)
    {
        _shopDriver = channel.CreateShopDriver();
        var context = new Context();
        _shop = new ShopDsl(_shopDriver, context);

        _shop.GoToShop()
            .Execute()
            .ShouldSucceed();
    }

    public void Dispose()
    {
        _shopDriver?.Dispose();
        ChannelContext.Clear();
    }
}
