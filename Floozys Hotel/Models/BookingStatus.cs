/// <summary>
/// Booking status enum - UC02: View Bookings (filter criteria)
/// </summary>
public enum BookingStatus
{
    Pending,      // Booking created but not confirmed
    Confirmed,    // Booking confirmed by guest/payment
    CheckedIn,    // Guest has checked in
    CheckedOut,   // Guest has checked out
    Cancelled     // Booking cancelled
}