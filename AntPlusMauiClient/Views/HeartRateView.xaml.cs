using AntPlusMauiClient.ViewModels;

namespace AntPlusMauiClient.Views;

public partial class HeartRateView : ContentView
{
	public HeartRateView(HeartRateViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel;
	}
}