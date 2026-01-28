using Dsl.Gherkin;
using Dsl.Gherkin.When;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Coupons;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;
using Optivem.Testing;
using static Optivem.EShop.SystemTest.Core.Gherkin.GherkinDefaults;

namespace Dsl.Gherkin.When;

public class BrowseCouponsBuilder : BaseWhenBuilder<BrowseCouponsResponse, BrowseCouponsVerification>
{
    public BrowseCouponsBuilder(SystemDsl app, ScenarioDsl scenario) : base(app, scenario)
    {
    }

    protected override ExecutionResult<BrowseCouponsResponse, BrowseCouponsVerification> Execute(SystemDsl app)
    {
        var result = app.Shop(Channel)
            .BrowseCoupons()
            .Execute();

        return new ExecutionResultBuilder<BrowseCouponsResponse, BrowseCouponsVerification>(result)
            .Build();
    }
}