
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Floozys_Hotel.Validation;

namespace Floozys_Hotel.Models
{
    class Booking
    {
        // Primary Key (IDENTITY in SQL)
        public int BookingID { get; init; }

        // Start Date (DATE in SQL)
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        // End Date (DATE in SQL) - Must be AFTER StartDate
        [Required(ErrorMessage = "End date is required")]
        [DateGreaterThan(nameof(StartDate), ErrorMessage = "End date must be after start date")]
        public DateTime EndDate { get; set; }

        // Check-In Time (DATETIME2 in SQL)
        [Required(ErrorMessage = "Check-in time is required")]
        public DateTime CheckInTime { get; set; }

        // Check-Out Time (DATETIME2 in SQL) - Must be AFTER CheckInTime
        [Required(ErrorMessage = "Check-out time is required")]
        [DateGreaterThan(nameof(CheckInTime), ErrorMessage = "Check-out time must be after check-in time")]
        public DateTime CheckOutTime { get; set; }

        // Foreign Keys
        [Required(ErrorMessage = "Room is required")]
        public int RoomID { get; set; }

        [Required(ErrorMessage = "Guest is required")]
        public int GuestID { get; set; }

    }
}