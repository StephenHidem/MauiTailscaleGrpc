using AntPlusMauiClient.ViewModels;

namespace AntPlusMauiClient.Views;

public partial class BicyclePowerView : ContentView
{
	public BicyclePowerView(BicyclePowerViewModel viewModel)
	{
        InitializeComponent();
        BindingContext = viewModel;
    }
}