namespace Optivem.EShop.SystemTest.Core.Common.Error;

public static class ProblemDetailConverter
{
    public static Error ToError(ProblemDetailResponse problemDetail)
    {
        var message = problemDetail.Detail ?? "Request failed";

        if (problemDetail.Errors != null && problemDetail.Errors.Any())
        {
            var fieldErrors = problemDetail.Errors
                .Select(e => new Error.FieldError(e.Field ?? "unknown", e.Message ?? string.Empty, e.Code))
                .ToList();
            return Error.Of(message, fieldErrors.AsReadOnly());
        }

        return Error.Of(message);
    }
}
