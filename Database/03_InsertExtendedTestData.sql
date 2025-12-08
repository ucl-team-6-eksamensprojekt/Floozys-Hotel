-- ============================================================
-- Extended Test Data for Floozys Hotel Booking System
-- Date Range: December 2025 - December 2027
-- ============================================================

-- ============================================================
-- Additional GUEST test data
-- ============================================================
INSERT INTO GUEST (FirstName, LastName, PassportNumber, Email, Country, PhoneNumber) VALUES
('Sophie', 'Andersen', 'DK8877665', 'sophie.andersen@mail.dk', 'Denmark', '+4523456789'),
('Michael', 'Schmidt', 'DE5544332', 'michael.schmidt@email.de', 'Germany', '+4917012345678'),
('Isabella', 'Rossi', 'IT9988776', 'isabella.rossi@posta.it', 'Italy', '+393401234567'),
('James', 'Taylor', 'UK7766554', 'james.taylor@mail.co.uk', 'UK', '+447890123456'),
('Yuki', 'Tanaka', 'JP3322114', 'yuki.tanaka@email.jp', 'Japan', '+81901234567'),
('Marie', 'Dubois', 'FR4455667', 'marie.dubois@mail.fr', 'France', '+33612345678'),
('Hans', 'Müller', 'DE6677889', 'hans.muller@email.de', 'Germany', '+4915123456789'),
('Olivia', 'Brown', 'US1122334', 'olivia.brown@mail.com', 'USA', '+12125551234'),
('Lucas', 'Silva', 'BR8899001', 'lucas.silva@email.br', 'Brazil', '+5511987654321'),
('Amelia', 'Wilson', 'AU5566778', 'amelia.wilson@mail.au', 'Australia', '+61412345678'),
('Erik', 'Jensen', 'DK2233445', 'erik.jensen@mail.dk', 'Denmark', '+4531234567'),
('Nina', 'Petersen', 'DK9900112', 'nina.petersen@mail.dk', 'Denmark', '+4542345678'),
('Carlos', 'Rodriguez', 'ES3344556', 'carlos.rodriguez@mail.es', 'Spain', '+34612345678'),
('Anna', 'Kowalski', 'PL7788990', 'anna.kowalski@mail.pl', 'Poland', '+48501234567'),
('Henrik', 'Sørensen', 'DK4455667', 'henrik.sorensen@mail.dk', 'Denmark', '+4553456789');
GO

-- ============================================================
-- BOOKING test data - December 2025
-- ============================================================
INSERT INTO BOOKING (StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID) VALUES
-- Early December 2025 - Confirmed bookings
('2025-12-01', '2025-12-04', NULL, NULL, 1, 1, 6),   -- Sophie in Room 101
('2025-12-02', '2025-12-06', NULL, NULL, 1, 3, 7),   -- Michael in Room 103
('2025-12-05', '2025-12-10', NULL, NULL, 1, 2, 8),   -- Isabella in Room 102

-- Mid December 2025 - Mix of statuses
('2025-12-10', '2025-12-14', NULL, NULL, 1, 4, 9),   -- James in Room 201
('2025-12-12', '2025-12-16', NULL, NULL, 0, 5, 10),  -- Yuki in Room 202 - Pending
('2025-12-15', '2025-12-20', NULL, NULL, 1, 1, 11),  -- Marie in Room 101

-- Christmas period 2025 - High demand
('2025-12-20', '2025-12-27', NULL, NULL, 1, 3, 12),  -- Hans in Room 103 - Christmas week
('2025-12-22', '2025-12-29', NULL, NULL, 1, 7, 13),  -- Olivia in Room 301
('2025-12-23', '2025-12-30', NULL, NULL, 1, 8, 14),  -- Lucas in Room 302 - Suite
('2025-12-24', '2025-12-28', NULL, NULL, 1, 2, 15),  -- Amelia in Room 102

-- New Year period 2025-2026
('2025-12-27', '2026-01-03', NULL, NULL, 1, 4, 16),  -- Erik crossing into new year
('2025-12-29', '2026-01-05', NULL, NULL, 1, 5, 17),  -- Nina crossing into new year
('2025-12-30', '2026-01-02', NULL, NULL, 1, 1, 18);  -- Carlos - New Year
GO

-- ============================================================
-- BOOKING test data - January-March 2026
-- ============================================================
INSERT INTO BOOKING (StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID) VALUES
-- January 2026
('2026-01-05', '2026-01-10', NULL, NULL, 1, 2, 19),   -- Anna
('2026-01-10', '2026-01-15', NULL, NULL, 1, 3, 20),   -- Henrik
('2026-01-15', '2026-01-20', NULL, NULL, 0, 1, 6),    -- Sophie returns - Pending
('2026-01-20', '2026-01-25', NULL, NULL, 1, 7, 7),    -- Michael
('2026-01-25', '2026-01-30', NULL, NULL, 1, 8, 8),    -- Isabella

-- February 2026 - Valentine's Day bookings
('2026-02-01', '2026-02-05', NULL, NULL, 1, 4, 9),    -- James
('2026-02-10', '2026-02-14', NULL, NULL, 1, 8, 10),   -- Yuki - Valentine's Suite
('2026-02-13', '2026-02-16', NULL, NULL, 1, 3, 11),   -- Marie - Valentine's
('2026-02-14', '2026-02-17', NULL, NULL, 1, 2, 12),   -- Hans - Valentine's
('2026-02-20', '2026-02-25', NULL, NULL, 1, 5, 13),   -- Olivia

-- March 2026
('2026-03-01', '2026-03-05', NULL, NULL, 1, 1, 14),   -- Lucas
('2026-03-05', '2026-03-10', NULL, NULL, 1, 7, 15),   -- Amelia
('2026-03-10', '2026-03-15', NULL, NULL, 0, 4, 16),   -- Erik - Pending
('2026-03-15', '2026-03-20', NULL, NULL, 1, 2, 17),   -- Nina
('2026-03-20', '2026-03-25', NULL, NULL, 1, 3, 18),   -- Carlos
('2026-03-25', '2026-03-30', NULL, NULL, 1, 8, 19);   -- Anna - Suite
GO

-- ============================================================
-- BOOKING test data - April-June 2026 (Spring/Summer bookings)
-- ============================================================
INSERT INTO BOOKING (StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID) VALUES
-- April 2026 - Easter period
('2026-04-01', '2026-04-06', NULL, NULL, 1, 1, 20),   -- Henrik
('2026-04-05', '2026-04-10', NULL, NULL, 1, 5, 6),    -- Sophie
('2026-04-10', '2026-04-15', NULL, NULL, 1, 2, 7),    -- Michael
('2026-04-12', '2026-04-18', NULL, NULL, 1, 8, 8),    -- Isabella - Easter Suite
('2026-04-20', '2026-04-25', NULL, NULL, 1, 3, 9),    -- James

-- May 2026
('2026-05-01', '2026-05-05', NULL, NULL, 1, 4, 10),   -- Yuki
('2026-05-05', '2026-05-10', NULL, NULL, 1, 7, 11),   -- Marie
('2026-05-10', '2026-05-15', NULL, NULL, 1, 1, 12),   -- Hans
('2026-05-15', '2026-05-20', NULL, NULL, 0, 2, 13),   -- Olivia - Pending
('2026-05-20', '2026-05-25', NULL, NULL, 1, 8, 14),   -- Lucas - Suite
('2026-05-25', '2026-05-30', NULL, NULL, 1, 5, 15),   -- Amelia

-- June 2026 - Summer beginning
('2026-06-01', '2026-06-07', NULL, NULL, 1, 3, 16),   -- Erik - Week stay
('2026-06-05', '2026-06-10', NULL, NULL, 1, 4, 17),   -- Nina
('2026-06-10', '2026-06-15', NULL, NULL, 1, 7, 18),   -- Carlos
('2026-06-15', '2026-06-22', NULL, NULL, 1, 8, 19),   -- Anna - Week in Suite
('2026-06-20', '2026-06-25', NULL, NULL, 1, 1, 20),   -- Henrik
('2026-06-25', '2026-06-30', NULL, NULL, 1, 2, 6);    -- Sophie
GO

-- ============================================================
-- BOOKING test data - July-September 2026 (Peak Summer Season)
-- ============================================================
INSERT INTO BOOKING (StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID) VALUES
-- July 2026 - High season
('2026-07-01', '2026-07-08', NULL, NULL, 1, 3, 7),    -- Michael - Week
('2026-07-05', '2026-07-12', NULL, NULL, 1, 5, 8),    -- Isabella - Week
('2026-07-10', '2026-07-17', NULL, NULL, 1, 8, 9),    -- James - Suite week
('2026-07-15', '2026-07-22', NULL, NULL, 1, 4, 10),   -- Yuki - Week
('2026-07-20', '2026-07-27', NULL, NULL, 1, 7, 11),   -- Marie - Week
('2026-07-25', '2026-08-01', NULL, NULL, 1, 1, 12),   -- Hans - Crossing month

-- August 2026 - Peak season
('2026-08-01', '2026-08-08', NULL, NULL, 1, 2, 13),   -- Olivia - Week
('2026-08-05', '2026-08-12', NULL, NULL, 1, 3, 14),   -- Lucas - Week
('2026-08-10', '2026-08-17', NULL, NULL, 1, 8, 15),   -- Amelia - Suite week
('2026-08-15', '2026-08-22', NULL, NULL, 1, 5, 16),   -- Erik - Week
('2026-08-20', '2026-08-27', NULL, NULL, 1, 4, 17),   -- Nina - Week
('2026-08-25', '2026-09-01', NULL, NULL, 1, 7, 18),   -- Carlos - Crossing to Sept

-- September 2026 - Late summer
('2026-09-01', '2026-09-07', NULL, NULL, 1, 1, 19),   -- Anna - Week
('2026-09-05', '2026-09-10', NULL, NULL, 1, 2, 20),   -- Henrik
('2026-09-10', '2026-09-15', NULL, NULL, 1, 8, 6),    -- Sophie - Suite
('2026-09-15', '2026-09-20', NULL, NULL, 1, 3, 7),    -- Michael
('2026-09-20', '2026-09-25', NULL, NULL, 0, 5, 8),    -- Isabella - Pending
('2026-09-25', '2026-09-30', NULL, NULL, 1, 4, 9);    -- James
GO

-- ============================================================
-- BOOKING test data - October-December 2026 (Autumn/Winter)
-- ============================================================
INSERT INTO BOOKING (StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID) VALUES
-- October 2026 - Autumn
('2026-10-01', '2026-10-05', NULL, NULL, 1, 7, 10),   -- Yuki
('2026-10-05', '2026-10-10', NULL, NULL, 1, 1, 11),   -- Marie
('2026-10-10', '2026-10-15', NULL, NULL, 1, 2, 12),   -- Hans
('2026-10-15', '2026-10-20', NULL, NULL, 4, 3, 13),   -- Olivia - CANCELLED
('2026-10-20', '2026-10-25', NULL, NULL, 1, 8, 14),   -- Lucas - Suite
('2026-10-25', '2026-10-30', NULL, NULL, 1, 5, 15),   -- Amelia

-- November 2026
('2026-11-01', '2026-11-05', NULL, NULL, 1, 4, 16),   -- Erik
('2026-11-05', '2026-11-10', NULL, NULL, 1, 7, 17),   -- Nina
('2026-11-10', '2026-11-15', NULL, NULL, 1, 1, 18),   -- Carlos
('2026-11-15', '2026-11-20', NULL, NULL, 0, 2, 19),   -- Anna - Pending
('2026-11-20', '2026-11-25', NULL, NULL, 1, 8, 20),   -- Henrik - Suite
('2026-11-25', '2026-11-30', NULL, NULL, 1, 3, 6),    -- Sophie

-- December 2026 - Christmas season
('2026-12-01', '2026-12-05', NULL, NULL, 1, 5, 7),    -- Michael
('2026-12-05', '2026-12-10', NULL, NULL, 1, 4, 8),    -- Isabella
('2026-12-10', '2026-12-15', NULL, NULL, 1, 7, 9),    -- James
('2026-12-15', '2026-12-20', NULL, NULL, 1, 1, 10),   -- Yuki
('2026-12-20', '2026-12-28', NULL, NULL, 1, 8, 11),   -- Marie - Christmas Suite
('2026-12-22', '2026-12-29', NULL, NULL, 1, 3, 12),   -- Hans - Christmas
('2026-12-24', '2026-12-30', NULL, NULL, 1, 2, 13),   -- Olivia - Christmas
('2026-12-27', '2027-01-03', NULL, NULL, 1, 5, 14),   -- Lucas - New Year
('2026-12-29', '2027-01-05', NULL, NULL, 1, 4, 15);   -- Amelia - New Year
GO

-- ============================================================
-- BOOKING test data - 2027 (Selected months)
-- ============================================================
INSERT INTO BOOKING (StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID) VALUES
-- January 2027
('2027-01-05', '2027-01-10', NULL, NULL, 1, 7, 16),   -- Erik
('2027-01-15', '2027-01-20', NULL, NULL, 1, 1, 17),   -- Nina
('2027-01-25', '2027-01-30', NULL, NULL, 1, 8, 18),   -- Carlos - Suite

-- March 2027
('2027-03-01', '2027-03-07', NULL, NULL, 1, 2, 19),   -- Anna - Week
('2027-03-15', '2027-03-22', NULL, NULL, 1, 3, 20),   -- Henrik - Week
('2027-03-25', '2027-03-30', NULL, NULL, 1, 8, 6),    -- Sophie - Suite

-- June 2027 - Summer start
('2027-06-01', '2027-06-08', NULL, NULL, 1, 5, 7),    -- Michael - Week
('2027-06-10', '2027-06-17', NULL, NULL, 1, 4, 8),    -- Isabella - Week
('2027-06-20', '2027-06-27', NULL, NULL, 1, 8, 9),    -- James - Suite week

-- September 2027
('2027-09-01', '2027-09-07', NULL, NULL, 1, 7, 10),   -- Yuki - Week
('2027-09-15', '2027-09-20', NULL, NULL, 1, 1, 11),   -- Marie
('2027-09-25', '2027-09-30', NULL, NULL, 0, 3, 12),   -- Hans - Pending

-- December 2027 - Christmas
('2027-12-20', '2027-12-27', NULL, NULL, 1, 8, 13),   -- Olivia - Christmas Suite
('2027-12-22', '2027-12-29', NULL, NULL, 1, 2, 14),   -- Lucas - Christmas
('2027-12-27', '2028-01-03', NULL, NULL, 1, 3, 15);   -- Amelia - New Year
GO

PRINT 'Extended test data inserted successfully!';
PRINT 'Additional Guests: 15 (Total: 20)';
PRINT 'Additional Bookings: 99';
PRINT 'Date Range: December 2025 - December 2027';
PRINT 'Booking Status Distribution:';
PRINT '  - Confirmed: ~90%';
PRINT '  - Pending: ~8%';
PRINT '  - Cancelled: ~2%';
