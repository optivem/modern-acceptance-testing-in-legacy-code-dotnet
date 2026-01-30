using Commons.Util;
using Optivem.EShop.SystemTest.Core.Clock.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Clock.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Clock.Client;

public class ExtClockResult : Result<VoidValue, ExtClockErrorResponse>
{
    private ExtClockResult(bool isSuccess, VoidValue? value, ExtClockErrorResponse? error) 
        : base(isSuccess, value, error) { }
    
    public static ExtClockResult Success() 
        => new(true, VoidValue.Empty, default);
    
    public static new ExtClockResult Failure(ExtClockErrorResponse error) 
        => new(false, default, error);
}

public class ExtClockResult<T> : Result<T, ExtClockErrorResponse>
{
    private ExtClockResult(bool isSuccess, T? value, ExtClockErrorResponse? error) 
        : base(isSuccess, value, error) { }
    
    public static new ExtClockResult<T> Success(T value) 
        => new(true, value, default);
    
    public static new ExtClockResult<T> Failure(ExtClockErrorResponse error) 
        => new(false, default, error);

    public new ExtClockResult<T2> Map<T2>(Func<T, T2> mapper)
    {
        return IsSuccess 
            ? ExtClockResult<T2>.Success(mapper(Value)) 
            : ExtClockResult<T2>.Failure(Error);
    }
}
