using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Commons.Dsl;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;

public abstract class BaseShopCommand<TResponse, TVerification>
    where TVerification : ResponseVerification<TResponse>
{
    protected readonly IShopDriver _driver;
    protected readonly UseCaseContext _context;

    protected BaseShopCommand(IShopDriver driver, UseCaseContext context)
    {
        _driver = driver;
        _context = context;
    }

    public abstract Task<ShopUseCaseResult<TResponse, TVerification>> Execute();
}