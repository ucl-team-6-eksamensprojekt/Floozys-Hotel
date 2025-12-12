using System;
using Floozys_Hotel.Database;
using Floozys_Hotel.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FloozyHotelTests.ModelTests
{
    [TestClass]
    public class BookingTests
    {
        public BookingTests() 
        {
            var config = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();

            DatabaseConfig.ConnectionString = config.GetConnectionString("DefaultConnection");
        }

        [TestMethod]
        public void Validate_WhenStartDateIsDefault_ReturnsError()
        {
            // Arrange
            var booking = new Booking
            {
                EndDate = DateTime.Today.AddDays(3),
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
            var errors = booking.Validate();

            // Assert
            Assert.IsTrue(errors.Contains("Start date is required"));
        }

        [TestMethod]
        public void Validate_WhenEndDateIsDefault_ReturnsError()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today,
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
            var errors = booking.Validate();

            // Assert
            Assert.IsTrue(errors.Contains("End date is required"));
        }

        [TestMethod]
        public void Validate_WhenEndDateBeforeStartDate_ReturnsError()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(5),
                EndDate = DateTime.Today.AddDays(3),
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
            var errors = booking.Validate();

            // Assert
            Assert.IsTrue(errors.Contains("End date must be after start date"));
        }

        [TestMethod]
        public void Validate_WhenStartDateInPast_ReturnsError()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(-5),
                EndDate = DateTime.Today.AddDays(-3),
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
            var errors = booking.Validate();

            // Assert
            Assert.IsTrue(errors.Contains("Start date cannot be in the past"));
        }

        [TestMethod]
        public void Validate_WhenRoomIsNull_ReturnsError()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3),
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
            var errors = booking.Validate();

            // Assert
            Assert.IsTrue(errors.Contains("Room is required"));
        }

        [TestMethod]
        public void Validate_WhenGuestIsNull_ReturnsError()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3),
                Room = new Room { RoomId = 1, RoomNumber = "101" }
            };

            // Act
            var errors = booking.Validate();

            // Assert
            Assert.IsTrue(errors.Contains("Guest is required"));
        }

        [TestMethod]
        public void Validate_WhenAllFieldsValid_ReturnsNoErrors()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3),
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
            var errors = booking.Validate();

            // Assert
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void NumberOfNights_CalculatesCorrectly()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(5)
            };

            // Act
            int nights = booking.NumberOfNights;

            // Assert
            Assert.AreEqual(5, nights);
        }

        [TestMethod]
        public void NumberOfNights_WhenDatesAreDefault_ReturnsZero()
        {
            // Arrange
            var booking = new Booking();

            // Act
            int nights = booking.NumberOfNights;

            // Assert
            Assert.AreEqual(0, nights);
        }

        [TestMethod]
        public void Validate_WhenCheckOutTimeBeforeCheckInTime_ReturnsError()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3),
                CheckInTime = DateTime.Now,
                CheckOutTime = DateTime.Now.AddHours(-1),  // Before CheckInTime
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
            var errors = booking.Validate();

            // Assert
            Assert.IsTrue(errors.Contains("Check-out time must be after check-in time"));
        }
    }
}