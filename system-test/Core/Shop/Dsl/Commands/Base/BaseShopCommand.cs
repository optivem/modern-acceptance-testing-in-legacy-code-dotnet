using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;

public abstract class BaseShopCommand<TResponse, TVerification> : BaseCommand<IShopDriver, TResponse, TVerification>
{
    protected BaseShopCommand(IShopDriver driver, Context context) 
        : base(driver, context)
    {
    }
}
