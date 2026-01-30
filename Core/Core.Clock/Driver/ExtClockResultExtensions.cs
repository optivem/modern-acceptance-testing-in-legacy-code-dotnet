using Optivem.EShop.SystemTest.Core.Clock.Client;
using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;

namespace Optivem.EShop.SystemTest.Core.Clock.Driver;

public static class ExtClockResultExtensions
{
    public static ClockResult ToResult(this ExtClockResult result)
    {
        return result.IsSuccess 
            ? ClockResult.Success() 
            : ClockResult.Failure(ClockErrorResponse.From(result.Error));
    }

    public static ClockResult<T> ToResult<T>(this ExtClockResult<T> result)
    {
        return result.IsSuccess 
            ? ClockResult<T>.Success(result.Value) 
            : ClockResult<T>.Failure(ClockErrorResponse.From(result.Error));
    }

    public static ClockResult<T> ToResult<T, TSource>(this ExtClockResult<TSource> result, Func<TSource, T> mapper)
    {
        return result.IsSuccess 
            ? ClockResult<T>.Success(mapper(result.Value)) 
            : ClockResult<T>.Failure(ClockErrorResponse.From(result.Error));
    }

    public static async Task<ClockResult> ToResultAsync(this Task<ExtClockResult> resultTask)
    {
        var result = await resultTask;
        return result.ToResult();
    }

    public static async Task<ClockResult<T>> ToResultAsync<T>(this Task<ExtClockResult<T>> resultTask)
    {
        var result = await resultTask;
        return result.ToResult();
    }

    public static async Task<ClockResult<T>> ToResultAsync<T, TSource>(
        this Task<ExtClockResult<TSource>> resultTask, 
        Func<TSource, T> mapper)
    {
        var result = await resultTask;
        return result.ToResult(mapper);
    }

    public static async Task<ExtClockResult<T2>> MapAsync<T, T2>(
        this Task<ExtClockResult<T>> resultTask,
        Func<T, T2> mapper)
    {
        var result = await resultTask;
        return result.Map(mapper);
    }
}
