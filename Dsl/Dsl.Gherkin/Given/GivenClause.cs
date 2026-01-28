using Dsl.Gherkin.When;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin.Given;
using Optivem.Testing.Channels;

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
            SetupClock();
            SetupErp();
            SetupTax();
            SetupShop();

            return new WhenClause(Channel, _app, _scenario, _products.Count > 0, _countries.Count > 0);
        }

        private void SetupClock()
        {
            _clock.Execute(_app);
        }

        private void SetupErp()
        {
            if (_orders.Count > 0 && _products.Count == 0)
            {
                var defaultProduct = new GivenProductBuilder(this);
                _products.Add(defaultProduct);
            }

            foreach (var product in _products)
            {
                product.Execute(_app);
            }
        }

        private void SetupTax()
        {
            if (_orders.Count > 0 && _countries.Count == 0)
            {
                var defaultCountry = new GivenCountryBuilder(this);
                _countries.Add(defaultCountry);
            }

            foreach (var country in _countries)
            {
                country.Execute(_app);
            }
        }

        private void SetupShop()
        {
            SetupCoupons();
            SetupOrders();
        }

        private void SetupCoupons()
        {
            if (_orders.Count > 0 && _coupons.Count == 0)
            {
                var defaultCoupon = new GivenCouponBuilder(this);
                _coupons.Add(defaultCoupon);
            }

            foreach (var coupon in _coupons)
            {
                coupon.Execute(_app);
            }
        }

        private void SetupOrders()
        {
            foreach (var order in _orders)
            {
                order.Execute(_app);
            }
        }
    }
}