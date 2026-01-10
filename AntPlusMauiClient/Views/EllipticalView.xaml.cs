using SmallEarthTech.AntPlus.DeviceProfiles.FitnessEquipment;

namespace AntPlusMauiClient.Views;

public partial class EllipticalView : ContentView
{
    public EllipticalView(Elliptical elliptical)
    {
        InitializeComponent();
        BindingContext = elliptical;
    }
}