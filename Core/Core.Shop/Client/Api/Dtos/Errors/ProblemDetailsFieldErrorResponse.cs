namespace Optivem.EShop.SystemTest.Core.Shop.Client.Api.Dtos.Errors;

public class ProblemDetailsFieldErrorResponse
{
    public string? Field { get; set; }
    public string? Message { get; set; }
    public string? Code { get; set; }
    public object? RejectedValue { get; set; }
}