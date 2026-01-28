using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.Commons.Dsl;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;

public abstract class BaseShopCommand<TResponse, TVerification> 
    : BaseUseCase<IShopDriver, TResponse, SystemError, TVerification, ErrorFailureVerification>
{
    protected BaseShopCommand(IShopDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }
}