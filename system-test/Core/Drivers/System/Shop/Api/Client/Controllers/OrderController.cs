using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Dtos;

namespace Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Api.Client.Controllers;

public class OrderController
{
    private const string Endpoint = "/api/orders";

    private readonly HttpGateway _httpClient;

    public OrderController(HttpGateway httpClient)
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
        return HttpUtils.GetCreatedResultOrFailure<PlaceOrderResponse>(httpResponse);
    }

    public Result<GetOrderResponse> ViewOrder(string orderNumber)
    {
        var httpResponse = _httpClient.Get($"{Endpoint}/{orderNumber}");
        return HttpUtils.GetOkResultOrFailure<GetOrderResponse>(httpResponse);
    }

    public Result<VoidResult> CancelOrder(string orderNumber)
    {
        var httpResponse = _httpClient.Post($"{Endpoint}/{orderNumber}/cancel");
        return HttpUtils.GetNoContentResultOrFailure(httpResponse);
    }
}
