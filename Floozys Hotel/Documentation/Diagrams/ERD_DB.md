```mermaid
classDiagram
    direction LR

    class GUEST {
        <<entity>>
        🔑 GuestID
        FirstName
        LastName
        PassportNumber
        Email
        Country
        PhoneNumber
    }

    class ROOM {
        <<entity>>
        🔑 RoomID
        RoomNumber
        Floor
        RoomSize
        Capacity
    }

    class BOOKING {
        <<entity>>
        🔑 BookingID
        StartDate
        EndDate
        CheckInTime
        CheckOutTime
        🗝️ RoomID
        🗝️ GuestID
    }

    %% Relations exactly like SQL Server:
    %% 1 → many (PK side = |, FK side = o)

    GUEST <-- BOOKING
    ROOM  <-- BOOKING
