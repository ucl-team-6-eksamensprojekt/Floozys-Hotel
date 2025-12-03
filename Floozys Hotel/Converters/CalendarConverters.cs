using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Floozys_Hotel.Models;

namespace Floozys_Hotel.Converters
{
    // Beregner hvor en booking skal placeres vandret i kalenderen.
    public class BookingLeftMarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Tjekker om alle nødvendige værdier er til stede: Startdato, Visningens start, Bredde og Antal dage.
            // values[0] = StartDate, values[1] = ViewStartDate, values[2] = ActualWidth, values[3] = DayCount
            if (values.Length < 4 || values[0] == null || values[1] == null || values[2] == null || values[3] == null)
                return 0.0;

            if (values[0] is DateTime startDate && values[1] is DateTime viewStartDate && 
                values[2] is double actualWidth && values[3] is int dayCount)
            {
                // Hvis der ingen dage er, eller bredden er 0, tegnes intet.
                if (dayCount == 0 || actualWidth == 0) return 0.0;
                
                // Beregner forskellen i dage mellem bookingens start og kalenderens start.
                var daysOffset = (startDate - viewStartDate).Days;
                
                // Hvis bookingen starter før den viste periode, sættes margin til 0.
                if (daysOffset < 0)
                {
                    return 0.0;
                }
                
                // Finder bredden af én dag i pixels.
                double dayWidth = actualWidth / dayCount;
                // Ganger antal dage med bredden for at finde marginen.
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

    // Bestemmer hvor bred en booking-boks skal være.
    public class BookingWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Sikrer at alle data er med: Booking, Visningens start, Bredde og Antal dage.
            // values[0] = Booking, values[1] = ViewStartDate, values[2] = ActualWidth, values[3] = DayCount
            if (values.Length >= 4 && values[0] is Booking booking && values[1] is DateTime viewStartDate &&
                values[2] is double actualWidth && values[3] is int dayCount)
            {
                if (dayCount == 0 || actualWidth == 0) return 0.0;
                
                // Hvis bookingen starter før visningen, klippes starten af.
                var effectiveStart = booking.StartDate < viewStartDate ? viewStartDate : booking.StartDate;
                
                // Beregner varigheden i dage.
                var days = (booking.EndDate - effectiveStart).Days + 1;
                
                if (days < 0) return 0.0;
                
                // Ganger antal dage med bredden af én dag for at få total bredde.
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
    
    // Viser datoen som et simpelt tal (f.eks. "1" eller "15").
    public class DateHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
             if (value is int day)
             {
                 // Laver tallet om til tekst.
                 return day.ToString();
             }
             return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Filtrerer bookinger så kun dem for det aktuelle værelse vises.
    public class RoomBookingsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Tager imod Værelses-ID og listen over alle bookinger.
            if (values.Length >= 2 && values[0] is int roomId && values[1] is IEnumerable<Booking> allBookings)
            {
                // Returnerer kun bookinger der matcher RoomID.
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
