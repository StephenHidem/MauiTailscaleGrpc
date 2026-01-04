using AntPlusMauiClient.PageModels;

namespace AntPlusMauiClient.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel mainPageModel)
        {
            BindingContext = mainPageModel;
            InitializeComponent();
        }
    }
}
