using CommunityToolkit.Mvvm.ComponentModel;
using SmallEarthTech.AntPlus.DeviceProfiles.BikeSpeedAndCadence;

namespace AntPlusMauiClient.ViewModels
{
    public partial class BikeCadenceViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial BikeCadenceSensor? BikeCadenceSensor { get; set; }

        public BikeCadenceViewModel(BikeCadenceSensor bikeCadenceSensor)
        {
            BikeCadenceSensor = bikeCadenceSensor;
        }
    }
}
