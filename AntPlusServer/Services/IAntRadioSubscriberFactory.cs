using SmallEarthTech.AntRadioInterface;

namespace AntPlusServer.Services
{
    /// <summary>
    /// Defines a factory for creating instances of AntRadio subscribers.
    /// </summary>
    public interface IAntRadioSubscriberFactory
    {
        /// <summary>
        /// Creates a new instance of the AntRadioSubscriber.
        /// </summary>
        /// <param name="antRadio">The AntRadio instance to be used by the subscriber.</param>
        /// <returns>A new instance of the AntRadioSubscriber.</returns>
        IAntRadioSubscriber CreateAntRadioSubscriber(IAntRadio antRadio);
    }
}
