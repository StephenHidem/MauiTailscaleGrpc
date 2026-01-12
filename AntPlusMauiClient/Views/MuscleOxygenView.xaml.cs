using AntPlusMauiClient.ViewModels;

namespace AntPlusMauiClient.Views;

public partial class MuscleOxygenView : ContentView
{
	public MuscleOxygenView(MuscleOxygenViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel;
	}
}