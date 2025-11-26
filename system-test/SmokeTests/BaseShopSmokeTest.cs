using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Clients.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optivem.EShop.SystemTest.SmokeTests
{
    public abstract class BaseShopSmokeTest
    {
        private readonly IShopDriver _shopApiDriver;

        public BaseShopSmokeTest()
        {
            _shopApiDriver = CreateDriver();
        }

        public void Dispose()
        {
            Closer.Close(_shopApiDriver);
        }

        protected abstract IShopDriver CreateDriver();

        [Fact]
        public void ShouldBeAbleToGoToShop()
        {
            var result = _shopApiDriver.GoToShop();
            Assert.True(result.Success);
        }
    }
}
