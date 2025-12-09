using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Commands.Base;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Context;

namespace Optivem.EShop.SystemTest.Core.Dsl.Shop.Commands.Base;

public abstract class BaseShopCommand<TResponse, TVerification> : BaseCommand<IShopDriver, TResponse, TVerification>
{
    protected BaseShopCommand(IShopDriver driver, TestContext context) 
        : base(driver, context)
    {
    }
}
