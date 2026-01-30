using Commons.Util;
using Dsl.Gherkin.Given;
using static Optivem.EShop.SystemTest.Core.Gherkin.GherkinDefaults;

namespace Optivem.EShop.SystemTest.Core.Gherkin.Given;

public class GivenProductBuilder : BaseGivenBuilder
{
    private string? _sku;
    private string? _unitPrice;

    public GivenProductBuilder(GivenClause givenClause)
        : base(givenClause)
    {
        WithSku(DefaultSku);
        WithUnitPrice(DefaultUnitPrice);
    }

    public GivenProductBuilder WithSku(string? sku)
    {
        _sku = sku;
        return this;
    }

    public GivenProductBuilder WithUnitPrice(string? unitPrice)
    {
        _unitPrice = unitPrice;
        return this;
    }

    public GivenProductBuilder WithUnitPrice(decimal? unitPrice)
    {
        return WithUnitPrice(Converter.FromDecimal(unitPrice));
    }

    internal override async Task Execute(SystemDsl app)
    {
        (await app.Erp().ReturnsProduct()
            .Sku(_sku)
            .UnitPrice(_unitPrice)
            .Execute())
            .ShouldSucceed();
    }
}