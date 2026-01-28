using Dsl.Gherkin.Builders.When.PlaceOrder;
using Dsl.Gherkin.Builders.When.CancelOrder;
using Dsl.Gherkin.When;
using Dsl.Gherkin.Then;
using Optivem.Testing.Channels;
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



        public WhenClause(Channel channel, SystemDsl app, ScenarioDsl scenario, bool hasProduct, bool hasTaxRate)
            : base(channel)
        {
            _app = app;
            _scenario = scenario;
            _hasProduct = hasProduct;
            _hasTaxRate = hasTaxRate;
        }

        public WhenClause(Channel channel, SystemDsl app, ScenarioDsl scenario)
            : this(channel, app, scenario, false, false)
        {
        }

        private void EnsureDefaults()
        {
            if (!_hasProduct)
            {
                _app.Erp().ReturnsProduct()
                    .Sku(DefaultSku)
                    .UnitPrice(DefaultUnitPrice)
                    .Execute()
                    .ShouldSucceed();
                _hasProduct = true;
            }

            if (!_hasTaxRate)
            {
                _app.Tax().ReturnsTaxRate()
                    .Country(DefaultCountry)
                    .TaxRate(DefaultTaxRate)
                    .Execute()
                    .ShouldSucceed();
                _hasTaxRate = true;
            }
        }

        public PlaceOrderBuilder PlaceOrder()
        {
            EnsureDefaults();
            return new PlaceOrderBuilder(_app, _scenario);
        }

        public CancelOrderBuilder CancelOrder()
        {
            EnsureDefaults();
            return new CancelOrderBuilder(_app, _scenario);
        }

        public ViewOrderBuilder ViewOrder()
        {
            EnsureDefaults();
            return new ViewOrderBuilder(_app, _scenario);
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