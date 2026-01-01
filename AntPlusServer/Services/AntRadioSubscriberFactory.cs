using SmallEarthTech.AntRadioInterface;

namespace AntPlusServer.Services
{
    public class AntRadioSubscriberFactory : IAntRadioSubscriberFactory
    {
        public IAntRadioSubscriber CreateAntRadioSubscriber(IAntRadio antRadio)
        {
            return new AntRadioSubscriber(antRadio);
        }
    }
}
