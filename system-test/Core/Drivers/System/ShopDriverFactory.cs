using Optivem.EShop.SystemTest.Core.Channels;
using Optivem.Channels;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Api;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Ui;

namespace Optivem.EShop.SystemTest.Core.Drivers.System;

/// <summary>
/// Factory for creating shop-specific drivers based on channel type.
/// This is where shop-specific logic lives, keeping the Channel class generic.
/// </summary>
public class ShopDriverFactory : IChannelDriverFactory<IShopDriver>
{
    public IShopDriver CreateDriver(string channelType)
    {
        return channelType switch
        {
            ChannelType.UI => new ShopUiDriver(TestConfiguration.GetShopUiBaseUrl()),
            ChannelType.API => new ShopApiDriver(TestConfiguration.GetShopApiBaseUrl()),
            _ => throw new InvalidOperationException($"Unknown channel type: {channelType}")
        };
    }
}
