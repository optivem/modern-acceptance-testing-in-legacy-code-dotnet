using System.Reflection;
using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Api;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Ui;
using Xunit.Sdk;

namespace Optivem.EShop.SystemTest.Core.Channels;

public class ChannelDataAttribute : DataAttribute
{
    private readonly string[] _channels;

    public ChannelDataAttribute(params string[] channels)
    {
        _channels = channels ?? throw new ArgumentNullException(nameof(channels));
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        foreach (var channel in _channels)
        {
            yield return new object[] { new Channel(channel) };
        }
    }
}

public class Channel
{
    private readonly string _channel;

    public Channel(string channel)
    {
        _channel = channel;
    }

    public IShopDriver CreateDriver()
    {
        return _channel switch
            {
                ChannelType.UI => new ShopUiDriver(TestConfiguration.GetShopUiBaseUrl()),
                ChannelType.API => new ShopApiDriver(TestConfiguration.GetShopApiBaseUrl()),
                _ => throw new InvalidOperationException($"Unknown channel: {_channel}")
            };
    }

    public override string ToString() => _channel;
}
