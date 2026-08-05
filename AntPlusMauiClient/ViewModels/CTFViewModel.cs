using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmallEarthTech.AntPlus.DeviceProfiles.BicyclePower;

namespace AntPlusMauiClient.ViewModels
{
    public partial class CTFViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial CrankTorqueFrequencySensor? Sensor { get; private set; }

        [ObservableProperty]
        public partial double? Slope { get; set; }

        [ObservableProperty]
        public partial ushort? SerialNumber { get; set; }

        public CTFViewModel(CrankTorqueFrequencySensor crankTorqueFrequencySensor)
        {
            Sensor = crankTorqueFrequencySensor;
            Sensor.PropertyChanged += Sensor_PropertyChanged;
        }

        private void Sensor_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CalibrationStatus")
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ManualCalRequestCommand.NotifyCanExecuteChanged();
                    SaveSlopeCommand.NotifyCanExecuteChanged();
                    SaveSerialNumberCommand.NotifyCanExecuteChanged();
                });
            }
        }

        [RelayCommand(CanExecute = nameof(CheckCanExecute))]
        private async Task ManualCalRequest()
        {
            _ = await Sensor!.RequestManualCalibration();
        }

        [RelayCommand(CanExecute = nameof(CheckCanExecute))]
        private async Task SaveSlope()
        {
            if (Slope != null)
            {
                _ = await Sensor!.SaveSlopeToFlash(Slope.Value);
            }
        }

        [RelayCommand(CanExecute = nameof(CheckCanExecute))]
        private async Task SaveSerialNumber()
        {
            if (SerialNumber != null)
            {
                _ = await Sensor!.SaveSerialNumberToFlash(SerialNumber.Value);
            }
        }

        private bool CheckCanExecute => Sensor?.CalibrationStatus != CalibrationResponse.InProgress;
    }
}
