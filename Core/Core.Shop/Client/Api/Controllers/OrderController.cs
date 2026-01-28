using Optivem.Commons.Util;
using Optivem.Commons.Http;
using Optivem.EShop.SystemTest.Core.Shop.Client.Api.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Api.Controllers;

public class OrderController
{
    private const string Endpoint = "/api/orders";

    private readonly JsonHttpClient<ProblemDetailResponse> _httpClient;

    public OrderController(JsonHttpClient<ProblemDetailResponse> httpClient)
    {
        _httpClient = httpClient;
    }

    public Result<PlaceOrderResponse, ProblemDetailResponse> PlaceOrder(PlaceOrderRequest request)
    {
        return _httpClient.Post<PlaceOrderResponse>(Endpoint, request);
    }

    public Result<ViewOrderResponse, ProblemDetailResponse> ViewOrder(string? orderNumber)
    {
        return _httpClient.Get<ViewOrderResponse>($"{Endpoint}/{orderNumber}");
    }

    public Result<VoidValue, ProblemDetailResponse> CancelOrder(string? orderNumber)
    {
        return _httpClient.Post($"{Endpoint}/{orderNumber}/cancel");
    }
}
