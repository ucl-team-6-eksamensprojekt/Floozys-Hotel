using System;
using System.Collections.Generic;

namespace Floozys_Hotel.Models
{
    public class Booking
    {
        // PROPERTIES

        public int BookingID { get; set; }

        // Display-friendly booking number
        public string BookingNumber => $"FLZ-{BookingID:D6}";

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public BookingStatus Status { get; set; }

        // FOREIGN KEYS
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

        public List<string> Validate()
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

        // BUSINESS LOGIC

        // UC: Check-in allowed for both Pending and Confirmed when guest arrives
        public bool CanCheckIn()
        {
            return (Status == BookingStatus.Pending || Status == BookingStatus.Confirmed) &&
                   StartDate.Date <= DateTime.Today &&
                   !CheckInTime.HasValue;
        }

        // UC: Check-out. Business rules for check-out eligibility
        public bool CanCheckOut()
        {
            return Status == BookingStatus.CheckedIn &&
                   CheckInTime.HasValue &&
                   !CheckOutTime.HasValue;
        }

        public void PerformCheckIn()
        {
            if (!CanCheckIn())
                throw new InvalidOperationException("Cannot check in this booking");

            CheckInTime = DateTime.Now;
            Status = BookingStatus.CheckedIn;
        }

        public void PerformCheckOut()
        {
            if (!CanCheckOut())
                throw new InvalidOperationException("Cannot check out this booking");

            CheckOutTime = DateTime.Now;
            Status = BookingStatus.CheckedOut;
        }

        // BUSINESS LOGIC - CANCELLATION

        // UC04: Business rules for cancellation eligibility
        public bool CanCancel()
        {
            return Status == BookingStatus.Pending || Status == BookingStatus.Confirmed;
        }

        // UC04: Cancel booking operation
        public void CancelBooking()
        {
            if (!CanCancel())
                throw new InvalidOperationException("Cannot cancel this booking");

            Status = BookingStatus.Cancelled;
        }

        // BUSINESS LOGIC - EDITING

        // UC03: Check if booking can be edited
        public bool CanEdit()
        {
            return Status == BookingStatus.Pending || Status == BookingStatus.Confirmed;
        }

        // UC03: Validate edit changes before saving
        public List<string> ValidateEdit(DateTime newStartDate, DateTime newEndDate)
        {
            var errors = new List<string>();

            if (newEndDate <= newStartDate)
                errors.Add("End date must be after start date");

            if (newStartDate.Date < DateTime.Now.Date)
                errors.Add("Start date cannot be in the past");

            return errors;
        }
    }
}