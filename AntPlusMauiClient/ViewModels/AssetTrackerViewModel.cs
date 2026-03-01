using CommunityToolkit.Mvvm.ComponentModel;
using SmallEarthTech.AntPlus.DeviceProfiles.AssetTracker;

namespace AntPlusMauiClient.ViewModels
{
    public partial class AssetTrackerViewModel(Tracker tracker) : ObservableObject
    {
        [ObservableProperty]
        public partial Tracker Tracker { get; private set; } = tracker;
    }
}
