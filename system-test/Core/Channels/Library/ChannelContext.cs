namespace Optivem.EShop.SystemTest.Core.Channels.Library;

public static class ChannelContext
{
    private static readonly AsyncLocal<string?> _current = new();

    public static void Set(string channel)
    {
        _current.Value = channel;
    }

    public static string Get()
    {
        var channel = _current.Value;
        if (channel == null)
        {
            throw new InvalidOperationException(
                "Channel type is not set. Please ensure that the test method is annotated with [Theory] and [ChannelData]");
        }
        return channel;
    }

    public static void Clear()
    {
        _current.Value = null;
    }
}
