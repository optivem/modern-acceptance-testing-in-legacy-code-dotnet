using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands.Base;
using Optivem.Lang;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands;

public class GoToTax : BaseTaxCommand<VoidValue, VoidVerification>
{
    public GoToTax(TaxDriver driver, Context context) 
        : base(driver, context)
    {
    }

    public override CommandResult<VoidValue, VoidVerification> Execute()
    {
        var result = _driver.GoToTax();
        return new CommandResult<VoidValue, VoidVerification>(result, _context, (_, ctx) => new VoidVerification(ctx));
    }
}
