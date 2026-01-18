using AntPlusMauiClient.GrpcServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using SmallEarthTech.AntPlus;
using SmallEarthTech.AntPlus.Extensions.Hosting;
using SmallEarthTech.AntRadioInterface;

namespace AntPlusMauiClient.PageModels
{
    public partial class MainPageModel : ObservableObject
    {
        private readonly AntCollection _antCollection;
        private readonly AntRadioService _antRadioService;
        private readonly ILogger<MainPageModel> _logger;
        private readonly CancellationToken _cancellationToken;

        public string ServerUrl => $"https://{AntRadioService.TailnetFqdn}";

        [ObservableProperty]
        public partial bool IsBusy { get; set; }

        [ObservableProperty]
        public partial object? SelectedAntDevice { get; set; }

        [ObservableProperty]
        public partial string? ProductDescription { get; set; }

        [ObservableProperty]
        public partial uint? SerialNumber { get; set; }

        [ObservableProperty]
        public partial string? HostVersion { get; set; }

        [ObservableProperty]
        public partial DeviceCapabilities? Capabilities { get; set; }

        [ObservableProperty]
        public partial List<string> Flags { get; set; } = [];

        [ObservableProperty]
        public partial AntCollection? AntDevices { get; set; }

        public MainPageModel(IAntRadio antRadioService, AntCollection antDevices, ILogger<MainPageModel> logger, CancellationTokenSource cancellationTokenSource)
        {
            _antCollection = antDevices;
            _antRadioService = (AntRadioService)antRadioService;
            _logger = logger;
            _cancellationToken = cancellationTokenSource.Token;
            _ = Task.Run(InitializeAsync);
        }

        private async Task InitializeAsync()
        {
            bool success = false;
            MainThread.BeginInvokeOnMainThread(() => IsBusy = true);
            while (!success && !_cancellationToken.IsCancellationRequested)
            {
                success = await _antRadioService.FindAntRadioServerAsync();
                if (!success)
                {
                    _logger.LogWarning("Unable to find ANT radio server. Retrying in 5 seconds...");
                    await Task.Delay(5000, _cancellationToken);
                }
            }

            // get properties from server
            ProductDescription = _antRadioService.ProductDescription;
            SerialNumber = _antRadioService.SerialNumber;
            HostVersion = _antRadioService.Version;
            AntDevices = _antCollection;

            // get capabilities from server
            Capabilities = await _antRadioService.GetDeviceCapabilities();
            Flags = Capabilities.GetType().GetProperties().
                Where(t => t.PropertyType == typeof(bool) && (bool)t.GetValue(Capabilities)!).
                ToList().
                ConvertAll<string>(e => e.Name);
            Flags.Sort();

            // handle radio response updates
            _antRadioService.RadioResponse += HandleRadioResponse;
            _antRadioService.RpcExceptionReceived += HandleRpcException;

            MainThread.BeginInvokeOnMainThread(() => IsBusy = false);

            await AntDevices.StartScanning();
        }

        private void HandleRadioResponse(object? sender, AntResponse e)
        {
            // handle radio response
            if (e.ResponseId == MessageId.StartupMessage)
            {
                _ = AntDevices!.StartScanning();
            }
        }

        private void HandleRpcException(object? sender, RpcException e)
        {
            if (e.StatusCode == StatusCode.Unavailable)
            {
                // server is not available
                _logger.LogError("Server is not available.");

                // remove the radio response handler
                _antRadioService.RadioResponse -= HandleRadioResponse;
                _antRadioService.RpcExceptionReceived -= HandleRpcException;

                // go to the home page and search for the server again
                MainThread.BeginInvokeOnMainThread(() => Shell.Current.GoToAsync("MainPage"));
            }
            else
            {
                // other errors
                _logger.LogError("RPC Exception: {Message}", e.Message);
            }
        }

        // RelayCommand to handle ANT device selection
        [RelayCommand]
        private void AntDeviceSelected(AntDevice selectedDevice)
        {
            if (selectedDevice != null)
            {
                // Navigate to the device details page with the selected device as a parameter
                var navigationParams = new Dictionary<string, object>
                {
                    { "AntDevice", selectedDevice }
                };
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync("AntDevicePage", navigationParams);
                    SelectedAntDevice = null; // Reset selection
                });
            }
        }
    }
}
