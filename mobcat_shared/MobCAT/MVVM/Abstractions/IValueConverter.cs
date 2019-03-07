using System;
using System.Globalization;

namespace Microsoft.MobCAT.MVVM.Abstractions
{
    public interface IValueConverter
    {
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <returns>The converted value.</returns>
        /// <param name="value">The original value to be converted.</param>
        /// <param name="targetType">The target type that the original value is to be converted to.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="culture">Culture.</param>
        object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Modifies the target data before passing it to the source object. This method is called only in TwoWay bindings.
        /// </summary>
        /// <returns>The value converted back from the target type.</returns>
        /// <param name="value">The value to convert back to the original type.</param>
        /// <param name="targetType">The target type that the value is to be converted back to.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="culture">Culture.</param>
        object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}