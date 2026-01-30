using Commons.Util;
using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;

namespace Optivem.EShop.SystemTest.Core.Clock.Driver;

public class ClockResult : Result<VoidValue, ClockErrorResponse>
{
    private ClockResult(bool isSuccess, VoidValue? value, ClockErrorResponse? error) 
        : base(isSuccess, value, error) { }
    
    public static ClockResult Success() 
        => new(true, VoidValue.Empty, default);
    
    public static new ClockResult Failure(ClockErrorResponse error) 
        => new(false, default, error);
}

public class ClockResult<T> : Result<T, ClockErrorResponse>
{
    private ClockResult(bool isSuccess, T? value, ClockErrorResponse? error) 
        : base(isSuccess, value, error) { }
    
    public static new ClockResult<T> Success(T value) 
        => new(true, value, default);
    
    public static new ClockResult<T> Failure(ClockErrorResponse error) 
        => new(false, default, error);
}
