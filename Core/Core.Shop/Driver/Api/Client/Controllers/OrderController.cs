using Optivem.Commons.Util;
using Optivem.Commons.Http;
using Optivem.EShop.SystemTest.Core.Common.Error;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Responses;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Requests;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Api.Client.Controllers;

public class OrderController
{
    private const string Endpoint = "/api/orders";

    private readonly JsonHttpClient<ProblemDetailResponse> _httpClient;

    public OrderController(JsonHttpClient<ProblemDetailResponse> httpClient)
    {
        _httpClient = httpClient;
    }

    public Result<PlaceOrderResponse, Error> PlaceOrder(PlaceOrderRequest request)
    {
        return _httpClient.Post<PlaceOrderResponse>(Endpoint, request)
            .MapFailure(ProblemDetailConverter.ToError);
    }

    public Result<GetOrderResponse, Error> ViewOrder(string orderNumber)
    {
        return _httpClient.Get<GetOrderResponse>($"{Endpoint}/{orderNumber}")
            .MapFailure(ProblemDetailConverter.ToError);
    }

    public Result<VoidValue, Error> CancelOrder(string orderNumber)
    {
        return _httpClient.Post($"{Endpoint}/{orderNumber}/cancel")
            .MapFailure(ProblemDetailConverter.ToError);
    }
}
