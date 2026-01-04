using AntPlusMauiClient.ViewModels;

namespace AntPlusMauiClient.Views;

public partial class UnknownDeviceView : ContentView
{
	public UnknownDeviceView(UnknownDeviceViewModel viewModel)
	{
		BindingContext = viewModel;
        InitializeComponent();
	}
}