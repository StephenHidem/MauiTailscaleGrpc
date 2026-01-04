using AntPlusMauiClient.PageModels;

namespace AntPlusMauiClient.Pages;

public partial class AntDevicePage : ContentPage
{
	public AntDevicePage(AntDevicePageModel pageModel)
	{
		BindingContext = pageModel;
        InitializeComponent();
	}
}