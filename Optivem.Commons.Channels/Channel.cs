namespace Optivem.Commons.Channels
{
    /// <summary>
    /// Represents a test execution channel (e.g., UI, API).
    /// Generic library class - not coupled to specific implementations.
    /// </summary>
    public class Channel
    {
        private readonly string _channelType;

        public Channel(string channelType)
        {
            _channelType = channelType ?? throw new ArgumentNullException(nameof(channelType));
        }

        /// <summary>
        /// Gets the channel type identifier (e.g., "UI", "API").
        /// </summary>
        public string Type => _channelType;

        /// <summary>
        /// Creates a driver using the provided factory.
        /// This keeps the Channel class generic and reusable.
        /// </summary>
        /// <typeparam name="TDriver">The type of driver to create</typeparam>
        /// <param name="factory">Factory responsible for creating channel-specific drivers</param>
        /// <returns>A driver instance for this channel</returns>
        public TDriver CreateDriver<TDriver>(IChannelDriverFactory<TDriver> factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return factory.CreateDriver(_channelType);
        }

        public override string ToString() => _channelType;

        public override bool Equals(object? obj)
        {
            return obj is Channel other && _channelType == other._channelType;
        }

        public override int GetHashCode() => _channelType.GetHashCode();
    }
}
