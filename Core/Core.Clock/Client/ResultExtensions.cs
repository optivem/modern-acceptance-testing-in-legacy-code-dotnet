using Commons.Util;
using Optivem.EShop.SystemTest.Core.Clock.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Clock.Client;

public static class ResultExtensions
{
    public static ClockClientResult ToResult(this Result<VoidValue, ExtClockErrorResponse> result)
    {
        return result.IsSuccess 
            ? ClockClientResult.Success() 
            : ClockClientResult.Failure(result.Error);
    }

    public static ClockClientResult<T> ToResult<T>(this Result<T, ExtClockErrorResponse> result)
    {
        return result.IsSuccess 
            ? ClockClientResult<T>.Success(result.Value) 
            : ClockClientResult<T>.Failure(result.Error);
    }

    public static async Task<ClockClientResult> ToResultAsync(this Task<Result<VoidValue, ExtClockErrorResponse>> resultTask)
    {
        var result = await resultTask;
        return result.ToResult();
    }

    public static async Task<ClockClientResult<T>> ToResultAsync<T>(this Task<Result<T, ExtClockErrorResponse>> resultTask)
    {
        var result = await resultTask;
        return result.ToResult();
    }
}
