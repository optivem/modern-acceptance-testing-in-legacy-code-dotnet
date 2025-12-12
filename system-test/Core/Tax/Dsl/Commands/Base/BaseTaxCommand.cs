using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands.Base;

public abstract class BaseTaxCommand<TResponse, TVerification> : BaseCommand<TaxDriver, TResponse, TVerification>
{
    protected BaseTaxCommand(TaxDriver driver, Context context) 
        : base(driver, context)
    {
    }
}
