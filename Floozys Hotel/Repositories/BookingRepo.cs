using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Floozys_Hotel.Models;

namespace Floozys_Hotel.Repositories
{
    public class BookingRepo
    {
        private List<Booking> _bookings;

        public BookingRepo()
        {
            _bookings = new List<Booking>
            {
                new Booking { BookingID = 1, RoomID = 1, GuestID = 1, StartDate = DateTime.Today.AddDays(-2), EndDate = DateTime.Today.AddDays(3), CheckInTime = DateTime.Now, CheckOutTime = DateTime.Now },
                new Booking { BookingID = 2, RoomID = 2, GuestID = 2, StartDate = DateTime.Today.AddDays(5), EndDate = DateTime.Today.AddDays(10), CheckInTime = DateTime.Now, CheckOutTime = DateTime.Now },
                new Booking { BookingID = 3, RoomID = 1, GuestID = 3, StartDate = DateTime.Today.AddDays(12), EndDate = DateTime.Today.AddDays(15), CheckInTime = DateTime.Now, CheckOutTime = DateTime.Now },
                new Booking { BookingID = 4, RoomID = 4, GuestID = 4, StartDate = DateTime.Today.AddDays(-5), EndDate = DateTime.Today.AddDays(-1), CheckInTime = DateTime.Now, CheckOutTime = DateTime.Now },
                 new Booking { BookingID = 5, RoomID = 5, GuestID = 5, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(5), CheckInTime = DateTime.Now, CheckOutTime = DateTime.Now },
            };
        }

        public List<Booking> GetAllBookings()
        {
            return _bookings;
        }
    }
}
