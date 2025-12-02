using System;
using Floozys_Hotel.Core;

namespace Floozys_Hotel.Models
{
    /// <summary>
    /// Represents a hotel room booking for a specific guest and time period.
    /// Contains business rules for valid booking dates and times.
    /// Follows Larman's Information Expert (GRASP) - validates its own data.
    /// Follows DDD - business logic in domain model.
    /// UC01: Register Booking - StartDate, EndDate (CheckInTime/CheckOutTime optional)
    /// UC03: Update Booking - Can modify all fields including CheckInTime, CheckOutTime
    /// </summary>
    public class Booking : ObservableObject
    {
        // Unique identifier for the booking (assigned by repository after database insert)
        private int _bookingID;
        public int BookingID
        {
            get => _bookingID;
            set
            {
                _bookingID = value;
                OnPropertyChanged();
            }
        }

        // Start date of booking (first day of stay) - UC01, UC03
        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (value == default)
                {
                    throw new ArgumentException("Start date is required");
                }
                _startDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Duration));
                OnPropertyChanged(nameof(NumberOfNights));
            }
        }

        // End date of booking (last day of stay) - Must be after start date - UC01, UC03
        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (value == default)
                {
                    throw new ArgumentException("End date is required");
                }
                if (value <= StartDate)
                {
                    throw new ArgumentException("End date must be after start date");
                }
                _endDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Duration));
                OnPropertyChanged(nameof(NumberOfNights));
            }
        }

        // Actual check-in timestamp 
        private DateTime? _checkInTime;
        public DateTime? CheckInTime
        {
            get => _checkInTime;
            set
            {
                _checkInTime = value;
                OnPropertyChanged();
            }
        }

        // Actual check-out timestamp 
        private DateTime? _checkOutTime;
        public DateTime? CheckOutTime
        {
            get => _checkOutTime;
            set
            {
                // Validate only if both are set
                if (value.HasValue && CheckInTime.HasValue && value.Value <= CheckInTime.Value)
                {
                    throw new ArgumentException("Check-out time must be after check-in time");
                }
                _checkOutTime = value;
                OnPropertyChanged();
            }
        }

        // Reference to the booked room - UC01, UC03
        private int _roomID;
        public int RoomID
        {
            get => _roomID;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Room is required");
                }
                _roomID = value;
                OnPropertyChanged();
            }
        }

        // Reference to the guest making the booking - UC01, UC03
        private int _guestID;
        public int GuestID
        {
            get => _guestID;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Guest is required");
                }
                _guestID = value;
                OnPropertyChanged();
            }
        }

        // Calculated property - duration of stay (based on dates)
        public TimeSpan Duration
        {
            get
            {
                if (StartDate == default || EndDate == default)
                    return TimeSpan.Zero;

                return EndDate - StartDate;
            }
        }

        // Calculated property - number of nights stayed
        public int NumberOfNights
        {
            get
            {
                if (StartDate == default || EndDate == default)
                    return 0;

                return (EndDate - StartDate).Days;
            }
        }
    }
}