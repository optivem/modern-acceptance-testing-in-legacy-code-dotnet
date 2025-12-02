using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Api;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Ui;

namespace Optivem.EShop.SystemTest.Core.Channels;

/// <summary>
/// Project-specific extension methods for Channel.
/// This keeps the Channel class generic while providing domain-specific factory methods.
/// </summary>
public static class ChannelExtensions
{
    /// <summary>
    /// Creates a ShopDriver instance based on the channel type.
    /// </summary>
    public static IShopDriver CreateShopDriver(this Channel channel)
    {
        return channel.Value switch
        {
            ChannelType.UI => new ShopUiDriver(TestConfiguration.GetShopUiBaseUrl()),
            ChannelType.API => new ShopApiDriver(TestConfiguration.GetShopApiBaseUrl()),
            _ => throw new InvalidOperationException($"Unknown channel: {channel.Value}")
        };
    }
}
