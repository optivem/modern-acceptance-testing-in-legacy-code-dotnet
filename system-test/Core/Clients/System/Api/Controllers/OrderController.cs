using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Api.Dtos;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Api.Controllers;

public class OrderController
{
    private const string Endpoint = "/orders";

    private readonly TestHttpClient _httpClient;

    public OrderController(TestHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> PlaceOrderAsync(string sku, string quantity, string country)
    {
        var request = new PlaceOrderRequest
        {
            Sku = sku,
            Quantity = quantity,
            Country = country
        };

        return await _httpClient.PostAsync(Endpoint, request);
    }

    public async Task<PlaceOrderResponse> AssertOrderPlacedSuccessfullyAsync(HttpResponseMessage httpResponse)
    {
        _httpClient.AssertCreated(httpResponse);
        return await _httpClient.ReadBodyAsync<PlaceOrderResponse>(httpResponse);
    }

    public void AssertOrderPlacementFailed(HttpResponseMessage httpResponse)
    {
        _httpClient.AssertUnprocessableEntity(httpResponse);
    }

    public async Task<string> GetErrorMessageAsync(HttpResponseMessage httpResponse)
    {
        return await httpResponse.Content.ReadAsStringAsync();
    }

    public async Task<HttpResponseMessage> ViewOrderAsync(string orderNumber)
    {
        return await _httpClient.GetAsync($"{Endpoint}/{orderNumber}");
    }

    public async Task<GetOrderResponse> AssertOrderViewedSuccessfullyAsync(HttpResponseMessage httpResponse)
    {
        _httpClient.AssertOk(httpResponse);
        return await _httpClient.ReadBodyAsync<GetOrderResponse>(httpResponse);
    }

    public async Task<HttpResponseMessage> CancelOrderAsync(string orderNumber)
    {
        return await _httpClient.PostAsync($"{Endpoint}/{orderNumber}/cancel");
    }

    public void AssertOrderCancelledSuccessfully(HttpResponseMessage httpResponse)
    {
        _httpClient.AssertNoContent(httpResponse);
    }
}
