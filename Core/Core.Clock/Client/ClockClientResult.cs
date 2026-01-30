using Commons.Util;
using Optivem.EShop.SystemTest.Core.Clock.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Clock.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Clock.Client;

public class ClockClientResult : Result<VoidValue, ExtClockErrorResponse>
{
    private ClockClientResult(bool isSuccess, VoidValue? value, ExtClockErrorResponse? error) 
        : base(isSuccess, value, error) { }
    
    public static ClockClientResult Success() 
        => new(true, VoidValue.Empty, default);
    
    public static new ClockClientResult Failure(ExtClockErrorResponse error) 
        => new(false, default, error);
}

public class ClockClientResult<T> : Result<T, ExtClockErrorResponse>
{
    private ClockClientResult(bool isSuccess, T? value, ExtClockErrorResponse? error) 
        : base(isSuccess, value, error) { }
    
    public static new ClockClientResult<T> Success(T value) 
        => new(true, value, default);
    
    public static new ClockClientResult<T> Failure(ExtClockErrorResponse error) 
        => new(false, default, error);

    public new ClockClientResult<T2> Map<T2>(Func<T, T2> mapper)
    {
        return IsSuccess 
            ? ClockClientResult<T2>.Success(mapper(Value)) 
            : ClockClientResult<T2>.Failure(Error);
    }
}
