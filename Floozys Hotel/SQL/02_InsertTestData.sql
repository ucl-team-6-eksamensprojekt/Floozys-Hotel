-- ============================================================
-- SQL Test Data Script for Floozys Hotel
-- Inserts 10 Rooms, 100 Guests, and Random Bookings for 5 Years
-- ============================================================

USE HotelBooking;
GO

SET NOCOUNT ON;

-- 1. Clear existing data
DELETE FROM BOOKING;
DELETE FROM GUEST;
DELETE FROM ROOM;
DBCC CHECKIDENT ('BOOKING', RESEED, 0);
DBCC CHECKIDENT ('GUEST', RESEED, 0);
DBCC CHECKIDENT ('ROOM', RESEED, 0);

-- 2. Insert 10 Rooms
INSERT INTO ROOM (RoomNumber, Floor, RoomSize, Capacity, Status) VALUES
(101, 1, 'Single', 1, 0),
(102, 1, 'Double', 2, 0),
(103, 1, 'Double', 2, 0),
(104, 1, 'Suite', 4, 0),
(105, 1, 'Double', 2, 0),
(201, 2, 'Single', 1, 0),
(202, 2, 'Double', 2, 0),
(203, 2, 'Suite', 4, 0),
(204, 2, 'Double', 2, 0),
(205, 2, 'Double', 2, 0);

PRINT 'Inserted 10 Rooms.';

-- 3. Insert 100 Guests
-- Using a temporary table or value constructor for cleaner code
INSERT INTO GUEST (FirstName, LastName, Email, Country, PhoneNumber, PassportNumber) VALUES
('John', 'Smith', 'john.smith@gmail.com', 'USA', '+1 555-0101', 'US12345678'),
('Emma', 'Johnson', 'emma.j@yahoo.com', 'UK', '+44 20 7946 0000', 'UK98765432'),
('Hans', 'Müller', 'hans.m@t-online.de', 'Germany', '+49 30 123456', 'DE56781234'),
('Sofia', 'Rossi', 'sofia.r@libero.it', 'Italy', '+39 06 1234567', 'IT45678901'),
('Lars', 'Jensen', 'lars.j@mail.dk', 'Denmark', '+45 20 30 40 50', 'DK11223344'),
('Mette', 'Nielsen', 'mette.n@hotmail.com', 'Denmark', '+45 21 31 41 51', 'DK55667788'),
('Pierre', 'Dubois', 'pierre.d@orange.fr', 'France', '+33 1 23 45 67 89', 'FR33445566'),
('Maria', 'Garcia', 'maria.g@outlook.es', 'Spain', '+34 91 123 45 67', 'ES77889900'),
('Yuki', 'Tanaka', 'yuki.t@softbank.jp', 'Japan', '+81 3 1234 5678', 'JP99887766'),
('Wei', 'Zhang', 'wei.z@qq.com', 'China', '+86 10 1234 5678', 'CN11223344'),
('Anders', 'Andersen', 'anders.a@gmail.com', 'Denmark', '+45 22 33 44 55', 'DK99881122'),
('Karen', 'Kristensen', 'karen.k@mail.dk', 'Denmark', '+45 23 34 45 56', 'DK22334455'),
('Ole', 'Olsen', 'ole.o@yahoo.dk', 'Denmark', '+45 24 35 46 57', 'DK33445566'),
('Sven', 'Svensson', 'sven.s@telia.se', 'Sweden', '+46 8 123 45 67', 'SE44556677'),
('Ingrid', 'Johansen', 'ingrid.j@telenor.no', 'Norway', '+47 22 33 44 55', 'NO55667788'),
('Pekka', 'Virtanen', 'pekka.v@elisa.fi', 'Finland', '+358 9 123 4567', 'FI66778899'),
('Jan', 'Kowalski', 'jan.k@wp.pl', 'Poland', '+48 22 123 45 67', 'PL77889900'),
('Anna', 'Novakova', 'anna.n@seznam.cz', 'Czech Republic', '+420 2 12 34 56 78', 'CZ88990011'),
('Liam', 'O''Connor', 'liam.o@eircom.net', 'Ireland', '+353 1 234 5678', 'IE99001122'),
('Sophie', 'Van Dam', 'sophie.v@proximus.be', 'Belgium', '+32 2 123 45 67', 'BE00112233');

-- Generate 80 more guests programmatically to reach 100
DECLARE @i INT = 21;
WHILE @i <= 100
BEGIN
    INSERT INTO GUEST (FirstName, LastName, Email, Country, PhoneNumber, PassportNumber)
    VALUES (
        'Guest' + CAST(@i AS NVARCHAR(10)),
        'lastname' + CAST(@i AS NVARCHAR(10)),
        'guest' + CAST(@i AS NVARCHAR(10)) + '@example.com',
        CASE (@i % 5)
            WHEN 0 THEN 'USA'
            WHEN 1 THEN 'Denmark'
            WHEN 2 THEN 'Germany'
            WHEN 3 THEN 'Sweden'
            ELSE 'UK'
        END,
        '+00 ' + CAST(10000000 + @i AS NVARCHAR(20)),
        'PASSPORT' + CAST(@i AS NVARCHAR(10))
    );
    SET @i = @i + 1;
END

PRINT 'Inserted 100 Guests.';

-- 4. Generate Random Bookings for the next 5 years
-- We will iterate through each room and try to fill it with random bookings
-- spanning from today up to 5 years in the future.

PRINT 'Generating bookings...';

DECLARE @StartDate DATE = CAST(GETDATE() AS DATE);
DECLARE @EndDate DATE = DATEADD(YEAR, 5, @StartDate);
DECLARE @CurrentDate DATE;
DECLARE @BookingLength INT;
DECLARE @GuestID INT;
DECLARE @RoomID INT;

-- Use a cursor to iterate through ALL actual RoomIDs currently in the table
DECLARE RoomCursor CURSOR FOR 
SELECT RoomID FROM ROOM;

OPEN RoomCursor;
FETCH NEXT FROM RoomCursor INTO @RoomID;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @CurrentDate = @StartDate; -- Start from today for each room
    
    -- Add a random initial offset so bookings aren't perfectly aligned
    SET @CurrentDate = DATEADD(DAY, ABS(CHECKSUM(NEWID())) % 10, @CurrentDate);

    WHILE @CurrentDate < @EndDate
    BEGIN
        -- Random booking length between 1 and 14 days
        SET @BookingLength = (ABS(CHECKSUM(NEWID())) % 14) + 1;
        
        -- Select a RANDOM existing GuestID
        -- (This is safer than assuming IDs 1-100 exist sequentially)
        SELECT TOP 1 @GuestID = GuestID FROM GUEST ORDER BY NEWID();
        
        -- Booking Status logic
        DECLARE @Status INT;
        DECLARE @RandStatus INT = ABS(CHECKSUM(NEWID())) % 100;
        
        IF @CurrentDate < CAST(GETDATE() AS DATE) -- Past bookings
        BEGIN
             IF @RandStatus < 5 SET @Status = 4; -- Cancelled
             ELSE SET @Status = 3; -- CheckedOut
        END
        ELSE -- Future bookings
        BEGIN
             IF @RandStatus < 80 SET @Status = 1; -- Confirmed
             ELSE IF @RandStatus < 90 SET @Status = 0; -- Pending
             ELSE SET @Status = 4; -- Cancelled
        END

        -- Insert Booking
        -- Check if GuestID is not null just in case
        IF @GuestID IS NOT NULL
        BEGIN
            INSERT INTO BOOKING (StartDate, EndDate, Status, RoomID, GuestID, CheckInTime, CheckOutTime)
            VALUES (
                @CurrentDate, 
                DATEADD(DAY, @BookingLength, @CurrentDate), 
                @Status, 
                @RoomID, 
                @GuestID,
                NULL,
                NULL
            );
        END

        -- Move current date forward by booking length + gap (0-5 days)
        SET @CurrentDate = DATEADD(DAY, @BookingLength + (ABS(CHECKSUM(NEWID())) % 5), @CurrentDate);
    END
    
    FETCH NEXT FROM RoomCursor INTO @RoomID;
END

CLOSE RoomCursor;
DEALLOCATE RoomCursor;

PRINT 'Generated bookings for the next 5 years.';
