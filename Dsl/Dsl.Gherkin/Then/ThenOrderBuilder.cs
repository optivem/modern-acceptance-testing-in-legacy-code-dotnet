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
        private readonly List<Action<ViewOrderVerification>> _validations = new();
        private readonly Func<Task<ExecutionResult<TSuccessResponse, TSuccessVerification>>> _lazyExecute;

        public ThenOrderBuilder(
            ThenClause<TSuccessResponse, TSuccessVerification> thenClause, 
            SystemDsl app, 
            string orderNumber,
            Func<Task<ExecutionResult<TSuccessResponse, TSuccessVerification>>> lazyExecute) 
            : base(thenClause)
        {
            _app = app;
            _orderNumber = orderNumber;
            _lazyExecute = lazyExecute;
        }

        private async Task<ViewOrderVerification> GetOrderVerification()
        {
            if (_orderVerification == null)
            {
                var result = await (await _app.Shop(Channel)).ViewOrder()
                    .OrderNumber(_orderNumber)
                    .Execute();
                _orderVerification = result.ShouldSucceed();
            }
            return _orderVerification;
        }

        private async Task ExecuteValidations()
        {
            // Execute the When clause first
            await _lazyExecute();
            
            // Then get order verification and run validations
            var verification = await GetOrderVerification();
            foreach (var validation in _validations)
            {
                validation(verification);
            }
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasSku(string expectedSku)
        {
            _validations.Add(v => v.Sku(expectedSku));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasQuantity(int expectedQuantity)
        {
            _validations.Add(v => v.Quantity(expectedQuantity));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasCountry(string expectedCountry)
        {
            _validations.Add(v => v.Country(expectedCountry));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasUnitPrice(decimal expectedUnitPrice)
        {
            _validations.Add(v => v.UnitPrice(expectedUnitPrice));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasBasePrice(decimal expectedBasePrice)
        {
            _validations.Add(v => v.BasePrice(expectedBasePrice));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasBasePrice(string basePrice)
        {
            _validations.Add(v => v.BasePrice(basePrice));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasSubtotalPrice(decimal expectedSubtotalPrice)
        {
            _validations.Add(v => v.SubtotalPrice(expectedSubtotalPrice));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasSubtotalPrice(string expectedSubtotalPrice)
        {
            return HasSubtotalPrice(decimal.Parse(expectedSubtotalPrice));
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasTotalPrice(decimal expectedTotalPrice)
        {
            _validations.Add(v => v.TotalPrice(expectedTotalPrice));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasStatus(OrderStatus expectedStatus)
        {
            _validations.Add(v => v.Status(expectedStatus));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasDiscountRateGreaterThanOrEqualToZero()
        {
            _validations.Add(v => v.DiscountRateGreaterThanOrEqualToZero());
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasDiscountRate(decimal expectedDiscountRate)
        {
            _validations.Add(v => v.DiscountRate(expectedDiscountRate));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasDiscountAmount(decimal expectedDiscountAmount)
        {
            _validations.Add(v => v.DiscountAmount(expectedDiscountAmount));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasDiscountAmount(string expectedDiscountAmount)
        {
            _validations.Add(v => v.DiscountAmount(expectedDiscountAmount));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasAppliedCoupon(string expectedCouponCode)
        {
            _validations.Add(v => v.AppliedCouponCode(expectedCouponCode));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasAppliedCoupon()
        {
            return HasAppliedCoupon(GherkinDefaults.DefaultCouponCode);
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasDiscountAmountGreaterThanOrEqualToZero()
        {
            _validations.Add(v => v.DiscountAmountGreaterThanOrEqualToZero());
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasSubtotalPriceGreaterThanZero()
        {
            _validations.Add(v => v.SubtotalPriceGreaterThanZero());
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasTaxRate(decimal expectedTaxRate)
        {
            _validations.Add(v => v.TaxRate(expectedTaxRate));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasTaxRate(string expectedTaxRate)
        {
            _validations.Add(v => v.TaxRate(expectedTaxRate));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasTaxRateGreaterThanOrEqualToZero()
        {
            _validations.Add(v => v.TaxRateGreaterThanOrEqualToZero());
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasTaxAmount(string expectedTaxAmount)
        {
            _validations.Add(v => v.TaxAmount(expectedTaxAmount));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasTaxAmountGreaterThanOrEqualToZero()
        {
            _validations.Add(v => v.TaxAmountGreaterThanOrEqualToZero());
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasTotalPrice(string expectedTotalPrice)
        {
            _validations.Add(v => v.TotalPrice(expectedTotalPrice));
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasTotalPriceGreaterThanZero()
        {
            _validations.Add(v => v.TotalPriceGreaterThanZero());
            return this;
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> HasOrderNumberPrefix(string expectedPrefix)
        {
            _validations.Add(v => v.OrderNumberHasPrefix(expectedPrefix));
            return this;
        }

        // Make this awaitable by implementing GetAwaiter
        public TaskAwaiter GetAwaiter()
        {
            return ExecuteValidations().GetAwaiter();
        }

        // Allow implicit conversion to Task for method return types
        public static implicit operator Task(ThenOrderBuilder<TSuccessResponse, TSuccessVerification> builder)
        {
            return builder.ExecuteValidations();
        }
    }
}