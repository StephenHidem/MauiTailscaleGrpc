using AntPlusMauiClient.ViewModels;

namespace AntPlusMauiClient.Views;

public partial class BikeSpeedAndCadenceView : ContentView
{
	public BikeSpeedAndCadenceView(BikeSpeedAndCadenceViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel;
	}
}