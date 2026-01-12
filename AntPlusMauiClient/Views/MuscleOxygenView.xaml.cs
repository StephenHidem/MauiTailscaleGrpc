using AntPlusMauiClient.ViewModels;

namespace AntPlusMauiClient.Views;

public partial class MuscleOxygenView : ContentView
{
	public MuscleOxygenView(MuscleOxygenViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        Picker.IsOpen = true;
    }
}