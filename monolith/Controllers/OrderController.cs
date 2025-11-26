using Microsoft.AspNetCore.Mvc;
using Optivem.AtddAccelerator.EShop.Monolith.Core.DTOs;
using Optivem.AtddAccelerator.EShop.Monolith.Core.Services;

namespace Optivem.AtddAccelerator.EShop.Monolith.Controllers;

[ApiController]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("/api/orders")]
    public async Task<ActionResult<PlaceOrderResponse>> PlaceOrder([FromBody] PlaceOrderRequest request)
    {
        var response = await _orderService.PlaceOrderAsync(request);
        var location = $"/api/orders/{response.OrderNumber}";
        return Created(location, response);
    }

    [HttpGet("/api/orders/{orderNumber}")]
    public ActionResult<GetOrderResponse> GetOrder(string orderNumber)
    {
        var response = _orderService.GetOrder(orderNumber);
        return Ok(response);
    }

    [HttpPost("/api/orders/{orderNumber}/cancel")]
    public IActionResult CancelOrder(string orderNumber)
    {
        _orderService.CancelOrder(orderNumber);
        return NoContent();
    }
}
