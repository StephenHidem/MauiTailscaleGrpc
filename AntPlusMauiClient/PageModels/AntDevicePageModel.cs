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
        private readonly Func<Tracker, AssetTrackerView> _assetTrackerFactory;
        private readonly Func<StandardPowerSensor, BicyclePowerView> _bicyclePowerFactory;
        private readonly Func<BikeSpeedSensor, BikeSpeedView> _bikeSpeedFactory;
        private readonly Func<CombinedSpeedAndCadenceSensor, BikeSpeedAndCadenceView> _speedAndCadenceFactory;
        private readonly Func<CrankTorqueFrequencySensor, CTFView> _ctfFactory;

        [ObservableProperty]
        public partial AntDevice? Device { get; set; }

        [ObservableProperty]
        public partial ContentView? AntDeviceView { get; set; }

        public AntDevicePageModel(
            Func<Tracker, AssetTrackerView> assetTrackerFactory,
            Func<StandardPowerSensor, BicyclePowerView> bicyclePowerFactory,
            Func<BikeSpeedSensor, BikeSpeedView> bikeSpeedFactory,
            Func<CombinedSpeedAndCadenceSensor, BikeSpeedAndCadenceView> speedAndCadenceFactory,
            Func<CrankTorqueFrequencySensor, CTFView> ctfFactory)
        {
            _assetTrackerFactory = assetTrackerFactory;
            _bicyclePowerFactory = bicyclePowerFactory;
            _bikeSpeedFactory = bikeSpeedFactory;
            _speedAndCadenceFactory = speedAndCadenceFactory;
            _ctfFactory = ctfFactory;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Device = (AntDevice)query["AntDevice"];

            // switch on device type to create appropriate view
            switch (Device)
            {
                case Tracker trackerDevice:
                    AntDeviceView = _assetTrackerFactory(trackerDevice);
                    break;
                case StandardPowerSensor bikePowerDevice:
                    AntDeviceView = _bicyclePowerFactory(bikePowerDevice);
                    break;
                case CrankTorqueFrequencySensor crankTorqueFrequencySensor:
                    AntDeviceView = _ctfFactory(crankTorqueFrequencySensor);
                    break;
                case BikeSpeedSensor bikeSpeedSensor:
                    AntDeviceView = _bikeSpeedFactory(bikeSpeedSensor);
                    break;
                case CombinedSpeedAndCadenceSensor combinedSensor:
                    AntDeviceView = _speedAndCadenceFactory(combinedSensor);
                    break;
                case BikeCadenceSensor bikeCadenceSensor:
                    AntDeviceView = new BikeCadenceView(new BikeCadenceViewModel(bikeCadenceSensor));
                    break;
                case FitnessEquipment fitnessEquipmentDevice:
                    AntDeviceView = new FitnessEquipmentView(new FitnessEquipmentViewModel(fitnessEquipmentDevice));
                    break;
                case Geocache geocacheDevice:
                    AntDeviceView = new GeocacheView(new GeocacheViewModel(geocacheDevice));
                    break;
                case HeartRate heartRateDevice:
                    AntDeviceView = new HeartRateView(new HeartRateViewModel(heartRateDevice));
                    break;
                case MuscleOxygen muscleOxygenDevice:
                    AntDeviceView = new MuscleOxygenView(new MuscleOxygenViewModel(muscleOxygenDevice));
                    break;
                case StrideBasedSpeedAndDistance speedAndDistanceDevice:
                    AntDeviceView = new SDMView(new SDMViewModel(speedAndDistanceDevice));
                    break;
                case UnknownDevice unknownDevice:
                    AntDeviceView = new UnknownDeviceView(new UnknownDeviceViewModel(unknownDevice));
                    break;
                default:
                    break;
            }
        }
    }
}
