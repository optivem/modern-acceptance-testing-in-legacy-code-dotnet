using Optivem.EShop.SystemTest.Core.Channels;
using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Dsl;
using Optivem.EShop.SystemTest.Core.Dsl.Commons;
using Optivem.EShop.SystemTest.Core.Dsl.Shop;
using Optivem.Testing.Channels;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests;

public class ShopSmokeTest : IDisposable
{
    private readonly DslFactory _dslFactory;
    private ShopDsl? _shop;

    public ShopSmokeTest()
    {
        _dslFactory = new DslFactory();
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public void ShouldBeAbleToGoToShop(Channel channel)
    {
        _shop = _dslFactory.CreateShopDsl(channel);

        _shop.GoToShop()
            .Execute()
            .ShouldSucceed();
    }

    public void Dispose()
    {
        _shop?.Dispose();
        ChannelContext.Clear();
    }
}
