using SmallEarthTech.AntPlus.DeviceProfiles.FitnessEquipment;

namespace AntPlusMauiClient.Views;

public partial class NordicSkierView : ContentView
{
    public NordicSkierView(NordicSkier nordicSkier)
    {
        InitializeComponent();
        BindingContext = nordicSkier;
    }
}