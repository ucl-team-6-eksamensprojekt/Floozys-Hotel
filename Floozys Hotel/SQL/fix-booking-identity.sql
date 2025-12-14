/*
 * FIX: Add IDENTITY to BookingID column
 * Problem: BookingID was not auto-incrementing, causing all IDs to be 0
 * Solution: Recreate BOOKING table with IDENTITY(1,1) on BookingID
 * 
 * IMPORTANT: Run this ONCE on your local database
 * Date: 2025-12-13
 * Author: Michael Kragh
 */

-- Create a new table with correct structure
CREATE TABLE BOOKING_NEW (
    BookingID INT IDENTITY(1,1) PRIMARY KEY,
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NOT NULL,
    CheckInTime DATETIME2 NULL,
    CheckOutTime DATETIME2 NULL,
    Status INT NOT NULL,
    RoomID INT NOT NULL,
    GuestID INT NOT NULL,
    FOREIGN KEY (RoomID) REFERENCES ROOM(RoomID),
    FOREIGN KEY (GuestID) REFERENCES GUEST(GuestID)
);

-- Copy existing data (it will get new auto-incrementing IDs)
SET IDENTITY_INSERT BOOKING_NEW OFF;
INSERT INTO BOOKING_NEW (StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID)
SELECT StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID
FROM BOOKING;

-- Drop old table and rename new one
DROP TABLE BOOKING;
EXEC sp_rename 'BOOKING_NEW', 'BOOKING';

-- Verify the fix worked
SELECT TOP 5 BookingID, StartDate, EndDate, RoomID, GuestID, Status
FROM BOOKING
ORDER BY BookingID;

PRINT 'BookingID IDENTITY fix completed successfully!';