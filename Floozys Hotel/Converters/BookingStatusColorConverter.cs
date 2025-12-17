using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Floozys_Hotel.Models;

namespace Floozys_Hotel.Converters
{
    public class BookingStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BookingStatus status)
            {
                switch (status)
                {
                    case BookingStatus.Pending:
                        return new SolidColorBrush(Colors.Gold); // YELLOW for Pending

                    case BookingStatus.Confirmed:
                        return new SolidColorBrush(Colors.CornflowerBlue); // BLUE for Confirmed

                    case BookingStatus.CheckedIn:
                        return new SolidColorBrush(Colors.LimeGreen); // GREEN for CheckedIn

                    case BookingStatus.CheckedOut:
                        return new SolidColorBrush(Colors.LightGray); // GRAY for CheckedOut

                    case BookingStatus.Cancelled:
                        return new SolidColorBrush(Colors.IndianRed); // RED for Cancelled

                    default:
                        return new SolidColorBrush(Colors.Lavender); // Default
                }
            }
            return new SolidColorBrush(Colors.Lavender);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}