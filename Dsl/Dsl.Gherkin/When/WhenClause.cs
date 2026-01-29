using Dsl.Gherkin.Builders.When.PlaceOrder;
using Dsl.Gherkin.Builders.When.CancelOrder;
using Dsl.Gherkin.When;
using Dsl.Gherkin.Then;
using Optivem.Testing;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin;
using Optivem.EShop.SystemTest.Core.Gherkin.When;
using static Optivem.EShop.SystemTest.Core.Gherkin.GherkinDefaults;

namespace Dsl.Gherkin.When
{
    public class WhenClause : BaseClause
    {
        private readonly SystemDsl _app;
        private readonly ScenarioDsl _scenario;
        private bool _hasProduct;
        private bool _hasTaxRate;
        private readonly Func<Task>? _givenSetup;

        public WhenClause(Channel channel, SystemDsl app, ScenarioDsl scenario, bool hasProduct, bool hasTaxRate, Func<Task>? givenSetup = null)
            : base(channel)
        {
            _app = app;
            _scenario = scenario;
            _hasProduct = hasProduct;
            _hasTaxRate = hasTaxRate;
            _givenSetup = givenSetup;
        }

        public WhenClause(Channel channel, SystemDsl app, ScenarioDsl scenario)
            : this(channel, app, scenario, false, false, null)
        {
        }

        private async Task EnsureDefaults()
        {
            // Execute Given setup first if provided
            if (_givenSetup != null)
            {
                await _givenSetup();
            }

            if (!_hasProduct)
            {
                var result = await _app.Erp().ReturnsProduct()
                    .Sku(DefaultSku)
                    .UnitPrice(DefaultUnitPrice)
                    .Execute();
                result.ShouldSucceed();
                _hasProduct = true;
            }

            if (!_hasTaxRate)
            {
                var result = await _app.Tax().ReturnsTaxRate()
                    .Country(DefaultCountry)
                    .TaxRate(DefaultTaxRate)
                    .Execute();
                result.ShouldSucceed();
                _hasTaxRate = true;
            }
        }

        public PlaceOrderBuilder PlaceOrder()
        {
            return new PlaceOrderBuilder(_app, _scenario, () => EnsureDefaults());
        }

        public CancelOrderBuilder CancelOrder()
        {
            return new CancelOrderBuilder(_app, _scenario, () => EnsureDefaults());
        }

        public ViewOrderBuilder ViewOrder()
        {
            return new ViewOrderBuilder(_app, _scenario, () => EnsureDefaults());
        }

        public PublishCouponBuilder PublishCoupon()
        {
            return new PublishCouponBuilder(_app, _scenario);
        }

        public BrowseCouponsBuilder BrowseCoupons()
        {
            return new BrowseCouponsBuilder(_app, _scenario);
        }
    }
}