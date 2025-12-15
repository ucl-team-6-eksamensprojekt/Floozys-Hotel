using System;
using System.Collections.Generic;

namespace Floozys_Hotel.Models
{
    public class Booking
    {
        // PROPERTIES

        public int BookingID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime? CheckInTime { get; set; }  // Nullable because guest may not have checked in yet

        public DateTime? CheckOutTime { get; set; }  // Nullable because guest may not have checked out yet

        // ENUM
        public BookingStatus Status { get; set; }

        // FOREIGN KEYS - Added these since we need them to correctly map the data from the database :-)
        public int RoomID { get; set; }
        public int GuestID { get; set; }

        public Room Room { get; set; }

        public Guest Guest { get; set; }

        // CALCULATED PROPERTIES

        public int NumberOfNights
        {
            get
            {
                if (StartDate == default || EndDate == default)
                    return 0;

                return (EndDate - StartDate).Days;
            }
        }

        // VALIDATION

        public List<string> Validate()  // Collects all validation errors instead of throwing on first error
        {
            var errors = new List<string>();

            if (StartDate == default)
                errors.Add("Start date is required");

            if (EndDate == default)
                errors.Add("End date is required");

            if (EndDate != default && StartDate != default && EndDate <= StartDate)
                errors.Add("End date must be after start date");

            if (StartDate != default && StartDate.Date < DateTime.Now.Date)
                errors.Add("Start date cannot be in the past");

            if (Room == null)
                errors.Add("Room is required");

            if (Guest == null)
                errors.Add("Guest is required");

            if (CheckOutTime.HasValue && CheckInTime.HasValue && CheckOutTime.Value <= CheckInTime.Value)
                errors.Add("Check-out time must be after check-in time");

            return errors;
        }

        // HELPER METHODS FOR CHECK-IN/CHECK-OUT

        /// <summary>
        /// Checks if booking can be checked in today
        /// </summary>
        public bool CanCheckIn()
        {
            return Status == BookingStatus.Confirmed &&
                   StartDate.Date <= DateTime.Today &&
                   !CheckInTime.HasValue;
        }

        /// <summary>
        /// Checks if booking can be checked out
        /// </summary>
        public bool CanCheckOut()
        {
            return Status == BookingStatus.CheckedIn &&
                   CheckInTime.HasValue &&
                   !CheckOutTime.HasValue;
        }

        /// <summary>
        /// Performs check-in operation
        /// </summary>
        public void PerformCheckIn()
        {
            if (!CanCheckIn())
                throw new InvalidOperationException("Cannot check in this booking");

            CheckInTime = DateTime.Now;
            Status = BookingStatus.CheckedIn;
        }

        /// <summary>
        /// Performs check-out operation
        /// </summary>
        public void PerformCheckOut()
        {
            if (!CanCheckOut())
                throw new InvalidOperationException("Cannot check out this booking");

            CheckOutTime = DateTime.Now;
            Status = BookingStatus.CheckedOut;
        }
    }
}