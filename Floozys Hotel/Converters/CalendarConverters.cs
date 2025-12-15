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
                return new Thickness(0);

            if (values[0] is DateTime startDate && values[1] is DateTime viewStartDate &&
                values[2] is double actualWidth && values[3] is int dayCount)
            {
                // If there are no days or width is 0, nothing should be drawn
                if (dayCount == 0 || actualWidth == 0) return new Thickness(0);

                // Calculate the difference in days between booking start and calendar start
                var daysOffset = (startDate - viewStartDate).Days;

                // Adjust for half-day visualization: Start at noon
                double offsetWithHalfDay = daysOffset + 0.5;

                // If booking starts before the visible period, we might still need to draw it if it overlaps into the view
                // But the margin calculation handles positioning relative to the start of the grid.
                // If offset is negative, margin is usually 0 (handled by previous logic or implicit clipping), 
                // but let's keep the simple logic: negative margin isn't usually valid for Grid positioning in this context.
                if (offsetWithHalfDay < 0)
                {
                    // If it starts way before, margin is 0. 
                    // Note: If it starts -0.5 days (noon yesterday), margin is 0 in the current view? 
                    // Actually, if it starts before, the margin should be 0 and we construct the width to bridge the gap.
                    // But if checking strictly StartDate vs ViewDate...
                    // Let's stick to the plan: Shift existing margin calculation.
                    if (daysOffset < 0) return new Thickness(0); 
                }
                
                // Determine left margin based on 0.5 offset if positive
                double effectiveOffset = Math.Max(0, daysOffset + 0.5);

                // Find the width of one day in pixels
                double dayWidth = actualWidth / dayCount;

                // Multiply number of days by width to find the left margin
                double left = (double)(effectiveOffset * dayWidth);
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

                // Calculate duration in days of the VISIBLE portion
                // Total duration = (EndDate - StartDate).Days
                // But we act on visible range.
                
                // Logic:
                // 1. Calculate raw visible duration in days.
                // 2. Subtract 0.5 for the Check-Out day (it ends at noon).
                // 3. Subtract 0.5 for the Check-In day IF the Check-In day is visible (it starts at noon).
                
                double visibleDuration = (booking.EndDate - effectiveStart).Days; // e.g. 17th to 18th = 1 day diff
                
                // Add 1 to include the full day range initially?
                // Example: 17th to 18th. 1 day diff. 
                // We want visual: 17th PM (0.5) + 18th AM (0.5) = 1.0 day width? 
                // Wait, 17th to 18th is 1 night. Visualization spans from 17th (center) to 18th (center).
                // Total width = 1.0 day.
                
                // Example: 17th to 19th (2 nights). 
                // Spans 17th (center) -> 18th (full) -> 19th (center). 
                // 0.5 + 1.0 + 0.5 = 2.0 days width.
                
                // Formula: Dates Difference is exactly the width in days we want!
                // (EndDate - StartDate).Days = Number of nights. 
                // 1 night = 1 day width. 
                
                // BUT we have to handle clipping.
                // If started before view: 
                // ViewStart 17th. Booking 16-19. 
                // Visible part: 17th (full) -> 18th (full) -> 19th (center).
                // Real: 16-19 is 3 nights.
                // Visible from 17th start (00:00) to 19th (12:00) = 2.5 days.
                
                double widthInDays;
                
                if (booking.StartDate < viewStartDate)
                {
                    // Starts before view. We see from Start of View (00:00) to EndDate (12:00)
                    // Difference: (EndDate - ViewStart).Days
                    // Visual: 17th(00:00) to 19th(12:00). 
                    // Diff (19-17) = 2 days. 
                    // Are we seeing 12:00 end? Yes.
                    // Are we seeing 00:00 start? Yes.
                    // So Width = 2.0? 
                    // 17(full) + 18(full) + 19(half)? No, 19th ends at noon. 
                    // 17(full) + 18(full) + 19(0.5) = 2.5? 
                    
                    // Let's assume standard "end at noon".
                    // Diff days (19-17) = 2. 
                    // We want 2.5? No wait. 
                    // 16(noon)-17(noon)-18(noon)-19(noon).
                    // View: 17(00:00) to ...
                    // Visible: 17(AM+PM) + 18(AM+PM) + 19(AM).
                    // 1.0 + 1.0 + 0.5 = 2.5.
                    
                    // Calculation: (EndDate - ViewStartDate).Days + 0.5?
                    // (19-17) = 2 + 0.5 = 2.5. Correct.
                     widthInDays = (booking.EndDate - viewStartDate).Days + 0.5;
                }
                else
                {
                    // Starts inside view.
                    // 17-19.
                    // 17(PM) + 18(full) + 19(AM).
                    // 0.5 + 1.0 + 0.5 = 2.0.
                    // Diff (19-17) = 2. 
                    // Matches exactly Number of Nights.
                    widthInDays = (booking.EndDate - booking.StartDate).Days;
                }
                
                // Safety check
                if (widthInDays < 0) return 0.0;

                // Multiply number of days by day width to get total width
                double dayWidth = actualWidth / dayCount;
                return widthInDays * dayWidth;
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
                // Null check prevents crash if Room object is not assigned to booking
                // Filter returns only bookings that match the RoomId
                return allBookings.Where(b => b.Room != null && b.Room.RoomId == roomId).ToList();
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}