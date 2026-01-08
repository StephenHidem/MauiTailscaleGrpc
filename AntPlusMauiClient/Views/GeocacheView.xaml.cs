using AntPlusMauiClient.ViewModels;

namespace AntPlusMauiClient.Views;

public partial class GeocacheView : ContentView
{
	public GeocacheView(GeocacheViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel;
	}
}