using CommunityToolkit.Mvvm.ComponentModel;
using SmallEarthTech.AntPlus.DeviceProfiles.BikeSpeedAndCadence;

namespace AntPlusMauiClient.ViewModels
{
    public partial class BikeSpeedAndCadenceViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial CombinedSpeedAndCadenceSensor? CombinedSpeedAndCadenceSensor { get; private set; }

        public BikeSpeedAndCadenceViewModel(CombinedSpeedAndCadenceSensor combinedSpeedAndCadenceSensor)
        {
            CombinedSpeedAndCadenceSensor = combinedSpeedAndCadenceSensor;
        }
    }
}
