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

        public string LocalTimeOffset
        {
            get
            {
                return $"{HoursSource[HoursIndex]:00}:{MinutesSource[MinutesIndex]:00}";
            }
        }

        public static int[] HoursSource => [.. Enumerable.Range(-15, 31)];
        public static int[] MinutesSource => [0, 15, 30, 45];

        public MuscleOxygenViewModel(MuscleOxygen muscleOxygen)
        {
            MuscleOxygen = muscleOxygen;
        }

        [RelayCommand]
        private async Task SetTime()
        {
            TimeSpan ts = new(HoursSource[HoursIndex], MinutesSource[MinutesIndex], 0);
            _ = await MuscleOxygen!.SendCommand(MuscleOxygen.CommandId.SetTime, ts, DateTime.UtcNow);
        }

        [RelayCommand(CanExecute = nameof(CanStartSession))]
        private async Task StartSession()
        {
            started = true;
            CheckCanExecutes();
            TimeSpan ts = new(HoursSource[HoursIndex], MinutesSource[MinutesIndex], 0);
            _ = await MuscleOxygen!.SendCommand(MuscleOxygen.CommandId.StartSession, ts, DateTime.UtcNow);
        }
        private bool CanStartSession() => !started;

        [RelayCommand(CanExecute = nameof(CanStopSession))]
        private async Task StopSession()
        {
            started = false;
            CheckCanExecutes();
            TimeSpan ts = new(HoursSource[HoursIndex], MinutesSource[MinutesIndex], 0);
            _ = await MuscleOxygen!.SendCommand(MuscleOxygen.CommandId.StopSession, ts, DateTime.UtcNow);
        }
        private bool CanStopSession() => started;

        [RelayCommand(CanExecute = nameof(CanLogLap))]
        private async Task LogLap()
        {
            TimeSpan ts = new(HoursSource[HoursIndex], MinutesSource[MinutesIndex], 0);
            _ = await MuscleOxygen!.SendCommand(MuscleOxygen.CommandId.Lap, ts, DateTime.UtcNow);
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
