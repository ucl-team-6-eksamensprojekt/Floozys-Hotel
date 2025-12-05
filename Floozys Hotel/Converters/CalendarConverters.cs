using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Floozys_Hotel.Models;

namespace Floozys_Hotel.Converters
{
    // Calculates where a booking should be positioned horizontally in the calendar
    public class BookingLeftMarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Verify all required values are present: StartDate, ViewStartDate, ActualWidth, DayCount
            // values[0] = StartDate, values[1] = ViewStartDate, values[2] = ActualWidth, values[3] = DayCount
            if (values.Length < 4 || values[0] == null || values[1] == null || values[2] == null || values[3] == null)
                return 0.0;

            if (values[0] is DateTime startDate && values[1] is DateTime viewStartDate &&
                values[2] is double actualWidth && values[3] is int dayCount)
            {
                // If there are no days or width is 0, nothing should be drawn
                if (dayCount == 0 || actualWidth == 0) return 0.0;

                // Calculate the difference in days between booking start and calendar start
                var daysOffset = (startDate - viewStartDate).Days;

                // If booking starts before the visible period, set margin to 0
                if (daysOffset < 0)
                {
                    return 0.0;
                }

                // Find the width of one day in pixels
                double dayWidth = actualWidth / dayCount;

                // Multiply number of days by width to find the left margin
                double left = (double)(daysOffset * dayWidth);
                return new Thickness(left, 5, 0, 0);
            }

            return new Thickness(0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Determines how wide a booking box should be
    public class BookingWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Ensure all data is present: Booking, ViewStartDate, ActualWidth, DayCount
            // values[0] = Booking, values[1] = ViewStartDate, values[2] = ActualWidth, values[3] = DayCount
            if (values.Length >= 4 && values[0] is Booking booking && values[1] is DateTime viewStartDate &&
                values[2] is double actualWidth && values[3] is int dayCount)
            {
                // If there are no days or width is 0, nothing should be drawn
                if (dayCount == 0 || actualWidth == 0) return 0.0;

                // If booking starts before the view, clip the start to the view start date
                var effectiveStart = booking.StartDate < viewStartDate ? viewStartDate : booking.StartDate;

                // Calculate duration in days (add 1 to include both start and end day)
                var days = (booking.EndDate - effectiveStart).Days + 1;

                // Negative days means booking is outside visible range
                if (days < 0) return 0.0;

                // Multiply number of days by day width to get total width
                double dayWidth = actualWidth / dayCount;
                return days * dayWidth;
            }
            return 0.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Displays the date as a simple number (e.g. "1" or "15")
    public class DateHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int day)
            {
                // Convert the integer to text for display
                return day.ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Filters bookings so only those for the current room are shown
    public class RoomBookingsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Receives Room ID and list of all bookings to filter
            if (values.Length >= 2 && values[0] is int roomId && values[1] is IEnumerable<Booking> allBookings)
            {
                // Filter only by RoomID since the Room object might not be populated
                return allBookings.Where(b => b.RoomID == roomId).ToList();
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}