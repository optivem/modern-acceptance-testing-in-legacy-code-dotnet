using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Dsl.Tax.Commands.Base;

public abstract class BaseTaxCommand<TResponse, TVerification> : BaseCommand<TaxApiDriver, TResponse, TVerification>
{
    protected BaseTaxCommand(TaxApiDriver driver, Context context) 
        : base(driver, context)
    {
    }
}
