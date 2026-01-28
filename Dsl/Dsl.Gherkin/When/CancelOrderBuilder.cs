using DevLab.JmesPath.Functions;
using Dsl.Gherkin.When;
using Optivem.Commons.Dsl;
using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Coupons;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;
using Optivem.Testing.Channels;
using static Optivem.EShop.SystemTest.Core.Gherkin.GherkinDefaults;

namespace Dsl.Gherkin.Builders.When.CancelOrder;

public class CancelOrderBuilder : BaseWhenBuilder<VoidValue, VoidVerification>
{
    private string? _orderNumber;

    public CancelOrderBuilder(SystemDsl app, ScenarioDsl scenario) : base(app, scenario)
    {
        WithOrderNumber(DefaultOrderNumber);
    }

    public CancelOrderBuilder WithOrderNumber(string? orderNumber)
    {
        _orderNumber = orderNumber;
        return this;
    }

    protected override ExecutionResult<VoidValue, VoidVerification> Execute(SystemDsl app)
    {
        var result = app.Shop(Channel).CancelOrder()
            .OrderNumber(_orderNumber)
            .Execute();

        return new ExecutionResultBuilder<VoidValue, VoidVerification>(result)
            .OrderNumber(_orderNumber)
            .Build();
    }
}