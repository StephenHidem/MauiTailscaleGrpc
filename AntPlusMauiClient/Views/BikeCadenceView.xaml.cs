using AntPlusMauiClient.ViewModels;

namespace AntPlusMauiClient.Views;

public partial class BikeCadenceView : ContentView
{
	public BikeCadenceView(BikeCadenceViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel;
	}
}