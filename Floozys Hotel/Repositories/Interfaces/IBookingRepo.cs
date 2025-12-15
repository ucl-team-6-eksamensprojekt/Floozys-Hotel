using Floozys_Hotel.Models;

namespace Floozys_Hotel.Repositories.Interfaces
{
    public interface IBookingRepo
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

        // UC: CHECK-IN/CHECK-OUT
        void CheckIn(int bookingID);
        void CheckOut(int bookingID);

        // UC04: Cancels a booking
        void CancelBooking(int bookingID);

        // UC03: Edit booking
        void EditBooking(int bookingID, DateTime newStartDate, DateTime newEndDate, int newRoomID, int newGuestID);
    }
}