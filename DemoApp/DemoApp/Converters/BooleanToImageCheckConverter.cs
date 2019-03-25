using System;
using System.Globalization;
using Xamarin.Forms;

namespace DemoApp.Converters
{
    public class BooleanToImageCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value)
                return "ic_checkbox_marked_circle_outline_grey600_48dp";
            return "ic_checkbox_blank_circle_outline_grey600_48dp";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
