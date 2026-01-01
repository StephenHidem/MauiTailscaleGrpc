using SmallEarthTech.AntRadioInterface;

namespace AntPlusServer.Services
{
    public class AntChannelSubscriberFactory : IAntChannelSubscriberFactory
    {
        public IAntChannelSubscriber CreateAntChannelSubscriber(IAntChannel antChannel)
        {
            return new AntChannelSubscriber(antChannel);
        }
    }
}
