using CommunityToolkit.Mvvm.ComponentModel;
using SmallEarthTech.AntPlus.DeviceProfiles.AssetTracker;

namespace AntPlusMauiClient.ViewModels
{
    public partial class AssetTrackerViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial Tracker Tracker { get; set; }

        public AssetTrackerViewModel(Tracker tracker)
        {
            Tracker = tracker;
        }
    }
}
