using System;
using System.Linq;
using Floozys_Hotel.Database;
using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FloozyHotelTests.RepositoryTests
{
    [TestClass]
    public class BookingRepoTests
    {
        public BookingRepoTests()
        {
            var config = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();

            DatabaseConfig.ConnectionString = config.GetConnectionString("DefaultConnection");
        }

        [TestMethod]
        public void Create_AddsBookingToRepository()
        {
            // Arrange
            var repo = new BookingRepo();
            var initialCount = repo.GetAll().Count;

            var booking = new Booking
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3),
                Status = BookingStatus.Pending,
                Room = new Room { RoomId = 1, RoomNumber = "101" },
                Guest = new Guest(
                    firstName: "John",
                    lastName: "Doe",
                    email: "john@example.com",
                    phoneNumber: "+4512345678",
                    country: "Denmark",
                    passportNumber: "DK123456"
                )
            };

            // Act
            repo.Create(booking);

            // Assert
            Assert.AreEqual(initialCount + 1, repo.GetAll().Count);
            Assert.IsTrue(booking.BookingID > 0);
        }

        [TestMethod]
        public void Create_AssignsBookingID()
        {
            // Arrange
            var repo = new BookingRepo();
            var booking = new Booking
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3),
                Status = BookingStatus.Pending,
                Room = new Room { RoomId = 1, RoomNumber = "101" },
                Guest = new Guest(
                    firstName: "John",
                    lastName: "Doe",
                    email: "john@example.com",
                    phoneNumber: "+4512345678",
                    country: "Denmark",
                    passportNumber: "DK123456"
                )
            };

            // Act
            repo.Create(booking);

            // Assert
            Assert.IsTrue(booking.BookingID > 0);
        }

        [TestMethod]
        public void Create_ThrowsExceptionWhenBookingIsNull()
        {
            // Arrange
            var repo = new BookingRepo();
            bool exceptionThrown = false;

            // Act
            try
            {
                repo.Create(null);
            }
            catch (ArgumentNullException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown, "Expected ArgumentNullException was not thrown");
        }

        [TestMethod]
        public void GetAll_ReturnsAllBookings()
        {
            // Arrange
            var repo = new BookingRepo();

            // Act
            var bookings = repo.GetAll();

            // Assert
            Assert.IsTrue(bookings.Count > 0);
        }

        [TestMethod]
        public void GetById_ReturnsCorrectBooking()
        {
            // Arrange
            var repo = new BookingRepo();
            var allBookings = repo.GetAll();
            var firstBooking = allBookings.First();

            // Act
            var result = repo.GetById(firstBooking.BookingID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(firstBooking.BookingID, result.BookingID);
        }

        [TestMethod]
        public void GetById_ReturnsNullWhenNotFound()
        {
            // Arrange
            var repo = new BookingRepo();

            // Act
            var result = repo.GetById(99999);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetByStatus_ReturnsOnlyBookingsWithStatus()
        {
            // Arrange
            var repo = new BookingRepo();

            // Act
            var pendingBookings = repo.GetByStatus(BookingStatus.Pending);

            // Assert
            Assert.IsTrue(pendingBookings.All(b => b.Status == BookingStatus.Pending));
        }

        [TestMethod]
        public void GetByRoomID_ReturnsOnlyBookingsForRoom()
        {
            // Arrange
            var repo = new BookingRepo();
            int roomId = 1;

            // Act
            var bookings = repo.GetByRoomID(roomId);

            // Assert
            Assert.IsTrue(bookings.All(b => b.Room != null && b.Room.RoomId == roomId));
        }

        [TestMethod]
        public void GetByGuestID_ReturnsOnlyBookingsForGuest()
        {
            // Arrange
            var repo = new BookingRepo();
            int guestId = 1;

            // Act
            var bookings = repo.GetByGuestID(guestId);

            // Assert
            Assert.IsTrue(bookings.All(b => b.Guest != null && b.Guest.GuestID == guestId));
        }

        [TestMethod]
        public void Update_ModifiesExistingBooking()
        {
            // Arrange
            var repo = new BookingRepo();
            var booking = repo.GetAll().First();
            var originalEndDate = booking.EndDate;

            // Add 100 days to ensure it's different from any existing date
            booking.EndDate = originalEndDate.AddDays(100);

            // Act
            repo.Update(booking);

            // Assert
            var updated = repo.GetById(booking.BookingID);
            Assert.AreNotEqual(originalEndDate, updated.EndDate);
            Assert.AreEqual(originalEndDate.AddDays(100), updated.EndDate);
        }

        [TestMethod]
        public void Update_ThrowsExceptionWhenBookingIsNull()
        {
            // Arrange
            var repo = new BookingRepo();
            bool exceptionThrown = false;

            // Act
            try
            {
                repo.Update(null);
            }
            catch (ArgumentNullException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown, "Expected ArgumentNullException was not thrown");
        }

        [TestMethod]
        public void Update_ThrowsExceptionWhenBookingNotFound()
        {
            // Arrange
            var repo = new BookingRepo();
            var booking = new Booking
            {
                BookingID = 99999,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3)
            };
            bool exceptionThrown = false;

            // Act
            try
            {
                repo.Update(booking);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown, "Expected ArgumentException was not thrown");
        }

        [TestMethod]
        public void Delete_RemovesBookingFromRepository()
        {
            // Arrange
            var repo = new BookingRepo();
            var booking = new Booking
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3),
                Status = BookingStatus.Pending,
                Room = new Room { RoomId = 1, RoomNumber = "101" },
                Guest = new Guest(
                    firstName: "Test",
                    lastName: "User",
                    email: "test@example.com",
                    phoneNumber: "+4512345678",
                    country: "Denmark",
                    passportNumber: "DK123456"
                )
            };
            repo.Create(booking);
            var countBefore = repo.GetAll().Count;

            // Act
            repo.Delete(booking.BookingID);

            // Assert
            Assert.AreEqual(countBefore - 1, repo.GetAll().Count);
            Assert.IsNull(repo.GetById(booking.BookingID));
        }

        [TestMethod]
        public void Delete_ThrowsExceptionWhenBookingNotFound()
        {
            // Arrange
            var repo = new BookingRepo();
            bool exceptionThrown = false;

            // Act
            try
            {
                repo.Delete(99999);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown, "Expected ArgumentException was not thrown");
        }
    }
}