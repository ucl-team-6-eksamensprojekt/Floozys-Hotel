using System;
using System.Globalization;
using System.Windows.Data;

namespace Floozys_Hotel.Converters
{
    public class CheckInStatusConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 2)
            {
                var checkIn = values[0] as DateTime?;
                var checkOut = values[1] as DateTime?;

                if (checkOut.HasValue)
                {
                    return $"Checked out: {checkOut.Value:dd/MM HH:mm}";
                }
                else if (checkIn.HasValue)
                {
                    return $"Checked in: {checkIn.Value:dd/MM HH:mm}";
                }
                else
                {
                    return "Not checked in";
                }
            }
            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
