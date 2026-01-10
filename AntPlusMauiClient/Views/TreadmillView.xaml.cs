using SmallEarthTech.AntPlus.DeviceProfiles.FitnessEquipment;

namespace AntPlusMauiClient.Views;

public partial class TreadmillView : ContentView
{
    public TreadmillView(Treadmill treadmill)
    {
        InitializeComponent();
        BindingContext = treadmill;
    }
}