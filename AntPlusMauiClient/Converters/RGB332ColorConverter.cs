using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AntPlusMauiClient.Converters
{
    public class RGB332ColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not byte rgb332)
                return Colors.Transparent; // Fallback if value is invalid

            // Extract bits
            int redBits = (rgb332 >> 5) & 0b00000111;
            int greenBits = (rgb332 >> 2) & 0b00000111;
            int blueBits = rgb332 & 0b00000011;

            // Scale to 0–255
            byte red = (byte)((redBits * 255) / 7);
            byte green = (byte)((greenBits * 255) / 7);
            byte blue = (byte)((blueBits * 255) / 3);

            return Color.FromRgb(red, green, blue);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
