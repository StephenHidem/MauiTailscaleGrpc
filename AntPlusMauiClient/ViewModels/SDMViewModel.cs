using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmallEarthTech.AntPlus.DeviceProfiles;

namespace AntPlusMauiClient.ViewModels
{
    public partial class SDMViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial StrideBasedSpeedAndDistance? StrideSpeedDistanceMonitor { get; set; }

        public SDMViewModel(StrideBasedSpeedAndDistance strideBasedSpeedAndDistance)
        {
            StrideSpeedDistanceMonitor = strideBasedSpeedAndDistance;
        }

        [RelayCommand]
        private async Task RequestSummary() => _ = await StrideSpeedDistanceMonitor!.RequestSummaryPage();

        [RelayCommand]
        private async Task RequestCapabilities() => _ = await StrideSpeedDistanceMonitor!.RequestBroadcastCapabilities();
    }
}
