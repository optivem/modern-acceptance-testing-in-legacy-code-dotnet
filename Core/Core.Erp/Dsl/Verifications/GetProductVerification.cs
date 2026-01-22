using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos;
using Optivem.Commons.Dsl;
using Shouldly;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl.Verifications;

public class GetProductVerification : ResponseVerification<GetProductResponse, UseCaseContext>
{
    public GetProductVerification(GetProductResponse response, UseCaseContext context)
        : base(response, context)
    {
    }

    public GetProductVerification Sku(string skuParamAlias)
    {
        var expectedSku = Context.GetParamValue(skuParamAlias);
        var actualSku = Response.Sku;
        actualSku.ShouldBe(expectedSku, $"Expected SKU to be '{expectedSku}', but was '{actualSku}'");
        return this;
    }

    public GetProductVerification Price(decimal expectedPrice)
    {
        var actualPrice = Response.Price;
        actualPrice.ShouldBe(expectedPrice, $"Expected price to be {expectedPrice}, but was {actualPrice}");
        return this;
    }

    public GetProductVerification Price(string expectedPrice)
    {
        return Price(decimal.Parse(expectedPrice));
    }
}