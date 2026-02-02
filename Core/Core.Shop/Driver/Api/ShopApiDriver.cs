using Commons.Util;
using Commons.Http;
using Optivem.EShop.SystemTest.Core.Shop.Client.Api.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Shop.Client.Api;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Internal;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Api.Internal;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Api;

public class ShopApiDriver : IShopDriver
{
    private readonly ShopApiClient _apiClient;
    private readonly IOrderDriver _orderDriver;
    private readonly ICouponDriver _couponDriver;

    public ShopApiDriver(string baseUrl)
    {
        _apiClient = new ShopApiClient(baseUrl);
        _orderDriver = new ShopApiOrderDriver(_apiClient);
        _couponDriver = new ShopApiCouponDriver(_apiClient);
    }

    public ValueTask DisposeAsync()
    {
        _apiClient?.Dispose();
        return ValueTask.CompletedTask;
    }

    public Task<Result<VoidValue, SystemError>> GoToShop()
        => _apiClient.Health().CheckHealth()
            .MapErrorAsync(SystemError.From);

    public IOrderDriver Orders() => _orderDriver;
    
    public ICouponDriver Coupons() => _couponDriver;
}
