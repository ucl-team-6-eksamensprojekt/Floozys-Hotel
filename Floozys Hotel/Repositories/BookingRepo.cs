using Floozys_Hotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Interop;
using System.Windows.Media.Animation;

namespace Floozys_Hotel.Repositories
{
    public class BookingRepo
    {
        private List<Booking> _bookings;  // In-memory storage simulates database
        private int _nextBookingID;  // Simulates database auto-increment

        // CONSTRUCTOR

        public BookingRepo()
        {
            _bookings = new List<Booking>
            {
                new Booking
                {
                    BookingID = 1,
                    RoomID = 1,
                    GuestID = 1,
                    StartDate = DateTime.Today.AddDays(-2),
                    EndDate = DateTime.Today.AddDays(3),
                    CheckInTime = DateTime.Now.AddDays(-2),
                    CheckOutTime = null,
                    Status = BookingStatus.CheckedIn
                },
                new Booking
                {
                    BookingID = 2,
                    RoomID = 2,
                    GuestID = 2,
                    StartDate = DateTime.Today.AddDays(5),
                    EndDate = DateTime.Today.AddDays(10),
                    CheckInTime = null,
                    CheckOutTime = null,
                    Status = BookingStatus.Confirmed
                },
                new Booking
                {
                    BookingID = 3,
                    RoomID = 1,
                    GuestID = 3,
                    StartDate = DateTime.Today.AddDays(12),
                    EndDate = DateTime.Today.AddDays(15),
                    CheckInTime = null,
                    CheckOutTime = null,
                    Status = BookingStatus.Pending
                },
                new Booking
                {
                    BookingID = 4,
                    RoomID = 4,
                    GuestID = 4,
                    StartDate = DateTime.Today.AddDays(-5),
                    EndDate = DateTime.Today.AddDays(-1),
                    CheckInTime = DateTime.Now.AddDays(-5),
                    CheckOutTime = DateTime.Now.AddDays(-1),
                    Status = BookingStatus.CheckedOut
                },
                new Booking
                {
                    BookingID = 5,
                    RoomID = 5,
                    GuestID = 5,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(5),
                    CheckInTime = DateTime.Now,
                    CheckOutTime = null,
                    Status = BookingStatus.CheckedIn
                }
            };

            _nextBookingID = _bookings.Max(b => b.BookingID) + 1;  // Start from 6
        }

        // CREATE

        public void Create(Booking booking)  // Adds new booking and assigns ID
        {
            if (booking == null)
            {
                throw new ArgumentNullException(nameof(booking), "Booking cannot be null");
            }

            booking.BookingID = _nextBookingID;  // Assign new ID (simulates database auto-increment)
            _nextBookingID++;
            _bookings.Add(booking);
        }

        // READ

        public List<Booking> GetAll()  // Returns copy to prevent external modification
        {
            return new List<Booking>(_bookings);
        }

        public Booking GetById(int bookingID)
        {
            return _bookings.FirstOrDefault(b => b.BookingID == bookingID);
        }

        public List<Booking> GetByStatus(BookingStatus status)  // Filter by status (Pending, Confirmed, etc.)
        {
            return _bookings.Where(b => b.Status == status).ToList();
        }

        public List<Booking> GetByRoomID(int roomID)  // Find all bookings for a room
        {
            return _bookings.Where(b => b.RoomID == roomID).ToList();
        }

        public List<Booking> GetByGuestID(int guestID)  // Find all bookings for a guest
        {
            return _bookings.Where(b => b.GuestID == guestID).ToList();
        }

        // UPDATE

        public void Update(Booking booking)
        {
            if (booking == null)
            {
                throw new ArgumentNullException(nameof(booking), "Booking cannot be null");
            }

            var existingBooking = _bookings.FirstOrDefault(b => b.BookingID == booking.BookingID);

            if (existingBooking == null)
            {
                throw new ArgumentException($"Booking with ID {booking.BookingID} not found");
            }

            existingBooking.StartDate = booking.StartDate;
            existingBooking.EndDate = booking.EndDate;
            existingBooking.CheckInTime = booking.CheckInTime;
            existingBooking.CheckOutTime = booking.CheckOutTime;
            existingBooking.Status = booking.Status;
            existingBooking.RoomID = booking.RoomID;
            existingBooking.GuestID = booking.GuestID;
        }

        // DELETE

        public void Delete(int bookingID)
        {
            var booking = _bookings.FirstOrDefault(b => b.BookingID == bookingID);

            if (booking == null)
            {
                throw new ArgumentException($"Booking with ID {bookingID} not found");
            }

            _bookings.Remove(booking);
        }
    }
}
