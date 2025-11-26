using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Dtos;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Results;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Api.Controllers;

public class OrderController
{
    private const string Endpoint = "/api/orders";

    private readonly TestHttpClient _httpClient;

    public OrderController(TestHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Result<PlaceOrderResponse> PlaceOrder(string? sku, string? quantity, string? country)
    {
        var request = new PlaceOrderRequest
        {
            Sku = sku,
            Quantity = quantity,
            Country = country
        };

        var httpResponse = _httpClient.Post(Endpoint, request);
        return TestHttpUtils.GetCreatedResultOrFailure<PlaceOrderResponse>(httpResponse);
    }

    public Result<GetOrderResponse> ViewOrder(string orderNumber)
    {
        var httpResponse = _httpClient.Get($"{Endpoint}/{orderNumber}");
        return TestHttpUtils.GetOkResultOrFailure<GetOrderResponse>(httpResponse);
    }

    public Result<object?> CancelOrder(string orderNumber)
    {
        var httpResponse = _httpClient.Post($"{Endpoint}/{orderNumber}/cancel");
        return TestHttpUtils.GetNoContentResultOrFailure(httpResponse);
    }
}
