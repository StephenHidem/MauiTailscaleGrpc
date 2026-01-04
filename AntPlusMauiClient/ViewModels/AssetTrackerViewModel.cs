using SmallEarthTech.AntPlus.DeviceProfiles.AssetTracker;

namespace AntPlusMauiClient.ViewModels
{
    public class AssetTrackerViewModel(Tracker tracker)
    {
        public Tracker Tracker { get; } = tracker;
    }
}
