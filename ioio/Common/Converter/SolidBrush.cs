﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ioio.Common.Converter
{

    public class SolidBrush : IValueConverter
    {
        // convert SolidBrush to/from Color
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((SolidColorBrush)value).Color;
        }
    }
}
