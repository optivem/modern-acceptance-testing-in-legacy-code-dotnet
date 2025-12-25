using Optivem.EShop.SystemTest.Core.Gherkin.Clauses;

namespace Optivem.EShop.SystemTest.Core.Gherkin.Builders;

public class ProductBuilder
{
    private readonly GivenClause _givenClause;
    private readonly SystemDsl _systemDsl;
    private string? _sku;
    private decimal? _unitPrice;

    public ProductBuilder(GivenClause givenClause, SystemDsl systemDsl)
    {
        _givenClause = givenClause;
        _systemDsl = systemDsl;
    }

    public ProductBuilder Sku(string sku)
    {
        _sku = sku;
        return this;
    }

    public ProductBuilder UnitPrice(decimal unitPrice)
    {
        _unitPrice = unitPrice;
        return this;
    }

    public ProductBuilder UnitPrice(string unitPrice)
    {
        _unitPrice = decimal.Parse(unitPrice);
        return this;
    }

    public GivenClause And()
    {
        _givenClause.AddGivenAction(() =>
        {
            var command = _systemDsl.Erp.CreateProduct();

            if (_sku != null)
            {
                command.Sku(_sku);
            }

            if (_unitPrice.HasValue)
            {
                command.UnitPrice(_unitPrice.Value);
            }

            command.Execute().ShouldSucceed();
        });

        return _givenClause;
    }
}
