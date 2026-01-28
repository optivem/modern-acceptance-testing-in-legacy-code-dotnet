using Dsl.Gherkin.Given;
using static Optivem.EShop.SystemTest.Core.Gherkin.GherkinDefaults;

namespace Optivem.EShop.SystemTest.Core.Gherkin.Given;

public class GivenCouponBuilder : BaseGivenBuilder
{
    private string? _couponCode;
    private string? _discountRate;
    private string? _validFrom;
    private string? _validTo;
    private string? _usageLimit;

    public GivenCouponBuilder(GivenClause givenClause) : base(givenClause)
    {
        WithCouponCode(DefaultCouponCode);
        WithDiscountRate(DefaultDiscountRate);
        WithValidFrom(Empty);
        WithValidTo(Empty);
        WithUsageLimit(Empty);
    }

    public GivenCouponBuilder WithCouponCode(string couponCode)
    {
        _couponCode = couponCode;
        return this;
    }

    public GivenCouponBuilder WithDiscountRate(string discountRate)
    {
        _discountRate = discountRate;
        return this;
    }

    public GivenCouponBuilder WithDiscountRate(decimal discountRate)
    {
        _discountRate = discountRate.ToString();
        return this;
    }


    public GivenCouponBuilder WithValidFrom(string validFrom)
    {
        _validFrom = validFrom;
        return this;
    }

    public GivenCouponBuilder WithValidTo(string validTo)
    {
        _validTo = validTo;
        return this;
    }

    public GivenCouponBuilder WithUsageLimit(string usageLimit)
    {
        _usageLimit = usageLimit;
        return this;
    }

    public GivenCouponBuilder WithUsageLimit(int usageLimit)
    {
        return WithUsageLimit(usageLimit.ToString());
    }

    internal override void Execute(SystemDsl app)
    {
        app.Shop(Channel).PublishCoupon()
            .CouponCode(_couponCode)
            .DiscountRate(_discountRate)
            .ValidFrom(_validFrom)
            .ValidTo(_validTo)
            .UsageLimit(_usageLimit)
            .Execute()
            .ShouldSucceed();
    }
}