using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;
using Optivem.Testing;
using static Optivem.EShop.SystemTest.Core.Gherkin.GherkinDefaults;

namespace Dsl.Gherkin.When;

public class ViewOrderBuilder : BaseWhenBuilder<ViewOrderResponse, ViewOrderVerification>
{
    private string? _orderNumber;

    public ViewOrderBuilder(SystemDsl app, ScenarioDsl scenario) : base(app, scenario)
    {
        WithOrderNumber(DefaultOrderNumber);
    }

    public ViewOrderBuilder WithOrderNumber(string? orderNumber)
    {
        _orderNumber = orderNumber;
        return this;
    }

    protected override ExecutionResult<ViewOrderResponse, ViewOrderVerification> Execute(SystemDsl app)
    {
        var result = app.Shop(Channel).ViewOrder()
            .OrderNumber(_orderNumber)
            .Execute();

        return new ExecutionResultBuilder<ViewOrderResponse, ViewOrderVerification>(result)
            .OrderNumber(DefaultOrderNumber)
            .Build();
    }
}