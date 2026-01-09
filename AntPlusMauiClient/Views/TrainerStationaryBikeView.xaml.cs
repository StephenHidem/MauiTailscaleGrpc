using SmallEarthTech.AntPlus.DeviceProfiles.FitnessEquipment;

namespace AntPlusMauiClient.Views;

public partial class TrainerStationaryBikeView : ContentView
{
    public TrainerStationaryBikeView(TrainerStationaryBike trainerStationaryBike)
    {
        InitializeComponent();
        BindingContext = trainerStationaryBike;
    }
}