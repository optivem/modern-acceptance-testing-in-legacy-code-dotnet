using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Optivem.Commons.Dsl;
using Shouldly;

namespace Optivem.EShop.SystemTest.Core.Clock.Dsl.Verifications;

public class GetTimeVerification : ResponseVerification<GetTimeResponse>
{
    public GetTimeVerification(GetTimeResponse response, UseCaseContext context) 
        : base(response, context)
    {
    }

    public GetTimeVerification TimeIsNotNull()
    {
        Response.Time.ShouldNotBe(default, 
            "Time should not be null or default");
        return this;
    }

    public GetTimeVerification Time(DateTimeOffset expectedTime)
    {
        Response.Time.ShouldBe(expectedTime, 
            $"Expected time: {expectedTime:O}, but got: {Response.Time:O}");
        return this;
    }

    public GetTimeVerification Time(string expectedTimeString)
    {
        var expectedTime = DateTimeOffset.Parse(expectedTimeString);
        return Time(expectedTime);
    }

    public GetTimeVerification TimeIsAfter(DateTimeOffset time)
    {
        Response.Time.ShouldBeGreaterThan(time, 
            $"Expected time to be after {time:O}, but was {Response.Time:O}");
        return this;
    }

    public GetTimeVerification TimeIsAfter(string timeString)
    {
        var time = DateTimeOffset.Parse(timeString);
        return TimeIsAfter(time);
    }

    public GetTimeVerification TimeIsBefore(DateTimeOffset time)
    {
        Response.Time.ShouldBeLessThan(time, 
            $"Expected time to be before {time:O}, but was {Response.Time:O}");
        return this;
    }

    public GetTimeVerification TimeIsBefore(string timeString)
    {
        var time = DateTimeOffset.Parse(timeString);
        return TimeIsBefore(time);
    }

    public GetTimeVerification TimeIsBetween(DateTimeOffset start, DateTimeOffset end)
    {
        Response.Time.ShouldBeGreaterThanOrEqualTo(start, 
            $"Expected time to be at or after {start:O}, but was {Response.Time:O}");
        Response.Time.ShouldBeLessThanOrEqualTo(end, 
            $"Expected time to be at or before {end:O}, but was {Response.Time:O}");
        return this;
    }

    public GetTimeVerification TimeIsBetween(string startString, string endString)
    {
        var start = DateTimeOffset.Parse(startString);
        var end = DateTimeOffset.Parse(endString);
        return TimeIsBetween(start, end);
    }
}
