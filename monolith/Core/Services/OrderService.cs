using Optivem.AtddAccelerator.EShop.Monolith.Core.DTOs;
using Optivem.AtddAccelerator.EShop.Monolith.Core.Entities;
using Optivem.AtddAccelerator.EShop.Monolith.Core.Exceptions;
using Optivem.AtddAccelerator.EShop.Monolith.Core.Repositories;
using Optivem.AtddAccelerator.EShop.Monolith.Core.Services.External;

namespace Optivem.AtddAccelerator.EShop.Monolith.Core.Services;

public class OrderService
{
    private static readonly TimeOnly OrderPlacementCutoffTime = new TimeOnly(17, 0);
    private static readonly TimeOnly CancellationBlockStart = new TimeOnly(22, 0);
    private static readonly TimeOnly CancellationBlockEnd = new TimeOnly(23, 0);

    private readonly IOrderRepository _orderRepository;
    private readonly ErpGateway _erpGateway;
    private readonly TaxGateway _taxGateway;

    public OrderService(IOrderRepository orderRepository, ErpGateway erpGateway, TaxGateway taxGateway)
    {
        _orderRepository = orderRepository;
        _erpGateway = erpGateway;
        _taxGateway = taxGateway;
    }

    public async Task<PlaceOrderResponse> PlaceOrderAsync(PlaceOrderRequest request)
    {
        var sku = request.Sku;
        var quantity = request.Quantity!.Value;
        var country = request.Country;

        var orderNumber = GenerateOrderNumber();
        var orderTimestamp = DateTime.UtcNow;
        var unitPrice = await GetUnitPriceAsync(sku);
        var discountRate = GetDiscountRate();
        var taxRate = await GetTaxRateAsync(country);

        var originalPrice = unitPrice * quantity;
        var discountAmount = originalPrice * discountRate;
        var subtotalPrice = originalPrice - discountAmount;
        var taxAmount = subtotalPrice * taxRate;
        var totalPrice = subtotalPrice + taxAmount;

        var order = new Order(orderNumber, orderTimestamp, country,
                sku, quantity, unitPrice, originalPrice,
                discountRate, discountAmount, subtotalPrice,
                taxRate, taxAmount, totalPrice, OrderStatus.PLACED);

        _orderRepository.Save(order);

        return new PlaceOrderResponse { OrderNumber = orderNumber };
    }

    private async Task<decimal> GetUnitPriceAsync(string sku)
    {
        var productDetails = await _erpGateway.GetProductDetailsAsync(sku);
        if (productDetails == null)
        {
            throw new ValidationException($"Product does not exist for SKU: {sku}");
        }

        return productDetails.Price;
    }

    private decimal GetDiscountRate()
    {
        var now = DateTime.Now;
        var currentTime = TimeOnly.FromDateTime(now);

        if (currentTime <= OrderPlacementCutoffTime)
        {
            return 0m;
        }

        return 0.15m;
    }

    private async Task<decimal> GetTaxRateAsync(string country)
    {
        var countryDetails = await _taxGateway.GetTaxDetailsAsync(country);
        if (countryDetails == null)
        {
            throw new ValidationException($"Country does not exist: {country}");
        }

        return countryDetails.TaxRate;
    }

    public GetOrderResponse GetOrder(string orderNumber)
    {
        var order = _orderRepository.FindById(orderNumber);

        if (order == null)
        {
            throw new NotExistValidationException($"Order {orderNumber} does not exist.");
        }

        return new GetOrderResponse
        {
            OrderNumber = orderNumber,
            Sku = order.Sku,
            Quantity = order.Quantity,
            UnitPrice = order.UnitPrice,
            OriginalPrice = order.OriginalPrice,
            DiscountRate = order.DiscountRate,
            DiscountAmount = order.DiscountAmount,
            SubtotalPrice = order.SubtotalPrice,
            TaxRate = order.TaxRate,
            TaxAmount = order.TaxAmount,
            TotalPrice = order.TotalPrice,
            Status = order.Status,
            Country = order.Country
        };
    }

    public void CancelOrder(string orderNumber)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
        {
            throw new ValidationException("Order number must not be empty");
        }

        var order = _orderRepository.FindById(orderNumber);

        if (order == null)
        {
            throw new NotExistValidationException($"Order {orderNumber} does not exist.");
        }

        if (order.Status == OrderStatus.CANCELLED)
        {
            throw new ValidationException("Order has already been cancelled");
        }

        var now = DateTime.Now;
        var currentDate = new DateTime(now.Year, 12, 31);
        var isDecember31 = now.Month == 12 && now.Day == 31;
        var currentTime = TimeOnly.FromDateTime(now);

        if (isDecember31 && currentTime > CancellationBlockStart && currentTime < CancellationBlockEnd)
        {
            throw new ValidationException("Order cancellation is not allowed on December 31st between 22:00 and 23:00");
        }

        order.Status = OrderStatus.CANCELLED;
        _orderRepository.Save(order);
    }

    private string GenerateOrderNumber()
    {
        return $"ORD-{Guid.NewGuid()}";
    }
}
