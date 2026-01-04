using CommunityToolkit.Mvvm.ComponentModel;
using SmallEarthTech.AntPlus.DeviceProfiles.BikeSpeedAndCadence;

namespace AntPlusMauiClient.ViewModels
{
    public partial class BikeSpeedViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial BikeSpeedSensor? BikeSpeedSensor { get; set; }

        public BikeSpeedViewModel(BikeSpeedSensor bikeSpeedSensor)
        {
            BikeSpeedSensor = bikeSpeedSensor;
        }
    }
}
