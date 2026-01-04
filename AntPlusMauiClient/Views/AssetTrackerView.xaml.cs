using AntPlusMauiClient.ViewModels;

namespace AntPlusMauiClient.Views;

public partial class AssetTrackerView : ContentView
{
	public AssetTrackerView(AssetTrackerViewModel viewModel)
	{
		BindingContext = viewModel;
        InitializeComponent();
	}
}