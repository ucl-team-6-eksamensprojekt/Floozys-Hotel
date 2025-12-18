using Microsoft.VisualStudio.TestTools.UnitTesting;
using Floozys_Hotel.Repositories;
using Floozys_Hotel.Models;
using System;
using System.Linq;

namespace Floozys_Hotel_Tests.Repositories
{
    /// <summary>
    /// Integration tests for BookingRepo - tests CRUD operations against real database
    /// NOTE: These tests require a test database connection
    /// FIRST Principles: Fast, Independent, Repeatable, Self-validating, Timely
    /// </summary>
    [TestClass]
    public class BookingRepoTests
    {
        private BookingRepo _bookingRepo;
        private GuestRepo _guestRepo;
        private RoomRepo _roomRepo;
        private int _testGuestId;
        private int _testRoomId;

        [TestInitialize]
        public void Setup()
        {
            // Arrange - Create repositories and test data
            _bookingRepo = new BookingRepo();
            _guestRepo = new GuestRepo();
            _roomRepo = new RoomRepo();

            // Create test guest
            var testGuest = new Guest
            {
                FirstName = "Test",
                LastName = "Guest",
                Email = "test@example.com",
                PhoneNumber = "+45 12345678",
                Country = "Denmark",
                PassportNumber = "TEST123"
            };
            _testGuestId = _guestRepo.AddGuest(testGuest);

            // Create test room
            var testRoom = new Room
            {
                RoomNumber = "999",
                Floor = 9,
                RoomSize = "Test",
                Capacity = 1,
                Status = RoomStatus.Available
            };
            _roomRepo.CreateRoom(testRoom);
            _testRoomId = _roomRepo.GetAll().First(r => r.RoomNumber == "999").RoomId;
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Clean up test data
            try
            {
                var allBookings = _bookingRepo.GetAll();
                var testBookings = allBookings.Where(b => b.GuestID == _testGuestId || b.RoomID == _testRoomId);
                foreach (var booking in testBookings)
                {
                    _bookingRepo.Delete(booking.BookingID);
                }

                _guestRepo.DeleteGuest(_testGuestId);
                _roomRepo.DeleteRoom(_testRoomId);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        // ============================================
        // CREATE TESTS
        // ============================================

        [TestMethod]
        public void Create_ValidBooking_InsertsIntoDatabase()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3),
                Status = BookingStatus.Pending,
                RoomID = _testRoomId,
                GuestID = _testGuestId
            };

            // Act
            _bookingRepo.Create(booking);

            // Assert
            Assert.IsTrue(booking.BookingID > 0, "BookingID should be assigned after insert");

            // Verify in database
            var retrieved = _bookingRepo.GetById(booking.BookingID);
            Assert.IsNotNull(retrieved, "Should retrieve created booking");
            Assert.AreEqual(booking.StartDate, retrieved.StartDate);
            Assert.AreEqual(booking.EndDate, retrieved.EndDate);
            Assert.AreEqual(BookingStatus.Pending, retrieved.Status);
        }

        [TestMethod]
        public void Create_NullBooking_ThrowsException()
        {
            // Arrange
            Booking booking = null;
            bool exceptionThrown = false;

            // Act
            try
            {
                _bookingRepo.Create(booking);
            }
            catch (ArgumentNullException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown, "Should throw ArgumentNullException for null booking");
        }

        // ============================================
        // READ TESTS
        // ============================================

        [TestMethod]
        public void GetAll_ReturnsAllBookings()
        {
            // Arrange
            var booking1 = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(2),
                Status = BookingStatus.Pending,
                RoomID = _testRoomId,
                GuestID = _testGuestId
            };
            var booking2 = new Booking
            {
                StartDate = DateTime.Today.AddDays(3),
                EndDate = DateTime.Today.AddDays(4),
                Status = BookingStatus.Confirmed,
                RoomID = _testRoomId,
                GuestID = _testGuestId
            };

            _bookingRepo.Create(booking1);
            _bookingRepo.Create(booking2);

            // Act
            var allBookings = _bookingRepo.GetAll();

            // Assert
            Assert.IsTrue(allBookings.Count >= 2, "Should retrieve at least the test bookings");
            var testBookings = allBookings.Where(b => b.GuestID == _testGuestId).ToList();
            Assert.AreEqual(2, testBookings.Count, "Should have 2 test bookings");
        }

        [TestMethod]
        public void GetById_ExistingBooking_ReturnsBooking()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3),
                Status = BookingStatus.Pending,
                RoomID = _testRoomId,
                GuestID = _testGuestId
            };
            _bookingRepo.Create(booking);

            // Act
            var retrieved = _bookingRepo.GetById(booking.BookingID);

            // Assert
            Assert.IsNotNull(retrieved, "Should retrieve booking");
            Assert.AreEqual(booking.BookingID, retrieved.BookingID);
            Assert.AreEqual(booking.StartDate, retrieved.StartDate);
            Assert.AreEqual(booking.EndDate, retrieved.EndDate);
        }

        [TestMethod]
        public void GetById_NonExistentBooking_ReturnsNull()
        {
            // Arrange
            int nonExistentId = 999999;

            // Act
            var retrieved = _bookingRepo.GetById(nonExistentId);

            // Assert
            Assert.IsNull(retrieved, "Should return null for non-existent booking");
        }

        [TestMethod]
        public void GetByStatus_FiltersByStatus()
        {
            // Arrange
            var pending = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(2),
                Status = BookingStatus.Pending,
                RoomID = _testRoomId,
                GuestID = _testGuestId
            };
            var confirmed = new Booking
            {
                StartDate = DateTime.Today.AddDays(3),
                EndDate = DateTime.Today.AddDays(4),
                Status = BookingStatus.Confirmed,
                RoomID = _testRoomId,
                GuestID = _testGuestId
            };

            _bookingRepo.Create(pending);
            _bookingRepo.Create(confirmed);

            // Act
            var pendingBookings = _bookingRepo.GetByStatus(BookingStatus.Pending);
            var confirmedBookings = _bookingRepo.GetByStatus(BookingStatus.Confirmed);

            // Assert
            Assert.IsTrue(pendingBookings.Any(b => b.BookingID == pending.BookingID), "Should find pending booking");
            Assert.IsTrue(confirmedBookings.Any(b => b.BookingID == confirmed.BookingID), "Should find confirmed booking");
        }

        [TestMethod]
        public void GetByRoomID_FiltersByRoom()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3),
                Status = BookingStatus.Pending,
                RoomID = _testRoomId,
                GuestID = _testGuestId
            };
            _bookingRepo.Create(booking);

            // Act
            var roomBookings = _bookingRepo.GetByRoomID(_testRoomId);

            // Assert
            Assert.IsTrue(roomBookings.Any(b => b.BookingID == booking.BookingID), "Should find booking for room");
        }

        [TestMethod]
        public void GetByGuestID_FiltersByGuest()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3),
                Status = BookingStatus.Pending,
                RoomID = _testRoomId,
                GuestID = _testGuestId
            };
            _bookingRepo.Create(booking);

            // Act
            var guestBookings = _bookingRepo.GetByGuestID(_testGuestId);

            // Assert
            Assert.IsTrue(guestBookings.Any(b => b.BookingID == booking.BookingID), "Should find booking for guest");
        }

        // ============================================
        // UPDATE TESTS
        // ============================================

        [TestMethod]
        public void Update_ExistingBooking_UpdatesDatabase()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3),
                Status = BookingStatus.Pending,
                RoomID = _testRoomId,
                GuestID = _testGuestId
            };
            _bookingRepo.Create(booking);

            // Act
            booking.Status = BookingStatus.Confirmed;
            booking.CheckInTime = DateTime.Now;
            _bookingRepo.Update(booking);

            // Assert
            var retrieved = _bookingRepo.GetById(booking.BookingID);
            Assert.AreEqual(BookingStatus.Confirmed, retrieved.Status);
            Assert.IsNotNull(retrieved.CheckInTime);
        }

        [TestMethod]
        public void Update_NonExistentBooking_ThrowsException()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 999999,
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3),
                Status = BookingStatus.Pending,
                RoomID = _testRoomId,
                GuestID = _testGuestId
            };
            bool exceptionThrown = false;

            // Act
            try
            {
                _bookingRepo.Update(booking);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown, "Should throw ArgumentException for non-existent booking");
        }

        [TestMethod]
        public void Update_NullBooking_ThrowsException()
        {
            // Arrange
            Booking booking = null;
            bool exceptionThrown = false;

            // Act
            try
            {
                _bookingRepo.Update(booking);
            }
            catch (ArgumentNullException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown, "Should throw ArgumentNullException for null booking");
        }

        // ============================================
        // DELETE TESTS
        // ============================================

  
        [TestMethod]
        public void Delete_NonExistentBooking_ThrowsException()
        {
            // Arrange
            int nonExistentId = 999999;
            bool exceptionThrown = false;

            // Act
            try
            {
                _bookingRepo.Delete(nonExistentId);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown, "Should throw ArgumentException for non-existent booking");
        }

        // ============================================
        // NAVIGATION PROPERTIES TESTS
        // ============================================

        [TestMethod]
        public void GetById_LoadsRoomAndGuestNavigationProperties()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3),
                Status = BookingStatus.Pending,
                RoomID = _testRoomId,
                GuestID = _testGuestId
            };
            _bookingRepo.Create(booking);

            // Act
            var retrieved = _bookingRepo.GetById(booking.BookingID);

            // Assert
            Assert.IsNotNull(retrieved.Room, "Room navigation property should be loaded");
            Assert.IsNotNull(retrieved.Guest, "Guest navigation property should be loaded");
            Assert.AreEqual(_testRoomId, retrieved.Room.RoomId);
            Assert.AreEqual(_testGuestId, retrieved.Guest.GuestID);
        }
    }
}