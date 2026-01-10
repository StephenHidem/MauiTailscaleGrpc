using AntPlusMauiClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SmallEarthTech.AntPlus.DeviceProfiles.FitnessEquipment;
using static SmallEarthTech.AntPlus.DeviceProfiles.FitnessEquipment.FitnessEquipment;

namespace AntPlusMauiClient.ViewModels
{
    public partial class FitnessEquipmentViewModel : ObservableObject
    {
        private static readonly Dictionary<Type, Type> equipmentViewMap = new()
        {
            { typeof(Treadmill), typeof(TreadmillView) },
            { typeof(Elliptical), typeof(EllipticalView) },
            { typeof(Rower), typeof(RowerView) },
            { typeof(Climber), typeof(ClimberView) },
            { typeof(NordicSkier), typeof(NordicSkierView) },
            { typeof(TrainerStationaryBike), typeof(TrainerStationaryBikeView) }
        };

        [ObservableProperty]
        public partial FitnessEquipment? FitnessEquipment { get; set; }
        [ObservableProperty]
        public partial ContentView? SpecificEquipmentView { get; set; }

        [ObservableProperty]
        public partial double UserWeight { get; set; }

        [ObservableProperty]
        public partial byte WheelDiameterOffset { get; set; }

        [ObservableProperty]
        public partial double BikeWeight { get; set; }

        [ObservableProperty]
        public partial double WheelDiameter { get; set; }

        [ObservableProperty]
        public partial double GearRatio { get; set; }

        [ObservableProperty]
        public partial TimeSpan LapSplitTime { get; set; }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(
            nameof(SetBasicResistanceCommand),
            nameof(SetTargetPowerCommand),
            nameof(SetWindResistanceCommand),
            nameof(SetTrackResistanceCommand))]
        public partial SupportedTrainingModes SupportedTrainingModes { get; set; }
        [ObservableProperty]
        public partial string[]? Capabilities { get; set; }

        // Resistance and power settings properties
        [ObservableProperty]
        public partial double BasicResistancePercent { get; set; }
        [ObservableProperty]
        public partial double TargetPowerWatts { get; set; }
        [ObservableProperty]
        public partial double Grade { get; set; }
        [ObservableProperty]
        public partial double CoefficientOfRollingResistance { get; set; } = 0.004;
        [ObservableProperty]
        public partial double WindResistanceCoefficient { get; set; }
        [ObservableProperty]
        public partial double WindSpeed { get; set; }
        [ObservableProperty]
        public partial double DraftingFactor { get; set; }


        public FitnessEquipmentViewModel(FitnessEquipment fitnessEquipment, IServiceProvider serviceProvider, ILogger<FitnessEquipmentViewModel> logger)
        {
            FitnessEquipment = fitnessEquipment;

            // Initialize the specific equipment view based on the type of fitness equipment
            Type equipmentType = FitnessEquipment.GetType();
            if (equipmentViewMap.TryGetValue(equipmentType, out var viewType))
            {
                SpecificEquipmentView = (ContentView)ActivatorUtilities.CreateInstance(serviceProvider, viewType, FitnessEquipment);
            }
            else
            {
                logger.LogError("Unsupported fitness equipment type: {EquipmentType}", equipmentType.Name);
            }

            FitnessEquipment.LapToggled += FitnessEquipment_LapToggled;
            FitnessEquipment.PropertyChanged += FitnessEquipment_PropertyChanged;
            _ = FitnessEquipment.RequestFECapabilities();
        }

        private void FitnessEquipment_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TrainingModes")
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    SupportedTrainingModes = FitnessEquipment!.TrainingModes;
                    Capabilities = FitnessEquipment.TrainingModes.ToString().Split(',');
                });
            }
        }

        private void FitnessEquipment_LapToggled(object? sender, EventArgs e)
        {
            LapSplitTime = ((FitnessEquipment)sender!).GeneralData.ElapsedTime;
        }

        [RelayCommand]
        private async Task RequestCapabilities() => _ = await FitnessEquipment!.RequestFECapabilities();

        [RelayCommand]
        private async Task SetUserConfig() => _ = await FitnessEquipment!.SetUserConfiguration(UserWeight, WheelDiameterOffset, BikeWeight, WheelDiameter, GearRatio);

        [RelayCommand(CanExecute = nameof(CanSetBasicResistance))]
        private async Task SetBasicResistance() => _ = await FitnessEquipment!.SetBasicResistance(BasicResistancePercent);
        private bool CanSetBasicResistance() => FitnessEquipment != null && FitnessEquipment.TrainingModes.HasFlag(SupportedTrainingModes.BasicResistance);

        [RelayCommand(CanExecute = nameof(CanSetTargetPower))]
        private async Task SetTargetPower() => _ = await FitnessEquipment!.SetTargetPower(TargetPowerWatts);
        private bool CanSetTargetPower() => FitnessEquipment != null && FitnessEquipment.TrainingModes.HasFlag(SupportedTrainingModes.TargetPower);

        [RelayCommand(CanExecute = nameof(CanSetWindResistance))]
        private async Task SetWindResistance() => _ = await FitnessEquipment!.SetWindResistance(WindResistanceCoefficient, (sbyte)Math.Round(WindSpeed), DraftingFactor);
        private bool CanSetWindResistance() => FitnessEquipment != null && FitnessEquipment.TrainingModes.HasFlag(SupportedTrainingModes.Simulation);

        [RelayCommand(CanExecute = nameof(CanSetTrackResistance))]
        private async Task SetTrackResistance() => _ = await FitnessEquipment!.SetTrackResistance(Grade, CoefficientOfRollingResistance);
        private bool CanSetTrackResistance() => FitnessEquipment != null && FitnessEquipment.TrainingModes.HasFlag(SupportedTrainingModes.Simulation);
    }
}
