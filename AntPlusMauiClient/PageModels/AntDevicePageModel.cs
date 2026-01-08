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
            Type viewModel;
            Type contentView;

            // switch on device type to create appropriate view
            switch (Device)
            {
                case Tracker:
                    viewModel = typeof(AssetTrackerViewModel);
                    contentView = typeof(AssetTrackerView);
                    break;
                case StandardPowerSensor:
                    viewModel = typeof(BicyclePowerViewModel);
                    contentView = typeof(BicyclePowerView);
                    break;
                case CrankTorqueFrequencySensor:
                    viewModel = typeof(CTFViewModel);
                    contentView = typeof(CTFView);
                    break;
                case BikeSpeedSensor:
                    viewModel = typeof(BikeSpeedViewModel);
                    contentView = typeof(BikeSpeedView);
                    break;
                case CombinedSpeedAndCadenceSensor:
                    viewModel = typeof(BikeSpeedAndCadenceViewModel);
                    contentView = typeof(BikeSpeedAndCadenceView);
                    break;
                case BikeCadenceSensor:
                    viewModel = typeof(BikeCadenceViewModel);
                    contentView = typeof(BikeCadenceView);
                    break;
                case FitnessEquipment:
                    viewModel = typeof(FitnessEquipmentViewModel);
                    contentView = typeof(FitnessEquipmentView);
                    break;
                case Geocache:
                    viewModel = typeof(GeocacheViewModel);
                    contentView = typeof(GeocacheView);
                    break;
                case HeartRate:
                    viewModel = typeof(HeartRateViewModel);
                    contentView = typeof(HeartRateView);
                    break;
                case MuscleOxygen:
                    viewModel = typeof(MuscleOxygenViewModel);
                    contentView = typeof(MuscleOxygenView);
                    break;
                case StrideBasedSpeedAndDistance:
                    viewModel = typeof(SDMViewModel);
                    contentView = typeof(SDMView);
                    break;
                default:
                    viewModel = typeof(UnknownDeviceViewModel);
                    contentView = typeof(UnknownDeviceView);
                    break;
            }

            var viewModelInstance = ActivatorUtilities.CreateInstance(_serviceProvider, viewModel, Device);
            AntDeviceView = ActivatorUtilities.CreateInstance(_serviceProvider, contentView, viewModelInstance) as ContentView;
        }
    }
}
