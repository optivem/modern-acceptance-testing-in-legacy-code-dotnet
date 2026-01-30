using Dsl.Gherkin.When;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin.Given;
using Optivem.Testing;

namespace Dsl.Gherkin.Given
{
    public class GivenClause : BaseClause
    {
        private readonly SystemDsl _app;
        private readonly ScenarioDsl _scenario;
        private readonly List<GivenProductBuilder> _products;
        private readonly List<GivenOrderBuilder> _orders;
        private GivenClockBuilder _clock;
        private readonly List<GivenCountryBuilder> _countries;
        private readonly List<GivenCouponBuilder> _coupons;

        public GivenClause(Channel channel, SystemDsl app, ScenarioDsl scenario)
            : base(channel)
        {
            _app = app;
            _scenario = scenario;
            _products = new List<GivenProductBuilder>();
            _orders = new List<GivenOrderBuilder>();
            _clock = new GivenClockBuilder(this);
            _countries = new List<GivenCountryBuilder>();
            _coupons = new List<GivenCouponBuilder>();
        }

        public GivenProductBuilder Product()
        {
            var productBuilder = new GivenProductBuilder(this);
            _products.Add(productBuilder);
            return productBuilder;
        }

        public GivenOrderBuilder Order()
        {
            var orderBuilder = new GivenOrderBuilder(this);
            _orders.Add(orderBuilder);
            return orderBuilder;
        }

        public GivenClockBuilder Clock()
        {
            _clock = new GivenClockBuilder(this);
            return _clock;
        }

        public GivenCountryBuilder Country()
        {
            var taxRateBuilder = new GivenCountryBuilder(this);
            _countries.Add(taxRateBuilder);
            return taxRateBuilder;
        }

        public GivenCouponBuilder Coupon()
        {
            var couponBuilder = new GivenCouponBuilder(this);
            _coupons.Add(couponBuilder);
            return couponBuilder;
        }

        public WhenClause When()
        {
            return new WhenClause(Channel, _app, _scenario, _products.Any(), _countries.Any(), SetupGiven);
        }

        private async Task SetupGiven()
        {
            await SetupClock();
            await SetupErp();
            await SetupTax();
            await SetupShop();
        }

        private async Task SetupClock()
        {
            await _clock.Execute(_app);
        }

        private async Task SetupErp()
        {
            if (_orders.Any() && !_products.Any())
            {
                var defaultProduct = new GivenProductBuilder(this);
                _products.Add(defaultProduct);
            }

            foreach (var product in _products)
            {
                await product.Execute(_app);
            }
        }

        private async Task SetupTax()
        {
            if (_orders.Any() && !_countries.Any())
            {
                var defaultCountry = new GivenCountryBuilder(this);
                _countries.Add(defaultCountry);
            }

            foreach (var country in _countries)
            {
                await country.Execute(_app);
            }
        }

        private async Task SetupShop()
        {
            await SetupCoupons();
            await SetupOrders();
        }

        private async Task SetupCoupons()
        {
            if (_orders.Any() && !_coupons.Any())
            {
                var defaultCoupon = new GivenCouponBuilder(this);
                _coupons.Add(defaultCoupon);
            }

            foreach (var coupon in _coupons)
            {
                await coupon.Execute(_app);
            }
        }

        private async Task SetupOrders()
        {
            foreach (var order in _orders)
            {
                await order.Execute(_app);
            }
        }
    }
}