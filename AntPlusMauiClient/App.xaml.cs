using Microsoft.Extensions.DependencyInjection;

namespace AntPlusMauiClient
{
    public partial class App : Application
    {
        private readonly CancellationTokenSource _cts;

        public App(CancellationTokenSource cancellationTokenSource)
        {
            _cts = cancellationTokenSource;
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = new(new AppShell());
            window.Destroying += (s, e) =>
            {
                _cts.Cancel();
            };
            return window;
        }
    }
}