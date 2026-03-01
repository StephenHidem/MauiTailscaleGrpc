using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmallEarthTech.AntPlus;
using SmallEarthTech.AntPlus.DeviceProfiles;
using SmallEarthTech.AntRadioInterface;
using System.Collections.ObjectModel;

namespace AntPlusMauiClient.ViewModels
{
    public partial class HeartRateViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial HeartRate? HeartRate { get; private set; }

        [ObservableProperty]
        public partial SportMode ModeRequested { get; set; }

        [ObservableProperty]
        public partial bool ApplyFeature { get; set; }

        [ObservableProperty]
        public partial bool EnableGymMode { get; set; }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor("SetGymModeCommand")]
        public partial bool IsGymModeSupported { get; set; }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor("SetSportModeCommand")]
        public partial bool IsRunningSupported { get; set; }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor("SetSportModeCommand")]
        public partial bool IsCyclingSupported { get; set; }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor("SetSportModeCommand")]
        public partial bool IsSwimmingSupported { get; set; }

        // Observable collection of heart rate data
        public ObservableCollection<HeartRate.CommonHeartRateData> HeartRateDataSeries { get; } = [];
        private readonly int MaxDataPoints = 400;

        public HeartRateViewModel(HeartRate heartRate)
        {
            HeartRate = heartRate;
            ModeRequested = SportMode.Generic;
            ApplyFeature = true;
            HeartRate.PropertyChanged += HeartRate_PropertyChanged;
            _ = HeartRate.RequestDataPage(HeartRate.DataPage.Capabilities);
        }

        private void HeartRate_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Capabilities")
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    IsCyclingSupported = HeartRate!.Capabilities.Supported.HasFlag(HeartRate.Features.Cycling);
                    IsRunningSupported = HeartRate.Capabilities.Supported.HasFlag(HeartRate.Features.Running);
                    IsSwimmingSupported = HeartRate.Capabilities.Supported.HasFlag(HeartRate.Features.Swimming);
                    IsGymModeSupported = HeartRate.Capabilities.Supported.HasFlag(HeartRate.Features.GymMode);
                    SetGymModeCommand.NotifyCanExecuteChanged();
                    SetSportModeCommand.NotifyCanExecuteChanged();
                });
            }
            else if (e.PropertyName == "HeartRateData")
            {
                if (HeartRateDataSeries.Count > MaxDataPoints)
                {
                    HeartRateDataSeries.RemoveAt(0);
                }
                HeartRateDataSeries.Add(HeartRate!.HeartRateData);
            }
        }

        [RelayCommand]
        private void GetCapabilities() => _ = HeartRate!.RequestDataPage(HeartRate.DataPage.Capabilities);

        [RelayCommand(CanExecute = nameof(CanSetGymMode))]
        private async Task<MessagingReturnCode> SetGymMode()
        {
            MessagingReturnCode result = await HeartRate!.SetHRFeature(ApplyFeature, EnableGymMode);
            _ = HeartRate.RequestDataPage(HeartRate.DataPage.Capabilities);
            return result;
        }
        private bool CanSetGymMode() => IsGymModeSupported;

        [RelayCommand(CanExecute = nameof(CanSetSportMode))]
        private async Task<MessagingReturnCode> SetSportMode()
        {
            MessagingReturnCode result = await HeartRate!.SetSportMode(ModeRequested);
            _ = HeartRate.RequestDataPage(HeartRate.DataPage.Capabilities);
            return result;
        }
        private bool CanSetSportMode() => IsRunningSupported || IsCyclingSupported || IsSwimmingSupported;
    }
}
