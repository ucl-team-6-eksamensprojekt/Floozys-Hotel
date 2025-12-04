-- ============================================================
-- Azure SQL Database Schema til Floozys Hotel Booking System
-- Database: HotelBooking
-- Server: g6hotelbooking.database.windows.net
-- ============================================================

-- Sletter eksisterende tabeller hvis de findes (til ren opsætning).
IF OBJECT_ID('BOOKING', 'U') IS NOT NULL DROP TABLE BOOKING;
IF OBJECT_ID('GUEST', 'U') IS NOT NULL DROP TABLE GUEST;
IF OBJECT_ID('ROOM', 'U') IS NOT NULL DROP TABLE ROOM;
GO

-- ============================================================
-- Tabel: ROOM - Værelser i hotellet
-- ============================================================
CREATE TABLE ROOM (
    RoomID INT PRIMARY KEY IDENTITY(1,1),
    RoomNumber INT NOT NULL,
    Floor INT NOT NULL,
    RoomSize NVARCHAR(20) NOT NULL,
    Capacity INT NOT NULL,
    Status INT NOT NULL DEFAULT 0  -- 0=Available, 1=OutOfService, 2=Maintenance
);
GO

-- ============================================================
-- Tabel: GUEST - Gæster der booker værelser
-- ============================================================
CREATE TABLE GUEST (
    GuestID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    PassportNumber NVARCHAR(50),  -- Nullable felt
    Email NVARCHAR(100) NOT NULL,
    Country NVARCHAR(50) NOT NULL,
    PhoneNumber NVARCHAR(50) NOT NULL
);
GO

-- ============================================================
-- Tabel: BOOKING - Bookinger der kobler gæster til værelser
-- ============================================================
CREATE TABLE BOOKING (
    BookingID INT PRIMARY KEY IDENTITY(1,1),
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    CheckInTime DATETIME2 NULL,  -- Nullable, udfyldes ved faktisk check-in
    CheckOutTime DATETIME2 NULL,  -- Nullable, udfyldes ved faktisk check-out
    Status INT NOT NULL DEFAULT 0,  -- 0=Pending, 1=Confirmed, 2=CheckedIn, 3=CheckedOut, 4=Cancelled
    RoomID INT NOT NULL,
    GuestID INT NOT NULL,
    -- Foreign keys til ROOM og GUEST tabellerne
    CONSTRAINT FK_Booking_Room FOREIGN KEY (RoomID) REFERENCES ROOM(RoomID),
    CONSTRAINT FK_Booking_Guest FOREIGN KEY (GuestID) REFERENCES GUEST(GuestID)
);
GO

-- ============================================================
-- Opretter indexes for bedre query-performance
-- ============================================================
CREATE INDEX IX_Booking_StartDate ON BOOKING(StartDate);
CREATE INDEX IX_Booking_EndDate ON BOOKING(EndDate);
CREATE INDEX IX_Booking_Status ON BOOKING(Status);
CREATE INDEX IX_Booking_RoomID ON BOOKING(RoomID);
CREATE INDEX IX_Booking_GuestID ON BOOKING(GuestID);
GO

PRINT 'Database schema oprettet succesfuldt!';
