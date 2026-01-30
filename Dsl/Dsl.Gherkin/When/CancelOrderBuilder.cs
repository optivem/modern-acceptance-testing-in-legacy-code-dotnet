using DevLab.JmesPath.Functions;
using Dsl.Gherkin.When;
using Commons.Dsl;
using Commons.Util;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Coupons;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;
using Optivem.Testing;
using static Optivem.EShop.SystemTest.Core.Gherkin.GherkinDefaults;

namespace Dsl.Gherkin.Builders.When.CancelOrder;

public class CancelOrderBuilder : BaseWhenBuilder<VoidValue, VoidVerification>
{
    private string? _orderNumber;

    public CancelOrderBuilder(SystemDsl app, ScenarioDsl scenario, Func<Task>? ensureDefaults = null) : base(app, scenario, ensureDefaults)
    {
        WithOrderNumber(DefaultOrderNumber);
    }

    public CancelOrderBuilder WithOrderNumber(string? orderNumber)
    {
        _orderNumber = orderNumber;
        return this;
    }

    protected override async Task<ExecutionResult<VoidValue, VoidVerification>> Execute(SystemDsl app)
    {
        var shop = await app.Shop(Channel);
        var result = await shop.CancelOrder()
            .OrderNumber(_orderNumber)
            .Execute();

        return new ExecutionResultBuilder<VoidValue, VoidVerification>(result)
            .OrderNumber(_orderNumber)
            .Build();
    }
}