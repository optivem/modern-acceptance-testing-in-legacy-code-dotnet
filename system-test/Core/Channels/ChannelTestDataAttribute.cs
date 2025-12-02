using System.Reflection;
using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Api;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Ui;
using Xunit.Sdk;

namespace Optivem.EShop.SystemTest.Core.Channels;

/// <summary>
/// Provides channel test data with automatic setup and teardown.
/// </summary>
public class ChannelTestDataAttribute : DataAttribute
{
    private readonly string[] _channels;

    public ChannelTestDataAttribute(params string[] channels)
    {
        _channels = channels ?? throw new ArgumentNullException(nameof(channels));
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        foreach (var channel in _channels)
        {
            yield return new object[] { new ChannelTestCase(channel) };
        }
    }
}

/// <summary>
/// Represents a test case for a specific channel.
/// Automatically manages driver lifecycle.
/// </summary>
public class ChannelTestCase : IDisposable
{
    private readonly string _channel;
    private IShopDriver? _shopDriver;

    public ChannelTestCase(string channel)
    {
        _channel = channel;
    }

    public IShopDriver GetShopDriver()
    {
        if (_shopDriver == null)
        {
            _shopDriver = _channel switch
            {
                ChannelType.UI => new ShopUiDriver(TestConfiguration.GetShopUiBaseUrl()),
                ChannelType.API => new ShopApiDriver(TestConfiguration.GetShopApiBaseUrl()),
                _ => throw new InvalidOperationException($"Unknown channel: {_channel}")
            };
        }
        return _shopDriver;
    }

    public void Dispose()
    {
        _shopDriver?.Dispose();
    }

    public override string ToString() => _channel;
}
