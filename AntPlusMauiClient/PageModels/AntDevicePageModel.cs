using AntPlusMauiClient.ViewModels;
using AntPlusMauiClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using SmallEarthTech.AntPlus;
using SmallEarthTech.AntPlus.DeviceProfiles;
using SmallEarthTech.AntPlus.DeviceProfiles.AssetTracker;
using SmallEarthTech.AntPlus.DeviceProfiles.BicyclePower;
using SmallEarthTech.AntPlus.DeviceProfiles.BikeSpeedAndCadence;
using SmallEarthTech.AntPlus.DeviceProfiles.FitnessEquipment;
using System;
using System.Collections.Generic;
using System.Text;

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

        [ObservableProperty]
        public partial AntDevice? Device { get; set; }

        [ObservableProperty]
        public partial ContentView? AntDeviceView { get; set; }

        public AntDevicePageModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Device = (AntDevice)query["AntDevice"];

            // Determine the ViewModel and View types based on the device type
            var deviceType = Device.GetType();
            var (viewModelType, contentViewType) = DeviceViewMap.GetValueOrDefault(deviceType, (typeof(UnknownDeviceViewModel), typeof(UnknownDeviceView)));

            var viewModelInstance = ActivatorUtilities.CreateInstance(_serviceProvider, viewModelType, Device);
            AntDeviceView = ActivatorUtilities.CreateInstance(_serviceProvider, contentViewType, viewModelInstance) as ContentView;
        }
    }
}
