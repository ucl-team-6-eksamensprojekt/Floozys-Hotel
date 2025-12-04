using System.Collections.Generic;
using Floozys_Hotel.Models;

namespace Floozys_Hotel.Repositories.Interfaces
{
    public interface IBooking
    {
        // CREATE
        void Create(Booking booking);

        // READ
        List<Booking> GetAll();
        Booking GetById(int bookingID);
        List<Booking> GetByStatus(BookingStatus status);
        List<Booking> GetByRoomID(int roomID);
        List<Booking> GetByGuestID(int guestID);

        // UPDATE
        void Update(Booking booking);

        // DELETE
        void Delete(int bookingID);
    }
}