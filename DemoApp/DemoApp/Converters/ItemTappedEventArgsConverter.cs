﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace DemoApp.Converters
{
    public class ItemTappedEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ItemTappedEventArgs eventArgs))
                throw new ArgumentException("Expected TappedEventArgs as value", nameof(value));

            return eventArgs.Item;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
