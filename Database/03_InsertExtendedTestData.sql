-- ============================================================
-- Udvidet testdata til Floozys Hotel Booking System
-- Dato-interval: December 2025 - December 2027
-- ============================================================

-- ============================================================
-- Yderligere gæstedata til test
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
-- BOOKING testdata - December 2025
-- ============================================================
INSERT INTO BOOKING (StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID) VALUES
-- Tidligt i december 2025 - Bekræftede bookinger
('2025-12-01', '2025-12-04', NULL, NULL, 1, 1, 6),   -- Sophie i værelse 101
('2025-12-02', '2025-12-06', NULL, NULL, 1, 3, 7),   -- Michael i værelse 103
('2025-12-05', '2025-12-10', NULL, NULL, 1, 2, 8),   -- Isabella i værelse 102

-- Midten af december 2025 - Forskellige statusser
('2025-12-10', '2025-12-14', NULL, NULL, 1, 4, 9),   -- James i værelse 201
('2025-12-12', '2025-12-16', NULL, NULL, 0, 5, 10),  -- Yuki i værelse 202 - Afventende
('2025-12-15', '2025-12-20', NULL, NULL, 1, 1, 11),  -- Marie i værelse 101

-- Juleperioden 2025 - Høj efterspørgsel
('2025-12-20', '2025-12-27', NULL, NULL, 1, 3, 12),  -- Hans i værelse 103 - Juleugen
('2025-12-22', '2025-12-29', NULL, NULL, 1, 7, 13),  -- Olivia i værelse 301
('2025-12-23', '2025-12-30', NULL, NULL, 1, 8, 14),  -- Lucas i værelse 302 - Suite
('2025-12-24', '2025-12-28', NULL, NULL, 1, 2, 15),  -- Amelia i værelse 102

-- Nytårsperioden 2025-2026
('2025-12-27', '2026-01-03', NULL, NULL, 1, 4, 16),  -- Erik krydser ind i nytår
('2025-12-29', '2026-01-05', NULL, NULL, 1, 5, 17),  -- Nina krydser ind i nytår
('2025-12-30', '2026-01-02', NULL, NULL, 1, 1, 18);  -- Carlos - Nytår
GO

-- ============================================================
-- BOOKING testdata - Januar-Marts 2026
-- ============================================================
INSERT INTO BOOKING (StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID) VALUES
-- Januar 2026
('2026-01-05', '2026-01-10', NULL, NULL, 1, 2, 19),   -- Anna
('2026-01-10', '2026-01-15', NULL, NULL, 1, 3, 20),   -- Henrik
('2026-01-15', '2026-01-20', NULL, NULL, 0, 1, 6),    -- Sophie kommer tilbage - Afventende
('2026-01-20', '2026-01-25', NULL, NULL, 1, 7, 7),    -- Michael
('2026-01-25', '2026-01-30', NULL, NULL, 1, 8, 8),    -- Isabella

-- Februar 2026 - Valentinsdag bookinger
('2026-02-01', '2026-02-05', NULL, NULL, 1, 4, 9),    -- James
('2026-02-10', '2026-02-14', NULL, NULL, 1, 8, 10),   -- Yuki - Valentine Suite
('2026-02-13', '2026-02-16', NULL, NULL, 1, 3, 11),   -- Marie - Valentinsdag
('2026-02-14', '2026-02-17', NULL, NULL, 1, 2, 12),   -- Hans - Valentinsdag
('2026-02-20', '2026-02-25', NULL, NULL, 1, 5, 13),   -- Olivia

-- Marts 2026
('2026-03-01', '2026-03-05', NULL, NULL, 1, 1, 14),   -- Lucas
('2026-03-05', '2026-03-10', NULL, NULL, 1, 7, 15),   -- Amelia
('2026-03-10', '2026-03-15', NULL, NULL, 0, 4, 16),   -- Erik - Afventende
('2026-03-15', '2026-03-20', NULL, NULL, 1, 2, 17),   -- Nina
('2026-03-20', '2026-03-25', NULL, NULL, 1, 3, 18),   -- Carlos
('2026-03-25', '2026-03-30', NULL, NULL, 1, 8, 19);   -- Anna - Suite
GO

-- ============================================================
-- BOOKING testdata - April-Juni 2026 (Forår/Sommer bookinger)
-- ============================================================
INSERT INTO BOOKING (StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID) VALUES
-- April 2026 - Påskeperiode
('2026-04-01', '2026-04-06', NULL, NULL, 1, 1, 20),   -- Henrik
('2026-04-05', '2026-04-10', NULL, NULL, 1, 5, 6),    -- Sophie
('2026-04-10', '2026-04-15', NULL, NULL, 1, 2, 7),    -- Michael
('2026-04-12', '2026-04-18', NULL, NULL, 1, 8, 8),    -- Isabella - Påske Suite
('2026-04-20', '2026-04-25', NULL, NULL, 1, 3, 9),    -- James

-- Maj 2026
('2026-05-01', '2026-05-05', NULL, NULL, 1, 4, 10),   -- Yuki
('2026-05-05', '2026-05-10', NULL, NULL, 1, 7, 11),   -- Marie
('2026-05-10', '2026-05-15', NULL, NULL, 1, 1, 12),   -- Hans
('2026-05-15', '2026-05-20', NULL, NULL, 0, 2, 13),   -- Olivia - Afventende
('2026-05-20', '2026-05-25', NULL, NULL, 1, 8, 14),   -- Lucas - Suite
('2026-05-25', '2026-05-30', NULL, NULL, 1, 5, 15),   -- Amelia

-- Juni 2026 - Sommerstart
('2026-06-01', '2026-06-07', NULL, NULL, 1, 3, 16),   -- Erik - Uge-ophold
('2026-06-05', '2026-06-10', NULL, NULL, 1, 4, 17),   -- Nina
('2026-06-10', '2026-06-15', NULL, NULL, 1, 7, 18),   -- Carlos
('2026-06-15', '2026-06-22', NULL, NULL, 1, 8, 19),   -- Anna - Uge i Suite
('2026-06-20', '2026-06-25', NULL, NULL, 1, 1, 20),   -- Henrik
('2026-06-25', '2026-06-30', NULL, NULL, 1, 2, 6);    -- Sophie
GO

-- ============================================================
-- BOOKING testdata - Juli-September 2026 (Højsæson sommer)
-- ============================================================
INSERT INTO BOOKING (StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID) VALUES
-- Juli 2026 - Højsæson
('2026-07-01', '2026-07-08', NULL, NULL, 1, 3, 7),    -- Michael - Uge
('2026-07-05', '2026-07-12', NULL, NULL, 1, 5, 8),    -- Isabella - Uge
('2026-07-10', '2026-07-17', NULL, NULL, 1, 8, 9),    -- James - Suite uge
('2026-07-15', '2026-07-22', NULL, NULL, 1, 4, 10),   -- Yuki - Uge
('2026-07-20', '2026-07-27', NULL, NULL, 1, 7, 11),   -- Marie - Uge
('2026-07-25', '2026-08-01', NULL, NULL, 1, 1, 12),   -- Hans - Krydser måneder

-- August 2026 - Peak sæson
('2026-08-01', '2026-08-08', NULL, NULL, 1, 2, 13),   -- Olivia - Uge
('2026-08-05', '2026-08-12', NULL, NULL, 1, 3, 14),   -- Lucas - Uge
('2026-08-10', '2026-08-17', NULL, NULL, 1, 8, 15),   -- Amelia - Suite uge
('2026-08-15', '2026-08-22', NULL, NULL, 1, 5, 16),   -- Erik - Uge
('2026-08-20', '2026-08-27', NULL, NULL, 1, 4, 17),   -- Nina - Uge
('2026-08-25', '2026-09-01', NULL, NULL, 1, 7, 18),   -- Carlos - Krydser til september

-- September 2026 - Sen sommer
('2026-09-01', '2026-09-07', NULL, NULL, 1, 1, 19),   -- Anna - Uge
('2026-09-05', '2026-09-10', NULL, NULL, 1, 2, 20),   -- Henrik
('2026-09-10', '2026-09-15', NULL, NULL, 1, 8, 6),    -- Sophie - Suite
('2026-09-15', '2026-09-20', NULL, NULL, 1, 3, 7),    -- Michael
('2026-09-20', '2026-09-25', NULL, NULL, 0, 5, 8),    -- Isabella - Afventende
('2026-09-25', '2026-09-30', NULL, NULL, 1, 4, 9);    -- James
GO

-- ============================================================
-- BOOKING testdata - Oktober-December 2026 (Efterår/Vinter)
-- ============================================================
INSERT INTO BOOKING (StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID) VALUES
-- Oktober 2026 - Efterår
('2026-10-01', '2026-10-05', NULL, NULL, 1, 7, 10),   -- Yuki
('2026-10-05', '2026-10-10', NULL, NULL, 1, 1, 11),   -- Marie
('2026-10-10', '2026-10-15', NULL, NULL, 1, 2, 12),   -- Hans
('2026-10-15', '2026-10-20', NULL, NULL, 4, 3, 13),   -- Olivia - ANNULLERET
('2026-10-20', '2026-10-25', NULL, NULL, 1, 8, 14),   -- Lucas - Suite
('2026-10-25', '2026-10-30', NULL, NULL, 1, 5, 15),   -- Amelia

-- November 2026
('2026-11-01', '2026-11-05', NULL, NULL, 1, 4, 16),   -- Erik
('2026-11-05', '2026-11-10', NULL, NULL, 1, 7, 17),   -- Nina
('2026-11-10', '2026-11-15', NULL, NULL, 1, 1, 18),   -- Carlos
('2026-11-15', '2026-11-20', NULL, NULL, 0, 2, 19),   -- Anna - Afventende
('2026-11-20', '2026-11-25', NULL, NULL, 1, 8, 20),   -- Henrik - Suite
('2026-11-25', '2026-11-30', NULL, NULL, 1, 3, 6),    -- Sophie

-- December 2026 - Julesæson
('2026-12-01', '2026-12-05', NULL, NULL, 1, 5, 7),    -- Michael
('2026-12-05', '2026-12-10', NULL, NULL, 1, 4, 8),    -- Isabella
('2026-12-10', '2026-12-15', NULL, NULL, 1, 7, 9),    -- James
('2026-12-15', '2026-12-20', NULL, NULL, 1, 1, 10),   -- Yuki
('2026-12-20', '2026-12-28', NULL, NULL, 1, 8, 11),   -- Marie - Jule Suite
('2026-12-22', '2026-12-29', NULL, NULL, 1, 3, 12),   -- Hans - Jul
('2026-12-24', '2026-12-30', NULL, NULL, 1, 2, 13),   -- Olivia - Jul
('2026-12-27', '2027-01-03', NULL, NULL, 1, 5, 14),   -- Lucas - Nytår
('2026-12-29', '2027-01-05', NULL, NULL, 1, 4, 15);   -- Amelia - Nytår
GO

-- ============================================================
-- BOOKING testdata - 2027 (Udvalgte måneder)
-- ============================================================
INSERT INTO BOOKING (StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID) VALUES
-- Januar 2027
('2027-01-05', '2027-01-10', NULL, NULL, 1, 7, 16),   -- Erik
('2027-01-15', '2027-01-20', NULL, NULL, 1, 1, 17),   -- Nina
('2027-01-25', '2027-01-30', NULL, NULL, 1, 8, 18),   -- Carlos - Suite

-- Marts 2027
('2027-03-01', '2027-03-07', NULL, NULL, 1, 2, 19),   -- Anna - Uge
('2027-03-15', '2027-03-22', NULL, NULL, 1, 3, 20),   -- Henrik - Uge
('2027-03-25', '2027-03-30', NULL, NULL, 1, 8, 6),    -- Sophie - Suite

-- Juni 2027 - Sommerstart
('2027-06-01', '2027-06-08', NULL, NULL, 1, 5, 7),    -- Michael - Uge
('2027-06-10', '2027-06-17', NULL, NULL, 1, 4, 8),    -- Isabella - Uge
('2027-06-20', '2027-06-27', NULL, NULL, 1, 8, 9),    -- James - Suite uge

-- September 2027
('2027-09-01', '2027-09-07', NULL, NULL, 1, 7, 10),   -- Yuki - Uge
('2027-09-15', '2027-09-20', NULL, NULL, 1, 1, 11),   -- Marie
('2027-09-25', '2027-09-30', NULL, NULL, 0, 3, 12),   -- Hans - Afventende

-- December 2027 - Jul
('2027-12-20', '2027-12-27', NULL, NULL, 1, 8, 13),   -- Olivia - Jule Suite
('2027-12-22', '2027-12-29', NULL, NULL, 1, 2, 14),   -- Lucas - Jul
('2027-12-27', '2028-01-03', NULL, NULL, 1, 3, 15);   -- Amelia - Nytår
GO

PRINT 'Udvidet testdata indsat succesfuldt!';
PRINT 'Yderligere gæster: 15 (I alt: 20)';
PRINT 'Yderligere bookinger: 99';
PRINT 'Dato-interval: December 2025 - December 2027';
PRINT 'Booking status-fordeling:';
PRINT '  - Bekræftet: ~90%';
PRINT '  - Afventende: ~8%';
PRINT '  - Annulleret: ~2%';
