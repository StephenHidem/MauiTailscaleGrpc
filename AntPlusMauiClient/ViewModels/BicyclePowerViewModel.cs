using AntPlusMauiClient.PageModels;
using AntPlusMauiClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SmallEarthTech.AntPlus.DeviceProfiles.BicyclePower;
using System.ComponentModel;

namespace AntPlusMauiClient.ViewModels;

public partial class BicyclePowerViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    public partial StandardPowerSensor? Sensor { get; set; }

    [ObservableProperty]
    public partial bool AutoCrankLength { get; set; }

    [ObservableProperty]
    public partial ContentView? TorqueSensorView { get; set; }

    public BicyclePowerViewModel(StandardPowerSensor sensor, IServiceProvider serviceProvider)
    {
        Sensor = sensor;
        _serviceProvider = serviceProvider;

        if (Sensor.TorqueSensor != null)
        {
            TorqueSensorView = (ContentView)ActivatorUtilities.CreateInstance(
                _serviceProvider,
                Sensor.TorqueSensor switch
                {
                    StandardCrankTorqueSensor _ => typeof(BicycleCrankTorqueView),
                    StandardWheelTorqueSensor _ => typeof(BicycleWheelTorqueView),
                    _ => throw new NotSupportedException("Unsupported torque sensor type")
                },
                Sensor.TorqueSensor
            );
        }

        Sensor.PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "AutoZeroSupported")
        {
            SetAutoZeroConfigCommand.NotifyCanExecuteChanged();
        }
        if (sender != null && e.PropertyName == "CalibrationStatus")
        {
        }
    }

    [RelayCommand(CanExecute = nameof(CheckCanExecute))]
    private async Task ManualCalRequest() => _ = await Sensor!.RequestManualCalibration();

    [RelayCommand(CanExecute = nameof(CheckCanExecute))]
    private async Task SetAutoZeroConfig() => _ = await Sensor!.SetAutoZeroConfiguration(Sensor.AutoZeroStatus == AutoZero.Off ? AutoZero.On : AutoZero.Off);

    [RelayCommand(CanExecute = nameof(CheckCanExecute))]
    private async Task GetCustomCalParameters() => _ = await Sensor!.RequestCustomParameters();

    [RelayCommand(CanExecute = nameof(CheckCanExecute))]
    private async Task SetCustomCalParameters(string parameters) => _ = await Sensor!.SetCustomParameters(Convert.FromHexString(parameters));

    [RelayCommand(CanExecute = nameof(CheckCanExecute))]
    private async Task GetParameters(SubPage subpage) => _ = await Sensor!.GetParameters(subpage);

    [RelayCommand(CanExecute = nameof(CheckCanExecute))]
    private async Task SetCrankLength(string length)
    {
        if (AutoCrankLength)
        {
            _ = await Sensor!.SetCrankLength(0xFE);
        }
        else
        {
            _ = await Sensor!.SetCrankLength(double.Parse(length));
        }
    }

    private bool CheckCanExecute => Sensor?.CalibrationStatus != CalibrationResponse.InProgress;
}
