using SmallEarthTech.AntPlus.DeviceProfiles.FitnessEquipment;

namespace AntPlusMauiClient.Views;

public partial class ClimberView : ContentView
{
    public ClimberView(Climber climber)
    {
        InitializeComponent();
        BindingContext = climber;
    }
}