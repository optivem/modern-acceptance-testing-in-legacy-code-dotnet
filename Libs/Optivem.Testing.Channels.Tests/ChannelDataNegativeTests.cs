using Xunit;

namespace Optivem.Testing.Channels.Tests;

/// <summary>
/// Manual validation tests for negative scenarios.
/// These tests demonstrate that ChannelData properly detects and rejects
/// incorrect usage of standard xUnit attributes (InlineData, ClassData, MemberData).
/// 
/// INSTRUCTIONS TO VERIFY:
/// Uncomment ONE test at a time and run the tests.
/// Each test should fail during test discovery with a helpful error message
/// guiding the user to use the correct Channel-specific attribute instead.
/// </summary>
public class ChannelDataNegativeTests
{
    /*
    // NEGATIVE TEST 1: Using [InlineData] with [ChannelData]
    // Expected Error: "Cannot use [InlineData] with [ChannelData]. Use [ChannelInlineData] instead.
    // Example: [ChannelData("UI", "API")] [ChannelInlineData("value1", "message1")]"
    
    [Theory]
    [ChannelData("UI", "API")]
    [InlineData("value1")]
    public void ShouldFail_WhenUsingInlineDataWithChannelData(Channel channel, string value)
    {
        // This test should never execute - it should fail at test discovery
        Assert.True(false, "This test should have failed during test discovery");
    }
    */

    /*
    // NEGATIVE TEST 2: Using [ClassData] with [ChannelData]
    // Expected Error: "Cannot use [ClassData] with [ChannelData]. Use [ChannelClassData] instead.
    // Example: [ChannelData("UI", "API")] [ChannelClassData(typeof(YourDataProvider))]"
    
    public class SampleDataProvider : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "value1" };
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Theory]
    [ChannelData("UI", "API")]
    [ClassData(typeof(SampleDataProvider))]
    public void ShouldFail_WhenUsingClassDataWithChannelData(Channel channel, string value)
    {
        // This test should never execute - it should fail at test discovery
        Assert.True(false, "This test should have failed during test discovery");
    }
    */

    /*
    // NEGATIVE TEST 3: Using [MemberData] with [ChannelData]
    // Expected Error: "Cannot use [MemberData] with [ChannelData]. Use [ChannelMemberData] instead.
    // Example: [ChannelData("UI", "API")] [ChannelMemberData(nameof(YourMethod))]"
    
    public static IEnumerable<object[]> GetSampleData()
    {
        yield return new object[] { "value1" };
    }

    [Theory]
    [ChannelData("UI", "API")]
    [MemberData(nameof(GetSampleData))]
    public void ShouldFail_WhenUsingMemberDataWithChannelData(Channel channel, string value)
    {
        // This test should never execute - it should fail at test discovery
        Assert.True(false, "This test should have failed during test discovery");
    }
    */

    /*
    // NEGATIVE TEST 4: Using [Fact] instead of [Theory] with [ChannelData]
    // Expected Error: "[ChannelData] requires [Theory], not [Fact]. Change [Fact] to [Theory].
    // Example: [Theory] [ChannelData("UI", "API")] public void Test(Channel channel) { }"
    
    [Fact]
    [ChannelData("UI", "API")]
    public void ShouldFail_WhenUsingFactInsteadOfTheory(Channel channel)
    {
        // This test should never execute - it should fail at test discovery
        Assert.True(false, "This test should have failed during test discovery");
    }
    */

    /*
    // NEGATIVE TEST 5: Missing [Theory] attribute with [ChannelData]
    // LIMITATION: This cannot be detected at test discovery time!
    // 
    // Without [Theory], xUnit doesn't recognize this as a theory test,
    // so it never calls ChannelDataAttribute.GetData() where our validation lives.
    // 
    // Result: xUnit will ignore this method entirely (not discover it as a test).
    // This is a limitation of xUnit's architecture, not our validation.
    // 
    // The test will simply be silently skipped during test discovery.
    // You won't see it in your test runner at all.
    //
    // BEST PRACTICE: Always use [Theory] with [ChannelData]
    
    [ChannelData("UI", "API")]
    public void ShouldFail_WhenMissingTheoryAttribute_ButWont(Channel channel)
    {
        // This method is never discovered as a test because it lacks [Theory]
        // xUnit ignores methods with only DataAttributes and no [Theory]
        Assert.True(false, "This will never execute - method is not discovered");
    }
    */

    [Fact]
    public void Documentation_NegativeTestsAreCommentedOut()
    {
        // This is a documentation test that passes.
        // To verify the negative test behavior, uncomment one test at a time above
        // and verify that it fails during test discovery with a helpful error message.
        Assert.True(true);
    }
}
