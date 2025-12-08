using System;
using System.Collections.Generic;
using System.Linq;
using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories.Interfaces;

namespace Floozys_Hotel.Repositories
{
    public class BookingRepo : IBooking
    {
        private List<Booking> _bookings;  // In-memory storage simulates database
        private int _nextBookingID;  // Simulates database auto-increment

        // CONSTRUCTOR

        public BookingRepo()
        {
            // Get sample rooms and guests for test data
            var roomRepo = new RoomRepo();
            var rooms = roomRepo.GetAll();

            _bookings = new List<Booking>
            {
                new Booking
                {
                    BookingID = 1,
                    Room = rooms.FirstOrDefault(r => r.RoomId == 1),
                    Guest = new Guest { GuestID = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "+4512345678", Country = "Denmark" },
                    StartDate = DateTime.Today.AddDays(-2),
                    EndDate = DateTime.Today.AddDays(3),
                    CheckInTime = DateTime.Now.AddDays(-2),
                    CheckOutTime = null,
                    Status = BookingStatus.CheckedIn
                },
                new Booking
                {
                    BookingID = 2,
                    Room = rooms.FirstOrDefault(r => r.RoomId == 2),
                    Guest = new Guest { GuestID = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", PhoneNumber = "+4587654321", Country = "Sweden" },
                    StartDate = DateTime.Today.AddDays(5),
                    EndDate = DateTime.Today.AddDays(10),
                    CheckInTime = null,
                    CheckOutTime = null,
                    Status = BookingStatus.Confirmed
                },
                new Booking
                {
                    BookingID = 3,
                    Room = rooms.FirstOrDefault(r => r.RoomId == 1),
                    Guest = new Guest { GuestID = 3, FirstName = "Bob", LastName = "Johnson", Email = "bob@example.com", PhoneNumber = "+4511223344", Country = "Norway" },
                    StartDate = DateTime.Today.AddDays(12),
                    EndDate = DateTime.Today.AddDays(15),
                    CheckInTime = null,
                    CheckOutTime = null,
                    Status = BookingStatus.Pending
                },
                new Booking
                {
                    BookingID = 4,
                    Room = rooms.FirstOrDefault(r => r.RoomId == 4),
                    Guest = new Guest { GuestID = 4, FirstName = "Alice", LastName = "Brown", Email = "alice@example.com", PhoneNumber = "+4599887766", Country = "Finland" },
                    StartDate = DateTime.Today.AddDays(-5),
                    EndDate = DateTime.Today.AddDays(-1),
                    CheckInTime = DateTime.Now.AddDays(-5),
                    CheckOutTime = DateTime.Now.AddDays(-1),
                    Status = BookingStatus.CheckedOut
                },
                new Booking
                {
                    BookingID = 5,
                    Room = rooms.FirstOrDefault(r => r.RoomId == 5),
                    Guest = new Guest { GuestID = 5, FirstName = "Charlie", LastName = "Wilson", Email = "charlie@example.com", PhoneNumber = "+4544332211", Country = "Iceland" },
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(5),
                    CheckInTime = DateTime.Now,
                    CheckOutTime = null,
                    Status = BookingStatus.CheckedIn
                }
            };

            _nextBookingID = _bookings.Max(b => b.BookingID) + 1;
        }

        // CREATE

        public void Create(Booking booking)  // Adds new booking and assigns ID
        {
            if (booking == null)
            {
                throw new ArgumentNullException(nameof(booking), "Booking cannot be null");
            }

            booking.BookingID = _nextBookingID;
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
            return _bookings.Where(b => b.Room != null && b.Room.RoomId == roomID).ToList();
        }

        public List<Booking> GetByGuestID(int guestID)  // Find all bookings for a guest
        {
            return _bookings.Where(b => b.Guest != null && b.Guest.GuestID == guestID).ToList();
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
            existingBooking.Room = booking.Room;
            existingBooking.Guest = booking.Guest;
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