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

        public BookingStatus Status { get; set; }

        public int RoomID { get; set; }

        public int GuestID { get; set; }

        // CALCULATED PROPERTIES

        public TimeSpan Duration
        {
            get
            {
                if (StartDate == default || EndDate == default)
                    return TimeSpan.Zero;

                return EndDate - StartDate;
            }
        }

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

            if (RoomID <= 0)
                errors.Add("Room is required");

            if (GuestID <= 0)
                errors.Add("Guest is required");

            if (CheckOutTime.HasValue && CheckInTime.HasValue && CheckOutTime.Value <= CheckInTime.Value)
                errors.Add("Check-out time must be after check-in time");

            return errors;
        }
    }
}
