```mermaid
classDiagram
    direction TB

    %% ---------- DOMAIN LAYER ----------
    class Booking {
        int BookingID
        DateTime StartDate
        DateTime EndDate
        DateTime? CheckInTime
        DateTime? CheckOutTime
        BookingStatus Status
        int NumberOfNights
    }

    class Room {
        int RoomId
        string RoomNumber
        int Floor
        string RoomSize
        int Capacity
        RoomStatus Status
    }

    class Guest {
        int GuestID
        string FirstName
        string LastName
        string Email
        string PhoneNumber
        string Country
        string PassportNumber
    }

    class BookingStatus {
        <<enumeration>>
        Pending
        Confirmed
        CheckedIn
        CheckedOut
        Cancelled
    }

    class RoomStatus {
        <<enumeration>>
        Available
        OutOfService
        Maintenance
    }

    %% Assoc mellem domain-objekter
    Booking "0..*" --> "1" Room : room
    Booking "0..*" --> "1" Guest : guest
    Booking --> BookingStatus : Status
    Room --> RoomStatus : Status


    %% ---------- REPOSITORY INTERFACES ----------
    class IBookingRepo {
        <<interface>>
        +void Create(Booking booking)
        +List~Booking~ GetAll()
        +Booking GetById(int bookingID)
        +List~Booking~ GetByStatus(BookingStatus status)
        +List~Booking~ GetByRoomID(int roomID)
        +List~Booking~ GetByGuestID(int guestID)
        +void Update(Booking booking)
        +void Delete(int bookingID)
    }

    class IRoomRepo {
        <<interface>>
        +void CreateRoom(Room room)
        +Room GetById(int roomId)
        +List~Room~ GetAll()
        +List~Room~ GetRoomsFromCriteria(int? floor, string? roomSize, RoomStatus? status)
        +void UpdateRoom(Room room)
        +void DeleteRoom(int roomId)
    }

    class IGuestRepo {
        <<interface>>
        +void CreateGuest(Guest guest)
        +Guest GetByID(int id)
        +List~Guest~ GetAll()
        +List~Guest~ GetAllByName(string name)
        +void UpdateGuest(Guest guest)
        +void DeleteGuest(int id)
    }

    %% ---------- CONCRETE REPOSITORIES ----------
    class BookingRepo {
        +void Create(Booking booking)
        +List~Booking~ GetAll()
        +Booking GetById(int bookingID)
        +List~Booking~ GetByStatus(BookingStatus status)
        +List~Booking~ GetByRoomID(int roomID)
        +List~Booking~ GetByGuestID(int guestID)
        +void Update(Booking booking)
        +void Delete(int bookingID)
    }

    class RoomRepo {
        +void CreateRoom(Room room)
        +Room GetById(int roomId)
        +List~Room~ GetAll()
        +List~Room~ GetRoomsFromCriteria(int? floor, string roomSize, RoomStatus? status)
        +void UpdateRoom(Room room)
        +void DeleteRoom(int roomId)
        +List~Room~ GetAllByAvailability()
    }

    class GuestRepo {
        +List~Guest~ GetAllGuests()
        +Guest GetGuestById(int guestId)
        +int AddGuest(Guest guest)
        +void UpdateGuest(Guest guest)
        +void DeleteGuest(int guestId)
    }

    %% Realization (implements)
    BookingRepo ..|> IBookingRepo
    RoomRepo ..|> IRoomRepo
    GuestRepo ..|> IGuestRepo

    %% Repositories "manages" mange entities
    BookingRepo "1" --> "0..*" Booking : manages
    BookingRepo "1" --> "0..*" Room : uses
    BookingRepo "1" --> "0..*" Guest : uses

    RoomRepo "1" --> "0..*" Room : manages
    GuestRepo "1" --> "0..*" Guest : manages

    %% ---------- INFRASTRUCTURE ----------
    class DatabaseConfig {
        +string ConnectionString
    }

    RoomRepo "1" --> "1" DatabaseConfig : uses
    GuestRepo "1" --> "1" DatabaseConfig : uses
