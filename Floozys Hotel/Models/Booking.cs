using Floozys_Hotel.Core;
using Floozys_Hotel.Models;
using System;
using System.ComponentModel.DataAnnotations;
using Floozys_Hotel.Validation;

namespace Floozys_Hotel.Models
{
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
        [Required(ErrorMessage = "Start date is required")]
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
        [Required(ErrorMessage = "End date is required")]
        [DateGreaterThan(nameof(StartDate), ErrorMessage = "End date must be after start date")]
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
        [Required(ErrorMessage = "Check-in time is required")]
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
        [Required(ErrorMessage = "Check-out time is required")]
        [DateGreaterThan(nameof(CheckInTime), ErrorMessage = "Check-out time must be after check-in time")]
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

        // Booking status - UC02 (filter by status), UC03 (can be updated)
        private BookingStatus _status;
        public BookingStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        // Foreign Keys
        private int _roomID;
        [Required(ErrorMessage = "Room is required")]
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

        private int _guestID;
        [Required(ErrorMessage = "Guest is required")]
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

        // Navigation properties
        private Room _room;
        public Room Room
        {
            get => _room;
            set
            {
                _room = value;
                OnPropertyChanged();
            }
        }

        private Guest _guest;
        public Guest Guest
        {
            get => _guest;
            set
            {
                _guest = value;
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