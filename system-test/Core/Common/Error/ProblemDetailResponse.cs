namespace Optivem.EShop.SystemTest.Core.Common.Error;

public class ProblemDetailResponse
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public string? Detail { get; set; }
    public string? Instance { get; set; }
    public string? Timestamp { get; set; }
    public List<ProblemDetailsFieldErrorResponse>? Errors { get; set; }
}
