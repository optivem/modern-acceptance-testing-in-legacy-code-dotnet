namespace Optivem.Testing.Channel;

/// <summary>
/// Factory interface for creating channel-specific drivers.
/// Implement this interface to provide custom driver creation logic.
/// </summary>
/// <typeparam name="TDriver">The type of driver to create</typeparam>
public interface IChannelDriverFactory<out TDriver>
{
    /// <summary>
    /// Creates a driver instance for the specified channel type.
    /// </summary>
    /// <param name="channelType">The channel type (e.g., "UI", "API")</param>
    /// <returns>An instance of the driver for the specified channel</returns>
    TDriver CreateDriver(string channelType);
}
