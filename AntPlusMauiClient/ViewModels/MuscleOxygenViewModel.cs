using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SmallEarthTech.AntPlus.DeviceProfiles;

namespace AntPlusMauiClient.ViewModels
{
    public partial class MuscleOxygenViewModel : ObservableObject
    {
        private bool started = false;

        [ObservableProperty]
        public partial MuscleOxygen MuscleOxygen { get; private set; }

        private readonly ILogger<MuscleOxygenViewModel> _logger;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LocalTimeOffset))]
        public partial int HoursIndex { get; set; } = 15;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LocalTimeOffset))]
        public partial int MinutesIndex { get; set; } = 0;

        [ObservableProperty]
        public partial bool IsPickerOpen { get; set; } = false;

        public string LocalTimeOffset
        {
            get
            {
                return $"{HoursSource[HoursIndex]:00}:{MinutesSource[MinutesIndex]:00}";
            }
        }

        private TimeSpan SelectedTimeOffset => new(HoursSource[HoursIndex], MinutesSource[MinutesIndex], 0);

        public static int[] HoursSource => [.. Enumerable.Range(-15, 31)];
        public static int[] MinutesSource => [0, 15, 30, 45];

        public MuscleOxygenViewModel(MuscleOxygen muscleOxygen, ILogger<MuscleOxygenViewModel> logger)
        {
            MuscleOxygen = muscleOxygen;
            _logger = logger;
        }

        [RelayCommand]
        private void GetLocalTimeOffset()
        {
            IsPickerOpen = true;
        }

        [RelayCommand]
        private async Task SetTime()
        {
            await SendTimeCommand(MuscleOxygen.CommandId.SetTime);
        }

        [RelayCommand(CanExecute = nameof(CanStartSession))]
        private async Task StartSession()
        {
            started = true;
            CheckCanExecutes();
            await SendTimeCommand(MuscleOxygen.CommandId.StartSession);
        }
        private bool CanStartSession() => !started;

        [RelayCommand(CanExecute = nameof(CanStopSession))]
        private async Task StopSession()
        {
            started = false;
            CheckCanExecutes();
            await SendTimeCommand(MuscleOxygen.CommandId.StopSession);
        }
        private bool CanStopSession() => started;

        [RelayCommand(CanExecute = nameof(CanLogLap))]
        private async Task LogLap()
        {
            await SendTimeCommand(MuscleOxygen.CommandId.Lap);
        }
        private bool CanLogLap() => started;

        private void CheckCanExecutes()
        {
            StartSessionCommand.NotifyCanExecuteChanged();
            StopSessionCommand.NotifyCanExecuteChanged();
            LogLapCommand.NotifyCanExecuteChanged();
        }

        private async Task SendTimeCommand(MuscleOxygen.CommandId commandId)
        {
            try
            {
                await MuscleOxygen.SendCommand(commandId, SelectedTimeOffset, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                // log the exception and inform the user
                _logger.LogError(ex, "Error sending {CommandId} command to MuscleOxygen device.", commandId);
                throw;
            }
        }
    }
}
