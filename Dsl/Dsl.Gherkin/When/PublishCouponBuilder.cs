using Dsl.Gherkin;
using Dsl.Gherkin.When;
using Commons.Dsl;
using Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;
using Optivem.Testing;
using static Optivem.EShop.SystemTest.Core.Gherkin.GherkinDefaults;

namespace Optivem.EShop.SystemTest.Core.Gherkin.When;

public class PublishCouponBuilder : BaseWhenBuilder<VoidValue, VoidVerification>
{
    private string? _couponCode;
    private string? _discountRate;
    private string? _validFrom;
    private string? _validTo;
    private string? _usageLimit;

    public PublishCouponBuilder(SystemDsl app, ScenarioDsl scenario, Func<Task>? ensureDefaults) : base(app, scenario, ensureDefaults)
    {
        WithCouponCode(DefaultCouponCode);
        WithDiscountRate(DefaultDiscountRate);
    }

    public PublishCouponBuilder WithCouponCode(string? couponCode)
    {
        _couponCode = couponCode;
        return this;
    }

    public PublishCouponBuilder WithDiscountRate(string? discountRate)
    {
        _discountRate = discountRate;
        return this;
    }

    public PublishCouponBuilder WithDiscountRate(decimal discountRate)
    {
        _discountRate = Converter.FromDecimal(discountRate);
        return this;
    }

    public PublishCouponBuilder WithValidFrom(string? validFrom)
    {
        _validFrom = validFrom;
        return this;
    }

    public PublishCouponBuilder WithValidTo(string? validTo)
    {
        _validTo = validTo;
        return this;
    }

    public PublishCouponBuilder WithUsageLimit(string? usageLimit)
    {
        _usageLimit = usageLimit;
        return this;
    }

    public PublishCouponBuilder WithUsageLimit(int usageLimit)
    {
        _usageLimit = Converter.FromInteger(usageLimit);
        return this;
    }

    protected override async Task<ExecutionResult<VoidValue, VoidVerification>> Execute(SystemDsl app)
    {
        var shop = await app.Shop(Channel);
        var result = await shop.PublishCoupon()
            .CouponCode(_couponCode)
            .DiscountRate(_discountRate)
            .ValidFrom(_validFrom)
            .ValidTo(_validTo)
            .UsageLimit(_usageLimit)
            .Execute();

        return new ExecutionResultBuilder<VoidValue, VoidVerification>(result)
            .CouponCode(_couponCode)
            .Build();
    }
}