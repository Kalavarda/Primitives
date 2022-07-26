using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Kalavarda.Primitives.Units.Items;

namespace Kalavarda.Primitives.Units.WPF
{
    public class QualityConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (value is ItemQuality quality)
                switch (quality)
                {
                    case ItemQuality.Junk:
                        return Brushes.Gray;
                    case ItemQuality.Ordinary:
                        return Brushes.Green;
                    case ItemQuality.Good:
                        return Brushes.Blue;
                    case ItemQuality.Rare:
                        return Brushes.Magenta;
                    case ItemQuality.Legendary:
                        return Brushes.Orange;
                    case ItemQuality.Epic:
                        return Brushes.Aqua;
                }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
