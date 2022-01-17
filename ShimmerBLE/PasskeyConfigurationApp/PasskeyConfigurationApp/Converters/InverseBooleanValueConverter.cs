using System;
using System.Globalization;
using MvvmCross.Converters;
using Xamarin.Forms;

namespace PassKeyConfigurationApp.Converters
{
    public class InverseBooleanValueConverter : MvxValueConverter<bool, bool>, IValueConverter
    {
        protected override bool Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return !value;
        }

        protected override bool ConvertBack(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return !value;
        }
    }
}