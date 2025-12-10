using Optivem.Testing.Channels;
using Optivem.EShop.SystemTest.Core.Drivers.System;

namespace Optivem.EShop.SystemTest.Core.Channels;

/// <summary>
/// Extension methods for Channel to provide shop-specific convenience methods.
/// This separates shop-specific concerns from the generic Channel library.
/// </summary>
public static class ChannelExtensions
{
    private static readonly ShopDriverFactory _shopDriverFactory = new();

    /// <summary>
    /// Creates a shop driver for this channel.
    /// Convenience method that uses the default ShopDriverFactory.
    /// </summary>
    public static IShopDriver CreateShopDriver(this Channel channel)
    {
        return channel.CreateDriver(_shopDriverFactory);
    }
}
