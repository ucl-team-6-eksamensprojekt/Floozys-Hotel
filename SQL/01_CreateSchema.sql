-- ============================================================
-- SQL Database Schema for Floozys Hotel Booking System
-- Database: HotelBooking
-- ============================================================

-- Check if database exists, create if not
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'HotelBooking')
BEGIN
    CREATE DATABASE HotelBooking;
    PRINT 'Database HotelBooking created.';
END
ELSE
BEGIN
    PRINT 'Database HotelBooking already exists.';
END
GO

USE HotelBooking;
GO

-- Drop existing tables if they exist (for clean setup)
IF OBJECT_ID('BOOKING', 'U') IS NOT NULL DROP TABLE BOOKING;
IF OBJECT_ID('GUEST', 'U') IS NOT NULL DROP TABLE GUEST;
IF OBJECT_ID('ROOM', 'U') IS NOT NULL DROP TABLE ROOM;
GO

-- ============================================================
-- Table: ROOM
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
-- Table: GUEST
-- ============================================================
CREATE TABLE GUEST (
    GuestID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    PassportNumber NVARCHAR(50),
    Email NVARCHAR(100) NOT NULL,
    Country NVARCHAR(50) NOT NULL,
    PhoneNumber NVARCHAR(50) NOT NULL
);
GO

-- ============================================================
-- Table: BOOKING
-- ============================================================
CREATE TABLE BOOKING (
    BookingID INT PRIMARY KEY IDENTITY(1,1),
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    CheckInTime DATETIME2 NULL,
    CheckOutTime DATETIME2 NULL,
    Status INT NOT NULL DEFAULT 0,  -- 0=Pending, 1=Confirmed, 2=CheckedIn, 3=CheckedOut, 4=Cancelled
    RoomID INT NOT NULL,
    GuestID INT NOT NULL,
    CONSTRAINT FK_Booking_Room FOREIGN KEY (RoomID) REFERENCES ROOM(RoomID),
    CONSTRAINT FK_Booking_Guest FOREIGN KEY (GuestID) REFERENCES GUEST(GuestID)
);
GO

-- ============================================================
-- Create indexes for better query performance
-- ============================================================
CREATE INDEX IX_Booking_StartDate ON BOOKING(StartDate);
CREATE INDEX IX_Booking_EndDate ON BOOKING(EndDate);
CREATE INDEX IX_Booking_Status ON BOOKING(Status);
CREATE INDEX IX_Booking_RoomID ON BOOKING(RoomID);
CREATE INDEX IX_Booking_GuestID ON BOOKING(GuestID);
GO

PRINT 'Database schema created successfully!';
