using Commons.Util;
using Commons.Http;
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

    public async Task<Result<PlaceOrderResponse, ProblemDetailResponse>> PlaceOrder(PlaceOrderRequest request)
    {
        return await _httpClient.Post<PlaceOrderResponse>(Endpoint, request);
    }

    public async Task<Result<ViewOrderResponse, ProblemDetailResponse>> ViewOrder(string orderNumber)
    {
        return await _httpClient.Get<ViewOrderResponse>($"{Endpoint}/{orderNumber}");
    }

    public async Task<Result<VoidValue, ProblemDetailResponse>> CancelOrder(string orderNumber)
    {
        return await _httpClient.Post($"{Endpoint}/{orderNumber}/cancel");
    }
}
