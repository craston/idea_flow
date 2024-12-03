namespace frontend.Converters
{
    using System;
    using System.Globalization;
    using Microsoft.Maui.Graphics;

    public class BooleanToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isUserMessage)
            {
                return isUserMessage ? Colors.LightBlue : Colors.LightGray;
            }
            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
