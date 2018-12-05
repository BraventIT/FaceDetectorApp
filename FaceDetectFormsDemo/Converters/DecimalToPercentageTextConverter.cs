using System;
using System.Globalization;
using Xamarin.Forms;

namespace FaceDetectFormsDemo.Converters
{
    public class DecimalToPercentageTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            string percentageDecimal = value as string;
            double.TryParse(percentageDecimal, out double percentage);
            return $"{percentage * 100}%";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
