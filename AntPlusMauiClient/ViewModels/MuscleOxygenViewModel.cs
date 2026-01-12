using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmallEarthTech.AntPlus.DeviceProfiles;

namespace AntPlusMauiClient.ViewModels
{
    public partial class MuscleOxygenViewModel : ObservableObject
    {
        private bool started = false;

        [ObservableProperty]
        public partial MuscleOxygen? MuscleOxygen { get; private set; }

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

        public MuscleOxygenViewModel(MuscleOxygen muscleOxygen)
        {
            MuscleOxygen = muscleOxygen;
        }

        [RelayCommand]
        private void GetLocalTimeOffset()
        {
            IsPickerOpen = true;
        }

        [RelayCommand]
        private async Task SetTime()
        {
            _ = await MuscleOxygen!.SendCommand(MuscleOxygen.CommandId.SetTime, SelectedTimeOffset, DateTime.UtcNow);
        }

        [RelayCommand(CanExecute = nameof(CanStartSession))]
        private async Task StartSession()
        {
            started = true;
            CheckCanExecutes();
            _ = await MuscleOxygen!.SendCommand(MuscleOxygen.CommandId.StartSession, SelectedTimeOffset, DateTime.UtcNow);
        }
        private bool CanStartSession() => !started;

        [RelayCommand(CanExecute = nameof(CanStopSession))]
        private async Task StopSession()
        {
            started = false;
            CheckCanExecutes();
            _ = await MuscleOxygen!.SendCommand(MuscleOxygen.CommandId.StopSession, SelectedTimeOffset, DateTime.UtcNow);
        }
        private bool CanStopSession() => started;

        [RelayCommand(CanExecute = nameof(CanLogLap))]
        private async Task LogLap()
        {
            _ = await MuscleOxygen!.SendCommand(MuscleOxygen.CommandId.Lap, SelectedTimeOffset, DateTime.UtcNow);
        }
        private bool CanLogLap() => started;

        private void CheckCanExecutes()
        {
            StartSessionCommand.NotifyCanExecuteChanged();
            StopSessionCommand.NotifyCanExecuteChanged();
            LogLapCommand.NotifyCanExecuteChanged();
        }
    }
}
