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
                        return new SolidColorBrush(Colors.Gold); // Yellow for Pending
                    
                    case BookingStatus.Confirmed:
                    case BookingStatus.CheckedIn:
                    case BookingStatus.CheckedOut:
                        return new SolidColorBrush(Colors.LimeGreen); // Green for Confirmed statuses
                    
                    default:
                        return new SolidColorBrush(Colors.Lavender); // Default color
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
