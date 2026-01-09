using SmallEarthTech.AntPlus.DeviceProfiles.BicyclePower;

namespace AntPlusMauiClient.Views;

public partial class BicycleCrankTorqueView : ContentView
{
    public BicycleCrankTorqueView(StandardCrankTorqueSensor sensor)
    {
        InitializeComponent();
        BindingContext = sensor;
    }
}