using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Api;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optivem.EShop.SystemTest.Core.Channels.Library
{
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
}
