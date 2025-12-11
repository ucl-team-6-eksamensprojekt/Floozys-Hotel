```mermaid
sequenceDiagram
    actor User
    participant View as NewBookingView
    participant VM as NewBookingViewModel
    participant RoomRepo
    participant Guest
    participant Booking
    participant BookingRepo
    participant DB as Database

    User ->> View: Fill form + click "Confirm booking"
    View ->> VM: ConfirmBookingCommand

    VM ->> Guest: new Guest(firstName, lastName, ...)
    VM ->> Guest: Validate()
    Guest -->> VM: errors / ok

    VM ->> RoomRepo: GetAllByAvailability()
    RoomRepo -->> VM: List<Room>

    VM ->> Booking: new Booking(StartDate, EndDate, Room, Guest, Status=Pending)
    VM ->> BookingRepo: Create(booking)
    BookingRepo ->> DB: INSERT Booking + Guest (if new)
    DB -->> BookingRepo: OK
    BookingRepo -->> VM: BookingID

    VM -->> View: Show success message
