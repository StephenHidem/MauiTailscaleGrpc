using AntPlusMauiClient.ViewModels;

namespace AntPlusMauiClient.Views;

public partial class SDMView : ContentView
{
	public SDMView(SDMViewModel viewModel)
	{
		BindingContext = viewModel;
        InitializeComponent();
	}
}