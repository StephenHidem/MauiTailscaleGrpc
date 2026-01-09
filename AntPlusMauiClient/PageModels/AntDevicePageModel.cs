using AntPlusMauiClient.ViewModels;
using AntPlusMauiClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using SmallEarthTech.AntPlus;
using SmallEarthTech.AntPlus.DeviceProfiles;
using SmallEarthTech.AntPlus.DeviceProfiles.AssetTracker;
using SmallEarthTech.AntPlus.DeviceProfiles.BicyclePower;
using SmallEarthTech.AntPlus.DeviceProfiles.BikeSpeedAndCadence;
using SmallEarthTech.AntPlus.DeviceProfiles.FitnessEquipment;

namespace AntPlusMauiClient.PageModels
{
    public partial class AntDevicePageModel : ObservableObject, IQueryAttributable
    {
        // Map of device types to their corresponding ViewModel and View types
        private static readonly Dictionary<Type, (Type, Type)> DeviceViewMap = new()
        {
            { typeof(Tracker), (typeof(AssetTrackerViewModel), typeof(AssetTrackerView)) },
            { typeof(StandardPowerSensor), (typeof(BicyclePowerViewModel), typeof(BicyclePowerView)) },
            { typeof(CrankTorqueFrequencySensor), (typeof(CTFViewModel), typeof(CTFView)) },
            { typeof(BikeSpeedSensor), (typeof(BikeSpeedViewModel), typeof(BikeSpeedView)) },
            { typeof(CombinedSpeedAndCadenceSensor), (typeof(BikeSpeedAndCadenceViewModel), typeof(BikeSpeedAndCadenceView)) },
            { typeof(BikeCadenceSensor), (typeof(BikeCadenceViewModel), typeof(BikeCadenceView)) },
            { typeof(FitnessEquipment), (typeof(FitnessEquipmentViewModel), typeof(FitnessEquipmentView)) },
            { typeof(Geocache), (typeof(GeocacheViewModel), typeof(GeocacheView)) },
            { typeof(HeartRate), (typeof(HeartRateViewModel), typeof(HeartRateView)) },
            { typeof(MuscleOxygen), (typeof(MuscleOxygenViewModel), typeof(MuscleOxygenView)) },
            { typeof(StrideBasedSpeedAndDistance), (typeof(SDMViewModel), typeof(SDMView)) }
        };
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AntDevicePageModel> _logger;

        [ObservableProperty]
        public partial AntDevice? Device { get; set; }

        [ObservableProperty]
        public partial ContentView? AntDeviceView { get; set; }

        public AntDevicePageModel(IServiceProvider serviceProvider, ILogger<AntDevicePageModel> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (!query.TryGetValue("AntDevice", out var deviceObj) || deviceObj is not AntDevice device)
            {
                // It's good practice to handle cases where expected query parameters are missing or of the wrong type.
                // Consider logging this and/or navigating back.
                _logger.LogWarning("AntDevice query parameter is missing or invalid.");
                return;
            }
            Device = device;

            // Determine the ViewModel and View types based on the device type
            var deviceType = Device.GetType();
            var (viewModelType, contentViewType) = DeviceViewMap.GetValueOrDefault(deviceType, (typeof(UnknownDeviceViewModel), typeof(UnknownDeviceView)));

            try
            {
                var viewModelInstance = ActivatorUtilities.CreateInstance(_serviceProvider, viewModelType, Device);
                AntDeviceView = (ContentView)ActivatorUtilities.CreateInstance(_serviceProvider, contentViewType, viewModelInstance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating view for device type {DeviceType}", deviceType.Name);
                var fallbackViewModel = ActivatorUtilities.CreateInstance(_serviceProvider, typeof(UnknownDeviceViewModel), Device);
                AntDeviceView = (ContentView)ActivatorUtilities.CreateInstance(_serviceProvider, typeof(UnknownDeviceView), fallbackViewModel);
            }
        }
    }
}
