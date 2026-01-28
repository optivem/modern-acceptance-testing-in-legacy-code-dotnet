using Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;

namespace Optivem.EShop.SystemTest.Core.Shop.Commons;

public static class SystemResults
{
    public static Result<T, SystemError> Success<T>(T value)
    {
        return Result<T, SystemError>.Success(value);
    }

    public static Result<VoidValue, SystemError> Success()
    {
        return Result<VoidValue, SystemError>.Success(VoidValue.Empty);
    }

    public static Result<T, SystemError> Failure<T>(string message)
    {
        return Result<T, SystemError>.Failure(SystemError.Of(message));
    }

    public static Result<T, SystemError> Failure<T>(SystemError error)
    {
        return Result<T, SystemError>.Failure(error);
    }
}

public static class SystemResultExtensions
{
    public static Result<VoidValue, SystemError> MapVoid<T>(this Result<T, SystemError> result)
    {
        if (result.IsSuccess)
        {
            return SystemResults.Success();
        }
        return SystemResults.Failure<VoidValue>(result.Error);
    }
}