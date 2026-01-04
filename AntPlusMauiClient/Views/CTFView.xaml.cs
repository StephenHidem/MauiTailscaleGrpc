using AntPlusMauiClient.ViewModels;

namespace AntPlusMauiClient.Views;

public partial class CTFView : ContentView
{
	public CTFView(CTFViewModel viewModel)
	{
		BindingContext = viewModel;
        InitializeComponent();
	}
}