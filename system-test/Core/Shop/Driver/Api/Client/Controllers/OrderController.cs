using Optivem.Lang;
using Optivem.Testing.Assertions;
using Optivem.Http;
using Optivem.Playwright;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Responses;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Requests;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Api.Client.Controllers;

public class OrderController
{
    private const string Endpoint = "/api/orders";

    private readonly JsonHttpClient _httpClient;

    public OrderController(JsonHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Result<PlaceOrderResponse, Error> PlaceOrder(PlaceOrderRequest request)
    {
        var httpResponse = _httpClient.Post(Endpoint, request);
        return HttpUtils.GetCreatedResultOrFailure<PlaceOrderResponse>(httpResponse);
    }

    public Result<GetOrderResponse, Error> ViewOrder(string orderNumber)
    {
        var httpResponse = _httpClient.Get($"{Endpoint}/{orderNumber}");
        return HttpUtils.GetOkResultOrFailure<GetOrderResponse>(httpResponse);
    }

    public Result<VoidValue, Error> CancelOrder(string orderNumber)
    {
        var httpResponse = _httpClient.Post($"{Endpoint}/{orderNumber}/cancel");
        return HttpUtils.GetNoContentResultOrFailure(httpResponse);
    }
}
