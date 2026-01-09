using SmallEarthTech.AntPlus.DeviceProfiles.BicyclePower;

namespace AntPlusMauiClient.Views;

public partial class BicycleWheelTorqueView : ContentView
{
    public BicycleWheelTorqueView(StandardWheelTorqueSensor sensor)
    {
        InitializeComponent();
        BindingContext = sensor;
    }
}
