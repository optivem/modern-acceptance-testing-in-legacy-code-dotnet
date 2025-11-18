using Optivem.EShop.Monolith.Core.DTOs;
using Optivem.EShop.Monolith.Core.Entities;
using Optivem.EShop.Monolith.Core.Exceptions;
using Optivem.EShop.Monolith.Core.Repositories;
using Optivem.EShop.Monolith.Core.Services.External;

namespace Optivem.EShop.Monolith.Core.Services;

public class OrderService
{
    private static readonly TimeOnly OrderPlacementCutoffTime = new(17, 0);
    private static readonly TimeOnly CancellationBlockStart = new(22, 0);
    private static readonly TimeOnly CancellationBlockEnd = new(23, 0);
    
    private readonly OrderRepository _orderRepository;
    private readonly ErpGateway _erpGateway;
    private readonly TaxGateway _taxGateway;

    public OrderService(OrderRepository orderRepository, ErpGateway erpGateway, TaxGateway taxGateway)
    {
        _orderRepository = orderRepository;
        _erpGateway = erpGateway;
        _taxGateway = taxGateway;
    }

    public async Task<PlaceOrderResponse> PlaceOrderAsync(PlaceOrderRequest request)
    {
        var sku = request.Sku!;
        var quantity = request.Quantity!.Value;
        var country = request.Country!;

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

        var order = new Order(
            orderNumber,
            orderTimestamp,
            country,
            sku,
            quantity,
            unitPrice,
            originalPrice,
            discountRate,
            discountAmount,
            subtotalPrice,
            taxRate,
            taxAmount,
            totalPrice,
            OrderStatus.PLACED
        );

        await _orderRepository.SaveAsync(order);

        return new PlaceOrderResponse
        {
            OrderNumber = orderNumber
        };
    }

    public async Task<GetOrderResponse> GetOrderAsync(string orderNumber)
    {
        var order = await _orderRepository.FindByIdAsync(orderNumber);

        if (order == null)
        {
            throw new NotExistValidationException($"Order {orderNumber} does not exist.");
        }

        return new GetOrderResponse
        {
            OrderNumber = order.OrderNumber,
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

    public async Task CancelOrderAsync(string orderNumber)
    {
        var order = await _orderRepository.FindByIdAsync(orderNumber);

        if (order == null)
        {
            throw new NotExistValidationException($"Order {orderNumber} does not exist.");
        }

        if (order.Status == OrderStatus.CANCELLED)
        {
            throw new ValidationException($"Order {orderNumber} has already been cancelled.");
        }

        var now = DateTime.Now;
        var currentTime = TimeOnly.FromDateTime(now);
        var isDecember31 = now.Month == 12 && now.Day == 31;

        if (isDecember31 && 
            currentTime > CancellationBlockStart && 
            currentTime < CancellationBlockEnd)
        {
            throw new ValidationException("Order cancellation is not allowed on December 31st between 22:00 and 23:00");
        }

        order.Status = OrderStatus.CANCELLED;
        await _orderRepository.UpdateAsync(order);
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
        var currentTime = TimeOnly.FromDateTime(DateTime.Now);

        if (currentTime <= OrderPlacementCutoffTime)
        {
            return 0.0m;
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

    private string GenerateOrderNumber()
    {
        return $"ORD-{Guid.NewGuid()}";
    }
}
