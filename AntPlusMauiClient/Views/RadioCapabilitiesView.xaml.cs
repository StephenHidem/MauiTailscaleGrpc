using AntPlusMauiClient.ViewModels;

namespace AntPlusMauiClient.Views;

public partial class RadioCapabilitiesView : ContentView
{
	public RadioCapabilitiesView(RadioCapabilitiesViewModel viewModel)
	{
		BindingContext = viewModel;
        InitializeComponent();
	}
}