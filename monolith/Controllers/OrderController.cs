using Microsoft.AspNetCore.Mvc;
using Optivem.EShop.Monolith.Core.DTOs;
using Optivem.EShop.Monolith.Core.Services;

namespace Optivem.EShop.Monolith.Controllers;

[ApiController]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("/api/orders")]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
    {
        var response = await _orderService.PlaceOrderAsync(request);
        return Created($"/api/orders/{response.OrderNumber}", response);
    }

    [HttpGet("/api/orders/{orderNumber}")]
    public async Task<IActionResult> GetOrder(string orderNumber)
    {
        var response = await _orderService.GetOrderAsync(orderNumber);
        return Ok(response);
    }

    [HttpPost("/api/orders/{orderNumber}/cancel")]
    public async Task<IActionResult> CancelOrder(string orderNumber)
    {
        await _orderService.CancelOrderAsync(orderNumber);
        return NoContent();
    }
}
