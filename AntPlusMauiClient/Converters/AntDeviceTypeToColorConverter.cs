using CommunityToolkit.Maui;
using SmallEarthTech.AntPlus;
using SmallEarthTech.AntPlus.DeviceProfiles;
using SmallEarthTech.AntPlus.DeviceProfiles.AssetTracker;
using SmallEarthTech.AntPlus.DeviceProfiles.BicyclePower;
using SmallEarthTech.AntPlus.DeviceProfiles.BikeSpeedAndCadence;
using SmallEarthTech.AntPlus.DeviceProfiles.FitnessEquipment;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AntPlusMauiClient.Converters
{
    public class AntDeviceTypeToColorConverter : IValueConverter
    {
        // create a static dictionary to lookup a color value from an ANT device channel ID device type
        private static readonly Dictionary<byte, Brush> deviceTypeColors = new()
        {
            { BicyclePower.DeviceClass, GetLinearGradientBrush(Colors.RoyalBlue) },                     // Bike Power
            { BikeSpeedSensor.DeviceClass, GetLinearGradientBrush(Colors.SkyBlue) },                    // Bike Speed
            { BikeCadenceSensor.DeviceClass, GetLinearGradientBrush(Colors.LightSkyBlue) },             // Bike Cadence
            { CombinedSpeedAndCadenceSensor.DeviceClass , GetLinearGradientBrush(Colors.SteelBlue) },   // Bike Speed and Cadence
            { HeartRate.DeviceClass, GetLinearGradientBrush(Colors.Red) },                              // Heart Rate
            { FitnessEquipment.DeviceClass, GetLinearGradientBrush(Colors.Silver) },                    // Fitness equipment Sensor
            { MuscleOxygen.DeviceClass, GetLinearGradientBrush(Colors.DarkOrange) },                    // Muscle Oxygen
            { Geocache.DeviceClass, GetLinearGradientBrush(Colors.Green) },                             // Geocache
            { Tracker.DeviceClass, GetLinearGradientBrush(Colors.LightGreen) },                         // Asset Tracker
            { StrideBasedSpeedAndDistance.DeviceClass, GetLinearGradientBrush(Colors.Orange) }          // Stride Based Speed and Distance
        };

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // if the value is an ANT device, lookup the color based on the device type, otherwise return transparent
            if (value is AntDevice device)
            {
                return deviceTypeColors.TryGetValue(device.ChannelId.DeviceType, out Brush? color) ? color : Brush.Transparent;
            }
            return Brush.Transparent;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        // create a linear gradient brush to use as the background for the device type color
        private static LinearGradientBrush GetLinearGradientBrush(Color color)
        {
            Color appThemeColor = Application.Current?.RequestedTheme == AppTheme.Dark ? Colors.Black : Colors.White;
            return new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1),
                GradientStops =
                [
                    new GradientStop { Color = color, Offset = 0 },
                    new GradientStop {Color = appThemeColor, Offset = 0.30F},
                    new GradientStop {Color = appThemeColor, Offset = 0.70F},
                    new GradientStop { Color = color, Offset = 1 }
                ]
            };
        }
    }
}
