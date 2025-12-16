using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.EShop.SystemTest.Core.Common.Error;
using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;

public abstract class BaseShopCommand<TResponse, TVerification> 
    : BaseUseCase<IShopDriver, UseCaseContext, TResponse, Error, TVerification, ErrorFailureVerification>
{
    protected BaseShopCommand(IShopDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }
}
