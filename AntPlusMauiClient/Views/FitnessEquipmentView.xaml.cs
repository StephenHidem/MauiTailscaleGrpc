using AntPlusMauiClient.ViewModels;

namespace AntPlusMauiClient.Views;

public partial class FitnessEquipmentView : ContentView
{
	public FitnessEquipmentView(FitnessEquipmentViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel;
	}
}