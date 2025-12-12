using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands.Base;

public abstract class BaseTaxCommand<TResponse, TVerification> : BaseCommand<TaxApiDriver, TResponse, TVerification>
{
    protected BaseTaxCommand(TaxApiDriver driver, Context context) 
        : base(driver, context)
    {
    }
}
