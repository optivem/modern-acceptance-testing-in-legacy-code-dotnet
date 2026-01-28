using Dsl.Gherkin.Then;
using Commons.Dsl;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;

namespace Dsl.Gherkin.Then
{
    public class ThenOrderBuilder<TSuccessResponse, TSuccessVerification> 
        : BaseThenBuilder<TSuccessResponse, TSuccessVerification>
        where TSuccessVerification : ResponseVerification<TSuccessResponse>
    {
        private readonly ViewOrderVerification _orderVerification;

        public ThenOrderBuilder(ThenClause<TSuccessResponse, TSuccessVerification> thenClause, SystemDsl app, string orderNumber) : base(thenClause)
        {
            _orderVerification = app.Shop(Channel).ViewOrder()
                .OrderNumber(orderNumber)
                .Execute()
                .ShouldSucceed();
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasSku(string expectedSku)
        {
            _orderVerification.Sku(expectedSku);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasQuantity(int expectedQuantity)
        {
            _orderVerification.Quantity(expectedQuantity);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasCountry(string expectedCountry)
        {
            _orderVerification.Country(expectedCountry);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasUnitPrice(decimal expectedUnitPrice)
        {
            _orderVerification.UnitPrice(expectedUnitPrice);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasBasePrice(decimal expectedBasePrice)
        {
            _orderVerification.BasePrice(expectedBasePrice);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasBasePrice(string basePrice)
        {
            _orderVerification.BasePrice(basePrice);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasSubtotalPrice(decimal expectedSubtotalPrice)
        {
            _orderVerification.SubtotalPrice(expectedSubtotalPrice);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasSubtotalPrice(string expectedSubtotalPrice)
        {
            return HasSubtotalPrice(decimal.Parse(expectedSubtotalPrice));
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasTotalPrice(decimal expectedTotalPrice)
        {
            _orderVerification.TotalPrice(expectedTotalPrice);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasStatus(OrderStatus expectedStatus)
        {
            _orderVerification.Status(expectedStatus);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasDiscountRateGreaterThanOrEqualToZero()
        {
            _orderVerification.DiscountRateGreaterThanOrEqualToZero();
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasDiscountRate(decimal expectedDiscountRate)
        {
            _orderVerification.DiscountRate(expectedDiscountRate);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasDiscountAmount(decimal expectedDiscountAmount)
        {
            _orderVerification.DiscountAmount(expectedDiscountAmount);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasDiscountAmount(string expectedDiscountAmount)
        {
            _orderVerification.DiscountAmount(expectedDiscountAmount);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasAppliedCoupon(string expectedCouponCode)
        {
            _orderVerification.AppliedCouponCode(expectedCouponCode);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasAppliedCoupon()
        {
            return HasAppliedCoupon(GherkinDefaults.DefaultCouponCode);
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasDiscountAmountGreaterThanOrEqualToZero()
        {
            _orderVerification.DiscountAmountGreaterThanOrEqualToZero();
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasSubtotalPriceGreaterThanZero()
        {
            _orderVerification.SubtotalPriceGreaterThanZero();
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasTaxRate(decimal expectedTaxRate)
        {
            _orderVerification.TaxRate(expectedTaxRate);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasTaxRate(string expectedTaxRate)
        {
            _orderVerification.TaxRate(expectedTaxRate);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasTaxRateGreaterThanOrEqualToZero()
        {
            _orderVerification.TaxRateGreaterThanOrEqualToZero();
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasTaxAmount(string expectedTaxAmount)
        {
            _orderVerification.TaxAmount(expectedTaxAmount);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasTaxAmountGreaterThanOrEqualToZero()
        {
            _orderVerification.TaxAmountGreaterThanOrEqualToZero();
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasTotalPrice(string expectedTotalPrice)
        {
            _orderVerification.TotalPrice(expectedTotalPrice);
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasTotalPriceGreaterThanZero()
        {
            _orderVerification.TotalPriceGreaterThanZero();
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasOrderNumberPrefix(string expectedPrefix)
        {
            _orderVerification.OrderNumberHasPrefix(expectedPrefix);
            return this;
        }
    }
}