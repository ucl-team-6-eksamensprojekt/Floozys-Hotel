-- ============================================================
-- Script to alter RoomNumber from INT to NVARCHAR(20)
-- Matches C# Model definitions
-- ============================================================

USE HotelBooking;
GO

PRINT 'Altering RoomNumber column to NVARCHAR(20)...';

-- Alter the column type
ALTER TABLE ROOM
ALTER COLUMN RoomNumber NVARCHAR(20) NOT NULL;
GO

PRINT 'RoomNumber column successfully changed to NVARCHAR(20).';
