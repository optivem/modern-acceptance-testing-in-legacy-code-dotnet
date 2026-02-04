using Dsl.Gherkin.Then;
using Commons.Dsl;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;
using System.Runtime.CompilerServices;

namespace Dsl.Gherkin.Then
{
    public class ThenOrderBuilder<TSuccessResponse, TSuccessVerification> 
        : BaseThenBuilder<TSuccessResponse, TSuccessVerification>
        where TSuccessVerification : ResponseVerification<TSuccessResponse>
    {
        private readonly SystemDsl _app;
        private readonly string _orderNumber;
        private ViewOrderVerification? _orderVerification;

        public ThenOrderBuilder(
            ThenClause<TSuccessResponse, TSuccessVerification> thenClause, 
            SystemDsl app, 
            string orderNumber) 
            : base(thenClause)
        {
            _app = app;
            _orderNumber = orderNumber;
        }

        private async Task<ViewOrderVerification> GetOrderVerification()
        {
            if (_orderVerification == null)
            {
                var shop = await _app.Shop(Channel);
                var result = await shop.ViewOrder()
                    .OrderNumber(_orderNumber)
                    .Execute();
                _orderVerification = result.ShouldSucceed();
            }
            return _orderVerification;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasSku(string expectedSku)
        {
            var verification = await GetOrderVerification();
            verification.Sku(expectedSku);
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasQuantity(int expectedQuantity)
        {
            var verification = await GetOrderVerification();
            verification.Quantity(expectedQuantity);
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasCountry(string expectedCountry)
        {
            var verification = await GetOrderVerification();
            verification.Country(expectedCountry);
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasUnitPrice(decimal expectedUnitPrice)
        {
            var verification = await GetOrderVerification();
            verification.UnitPrice(expectedUnitPrice);
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasBasePrice(decimal expectedBasePrice)
        {
            var verification = await GetOrderVerification();
            verification.BasePrice(expectedBasePrice);
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasBasePrice(string basePrice)
        {
            var verification = await GetOrderVerification();
            verification.BasePrice(basePrice);
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasSubtotalPrice(decimal expectedSubtotalPrice)
        {
            var verification = await GetOrderVerification();
            verification.SubtotalPrice(expectedSubtotalPrice);
            return this;
        }

        public Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasSubtotalPrice(string expectedSubtotalPrice)
        {
            return HasSubtotalPrice(decimal.Parse(expectedSubtotalPrice));
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasTotalPrice(decimal expectedTotalPrice)
        {
            var verification = await GetOrderVerification();
            verification.TotalPrice(expectedTotalPrice);
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasStatus(OrderStatus expectedStatus)
        {
            var verification = await GetOrderVerification();
            verification.Status(expectedStatus);
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasDiscountRateGreaterThanOrEqualToZero()
        {
            var verification = await GetOrderVerification();
            verification.DiscountRateGreaterThanOrEqualToZero();
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasDiscountRate(decimal expectedDiscountRate)
        {
            var verification = await GetOrderVerification();
            verification.DiscountRate(expectedDiscountRate);
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasDiscountAmount(decimal expectedDiscountAmount)
        {
            var verification = await GetOrderVerification();
            verification.DiscountAmount(expectedDiscountAmount);
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasDiscountAmount(string expectedDiscountAmount)
        {
            var verification = await GetOrderVerification();
            verification.DiscountAmount(expectedDiscountAmount);
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasAppliedCoupon(string expectedCouponCode)
        {
            var verification = await GetOrderVerification();
            verification.AppliedCouponCode(expectedCouponCode);
            return this;
        }

        public Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasAppliedCoupon()
        {
            return HasAppliedCoupon(GherkinDefaults.DefaultCouponCode);
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasDiscountAmountGreaterThanOrEqualToZero()
        {
            var verification = await GetOrderVerification();
            verification.DiscountAmountGreaterThanOrEqualToZero();
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasSubtotalPriceGreaterThanZero()
        {
            var verification = await GetOrderVerification();
            verification.SubtotalPriceGreaterThanZero();
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasTaxRate(decimal expectedTaxRate)
        {
            var verification = await GetOrderVerification();
            verification.TaxRate(expectedTaxRate);
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasTaxRate(string expectedTaxRate)
        {
            var verification = await GetOrderVerification();
            verification.TaxRate(expectedTaxRate);
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasTaxRateGreaterThanOrEqualToZero()
        {
            var verification = await GetOrderVerification();
            verification.TaxRateGreaterThanOrEqualToZero();
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasTaxAmount(string expectedTaxAmount)
        {
            var verification = await GetOrderVerification();
            verification.TaxAmount(expectedTaxAmount);
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasTaxAmountGreaterThanOrEqualToZero()
        {
            var verification = await GetOrderVerification();
            verification.TaxAmountGreaterThanOrEqualToZero();
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasTotalPrice(string expectedTotalPrice)
        {
            var verification = await GetOrderVerification();
            verification.TotalPrice(expectedTotalPrice);
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasTotalPriceGreaterThanZero()
        {
            var verification = await GetOrderVerification();
            verification.TotalPriceGreaterThanZero();
            return this;
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> HasOrderNumberPrefix(string expectedPrefix)
        {
            var verification = await GetOrderVerification();
            verification.OrderNumberHasPrefix(expectedPrefix);
            return this;
        }
    }
}