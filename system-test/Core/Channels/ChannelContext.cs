namespace Optivem.EShop.SystemTest.Core.Channels;

/// <summary>
/// Thread-local context for managing the current channel during test execution.
/// </summary>
public static class ChannelContext
{
    private static readonly AsyncLocal<string?> _current = new();

    /// <summary>
    /// Sets the current channel for the executing thread.
    /// </summary>
    public static void Set(string channel)
    {
        _current.Value = channel;
    }

    /// <summary>
    /// Gets the current channel for the executing thread.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no channel is set</exception>
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

    /// <summary>
    /// Clears the current channel for the executing thread.
    /// </summary>
    public static void Clear()
    {
        _current.Value = null;
    }
}
