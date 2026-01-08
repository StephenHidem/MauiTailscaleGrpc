using AntPlusMauiClient.ViewModels;

namespace AntPlusMauiClient.Views;

public partial class BikeSpeedView : ContentView
{
	public BikeSpeedView(BikeSpeedViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel;
	}
}