using SmallEarthTech.AntRadioInterface;

namespace AntPlusServer.Services
{
    /// <summary>
    /// Defines a contract for subscribing to ANT radio responses and managing the lifetime of the subscription.
    /// </summary>
    /// <remarks>Implementations of this interface allow clients to receive ANT radio response events and
    /// ensure proper resource cleanup by implementing IDisposable. Subscribers should attach event handlers to
    /// OnAntRadioResponse to process incoming ANT responses.</remarks>
    public interface IAntRadioSubscriber : IDisposable
    {
        /// <summary>
        /// Occurs when a response is received from the ANT radio.
        /// </summary>
        /// <remarks>Subscribe to this event to handle incoming ANT radio responses. The event provides an
        /// <see cref="AntResponse"/> object containing details of the response message.</remarks>
        event EventHandler<AntResponse> OnAntRadioResponse;
    }
}
