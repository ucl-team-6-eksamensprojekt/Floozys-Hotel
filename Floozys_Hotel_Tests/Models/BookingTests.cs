using Microsoft.VisualStudio.TestTools.UnitTesting;
using Floozys_Hotel.Models;
using System;
using System.Linq;

namespace Floozys_Hotel_Tests.Models
{
    /// <summary>
    /// Tests for Booking model validation logic
    /// FIRST Principles: Fast, Independent, Repeatable, Self-validating, Timely
    /// </summary>
    [TestClass]
    public class BookingTests
    {
        // ============================================
        // VALIDATION TESTS
        // ============================================

        [TestMethod]
        public void Validate_ValidBooking_ReturnsNoErrors()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3),
                RoomID = 1,
                GuestID = 1,
                Room = new Room { RoomId = 1 },
                Guest = new Guest { GuestID = 1 }
            };

            // Act
            var errors = booking.Validate();

            // Assert
            Assert.AreEqual(0, errors.Count, "Valid booking should have no errors");
        }

        [TestMethod]
        public void Validate_MissingStartDate_ReturnsError()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = default,
                EndDate = DateTime.Today.AddDays(3),
                RoomID = 1,
                GuestID = 1
            };

            // Act
            var errors = booking.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("Start date is required")));
        }

        [TestMethod]
        public void Validate_MissingEndDate_ReturnsError()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = default,
                RoomID = 1,
                GuestID = 1
            };

            // Act
            var errors = booking.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("End date is required")));
        }

        [TestMethod]
        public void Validate_EndDateBeforeStartDate_ReturnsError()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(5),
                EndDate = DateTime.Today.AddDays(3),
                RoomID = 1,
                GuestID = 1
            };

            // Act
            var errors = booking.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("End date must be after start date")));
        }

        [TestMethod]
        public void Validate_StartDateInPast_ReturnsError()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(-1),
                EndDate = DateTime.Today.AddDays(2),
                RoomID = 1,
                GuestID = 1
            };

            // Act
            var errors = booking.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("Start date cannot be in the past")));
        }

        [TestMethod]
        public void Validate_MissingRoom_ReturnsError()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3),
                Room = null,
                RoomID = 0,
                GuestID = 1
            };

            // Act
            var errors = booking.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("Room is required")));
        }

        [TestMethod]
        public void Validate_MissingGuest_ReturnsError()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3),
                RoomID = 1,
                Guest = null,
                GuestID = 0
            };

            // Act
            var errors = booking.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("Guest is required")));
        }

        [TestMethod]
        public void Validate_CheckOutBeforeCheckIn_ReturnsError()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3),
                CheckInTime = DateTime.Now,
                CheckOutTime = DateTime.Now.AddHours(-1),
                RoomID = 1,
                GuestID = 1,
                Room = new Room { RoomId = 1 },
                Guest = new Guest { GuestID = 1 }
            };

            // Act
            var errors = booking.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("Check-out time must be after check-in time")));
        }

        // ============================================
        // VALIDATE EDIT TESTS
        // ============================================

        [TestMethod]
        public void ValidateEdit_ValidDates_ReturnsNoErrors()
        {
            // Arrange
            var booking = new Booking();
            var newStartDate = DateTime.Today.AddDays(1);
            var newEndDate = DateTime.Today.AddDays(3);

            // Act
            var errors = booking.ValidateEdit(newStartDate, newEndDate);

            // Assert
            Assert.AreEqual(0, errors.Count, "Valid edit should have no errors");
        }

        [TestMethod]
        public void ValidateEdit_EndDateBeforeStartDate_ReturnsError()
        {
            // Arrange
            var booking = new Booking();
            var newStartDate = DateTime.Today.AddDays(5);
            var newEndDate = DateTime.Today.AddDays(3);

            // Act
            var errors = booking.ValidateEdit(newStartDate, newEndDate);

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("End date must be after start date")));
        }

        [TestMethod]
        public void ValidateEdit_StartDateInPast_ReturnsError()
        {
            // Arrange
            var booking = new Booking();
            var newStartDate = DateTime.Today.AddDays(-1);
            var newEndDate = DateTime.Today.AddDays(2);

            // Act
            var errors = booking.ValidateEdit(newStartDate, newEndDate);

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("Start date cannot be in the past")));
        }

        // ============================================
        // CALCULATED PROPERTIES TESTS
        // ============================================

        [TestMethod]
        public void NumberOfNights_TwoDayBooking_ReturnsOne()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = new DateTime(2024, 12, 17),
                EndDate = new DateTime(2024, 12, 18)
            };

            // Act
            var nights = booking.NumberOfNights;

            // Assert
            Assert.AreEqual(1, nights, "One night between Dec 17 and Dec 18");
        }

        [TestMethod]
        public void NumberOfNights_ThreeDayBooking_ReturnsTwo()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = new DateTime(2024, 12, 17),
                EndDate = new DateTime(2024, 12, 19)
            };

            // Act
            var nights = booking.NumberOfNights;

            // Assert
            Assert.AreEqual(2, nights, "Two nights between Dec 17 and Dec 19");
        }

        [TestMethod]
        public void NumberOfNights_DefaultDates_ReturnsZero()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = default,
                EndDate = default
            };

            // Act
            var nights = booking.NumberOfNights;

            // Assert
            Assert.AreEqual(0, nights, "Zero nights for default dates");
        }

        [TestMethod]
        public void BookingNumber_FormatsCorrectly()
        {
            // Arrange
            var booking = new Booking { BookingID = 123 };

            // Act
            var bookingNumber = booking.BookingNumber;

            // Assert
            Assert.AreEqual("FLZ-000123", bookingNumber, "Should format with leading zeros");
        }

        [TestMethod]
        public void BookingNumber_LargeID_FormatsCorrectly()
        {
            // Arrange
            var booking = new Booking { BookingID = 999999 };

            // Act
            var bookingNumber = booking.BookingNumber;

            // Assert
            Assert.AreEqual("FLZ-999999", bookingNumber, "Should format large IDs correctly");
        }
    }
}