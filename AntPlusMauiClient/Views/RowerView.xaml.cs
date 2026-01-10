using SmallEarthTech.AntPlus.DeviceProfiles.FitnessEquipment;

namespace AntPlusMauiClient.Views;

public partial class RowerView : ContentView
{
    public RowerView(Rower rower)
    {
        InitializeComponent();
        BindingContext = rower;
    }
}