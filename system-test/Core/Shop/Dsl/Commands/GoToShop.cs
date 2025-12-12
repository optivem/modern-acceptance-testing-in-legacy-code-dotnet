using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.Results;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;

public class GoToShop : BaseShopCommand<VoidValue, VoidVerification>
{
    public GoToShop(IShopDriver driver, Context context) 
        : base(driver, context)
    {
    }

    public override CommandResult<VoidValue, VoidVerification> Execute()
    {
        var result = _driver.GoToShop();
        return new CommandResult<VoidValue, VoidVerification>(result, _context, (_, ctx) => new VoidVerification(ctx));
    }
}
