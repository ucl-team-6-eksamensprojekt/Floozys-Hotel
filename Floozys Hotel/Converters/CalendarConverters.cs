using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Floozys_Hotel.Models;

namespace Floozys_Hotel.Converters
{
    // Beregner horisontal placering af en booking i kalenderen
    public class BookingLeftMarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Verificerer at alle nødvendige værdier er til stede: StartDate, ViewStartDate, ActualWidth, DayCount
            // values[0] = StartDate, values[1] = ViewStartDate, values[2] = ActualWidth, values[3] = DayCount
            if (values.Length < 4 || values[0] == null || values[1] == null || values[2] == null || values[3] == null)
                return 0.0;

            if (values[0] is DateTime startDate && values[1] is DateTime viewStartDate &&
                values[2] is double actualWidth && values[3] is int dayCount)
            {
                // Tegner intet hvis der ingen dage er, eller bredden er 0
                if (dayCount == 0 || actualWidth == 0) return 0.0;

                // Beregner forskellen i dage mellem bookingstart og kalenderstart
                var daysOffset = (startDate - viewStartDate).Days;

                // Sætter margin til 0, hvis bookingen starter før den synlige periode
                if (daysOffset < 0)
                {
                    return 0.0;
                }

                // Finder bredden af én dag i pixels
                double dayWidth = actualWidth / dayCount;

                // Ganger antal dage med bredden for at finde venstre margin
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

    // Bestemmer bredden af en booking-boks
    public class BookingWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Sikrer at alle data er til stede: Booking, ViewStartDate, ActualWidth, DayCount
            // values[0] = Booking, values[1] = ViewStartDate, values[2] = ActualWidth, values[3] = DayCount
            if (values.Length >= 4 && values[0] is Booking booking && values[1] is DateTime viewStartDate &&
                values[2] is double actualWidth && values[3] is int dayCount)
            {
                // Tegner intet hvis der ingen dage er, eller bredden er 0
                if (dayCount == 0 || actualWidth == 0) return 0.0;

                // Klipper startdatoen til visningens startdato, hvis bookingen starter før visningen
                var effectiveStart = booking.StartDate < viewStartDate ? viewStartDate : booking.StartDate;

                // Beregner varighed i dage (tilføjer 1 for at inkludere både start- og slutdag)
                var days = (booking.EndDate - effectiveStart).Days + 1;

                // Negative dage betyder, at bookingen er uden for det synlige område
                if (days < 0) return 0.0;

                // Ganger antal dage med dagsbredden for at få total bredde
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

    // Viser datoen som et simpelt tal (f.eks. "1" eller "15")
    public class DateHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int day)
            {
                // Konverterer heltallet til tekst til visning
                return day.ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Filtrerer bookinger, så kun dem for det aktuelle værelse vises
    public class RoomBookingsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Modtager værelses-ID og liste over alle bookinger til filtrering
            if (values.Length >= 2 && values[0] is int roomId && values[1] is IEnumerable<Booking> allBookings)
            {
                // Filtrerer kun på RoomID, da Room-objektet muligvis ikke er udfyldt
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