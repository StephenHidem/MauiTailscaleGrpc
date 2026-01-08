using AntPlusMauiClient.ViewModels;

namespace AntPlusMauiClient.Views;

public partial class SDMView : ContentView
{
	public SDMView(SDMViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel;
	}
}