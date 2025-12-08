-- ============================================================
-- Test Data for Floozys Hotel Booking System
-- ============================================================

-- ============================================================
-- Insert ROOM test data
-- ============================================================
INSERT INTO ROOM (RoomNumber, Floor, RoomSize, Capacity, Status) VALUES
(101, 1, 'Single', 1, 0),      -- Available
(102, 1, 'Double', 2, 0),      -- Available
(103, 1, 'Suite', 4, 0),       -- Available
(201, 2, 'Single', 1, 0),      -- Available
(202, 2, 'Double', 2, 0),      -- Available
(203, 2, 'Suite', 4, 1),       -- Out of Service
(301, 3, 'Double', 2, 0),      -- Available
(302, 3, 'Suite', 4, 0);       -- Available
GO

-- ============================================================
-- Insert GUEST test data
-- ============================================================
INSERT INTO GUEST (FirstName, LastName, PassportNumber, Email, Country, PhoneNumber) VALUES
('Anna', 'Smith', 'DK1234567', 'anna.smith@mail.com', 'Denmark', '+4512345678'),
('John', 'Doe', 'US9876543', 'john.doe@mail.com', 'USA', '+1234567890'),
('Maria', 'Garcia', 'ES1122334', 'maria.garcia@mail.com', 'Spain', '+34611223344'),
('Lars', 'Nielsen', 'DK7654321', 'lars.nielsen@mail.com', 'Denmark', '+4587654321'),
('Emma', 'Johnson', 'UK5566778', 'emma.johnson@mail.com', 'UK', '+447700900123');
GO

-- ============================================================
-- Insert BOOKING test data
-- ============================================================
INSERT INTO BOOKING (StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID) VALUES
-- Confirmed bookings with check-in/out
('2024-12-01', '2024-12-05', '2024-12-01 14:00:00', NULL, 2, 1, 1),              -- Checked In
('2024-12-03', '2024-12-07', '2024-12-03 15:30:00', '2024-12-07 11:00:00', 3, 2, 2),  -- Checked Out

-- Future bookings (confirmed)
('2024-12-10', '2024-12-15', NULL, NULL, 1, 3, 3),  -- Confirmed
('2024-12-12', '2024-12-18', NULL, NULL, 1, 4, 4),  -- Confirmed
('2024-12-20', '2024-12-25', NULL, NULL, 1, 5, 5),  -- Confirmed

-- Pending booking
('2024-12-08', '2024-12-10', NULL, NULL, 0, 7, 1),  -- Pending

-- Cancelled booking
('2024-11-25', '2024-11-30', NULL, NULL, 4, 8, 3);  -- Cancelled
GO

PRINT 'Test data inserted successfully!';
PRINT 'Total Rooms: 8';
PRINT 'Total Guests: 5';
PRINT 'Total Bookings: 7';
