using AntPlusMauiClient.ViewModels;

namespace AntPlusMauiClient.Views;

public partial class BicyclePowerView : ContentView
{
	public BicyclePowerView(BicyclePowerViewModel viewModel)
	{
		BindingContext = viewModel;
        InitializeComponent();
	}
}