using System;
using System.Globalization;
using Xamarin.Forms;

namespace Microsoft.MobCAT.Forms.Converters
{
    public class ListViewSelectedItemValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SelectedItemChangedEventArgs args = value as SelectedItemChangedEventArgs;

            if (args == null)
                return value;

            return args.SelectedItem;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}