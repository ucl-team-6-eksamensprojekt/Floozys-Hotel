using System;
using System.Linq;
using Floozys_Hotel.Database;
using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories;
using Floozys_Hotel.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FloozyHotelTests.ViewModelTests
{
    [TestClass]
    public class NewBookingViewModelTests
    {
        [TestMethod]
        public void Constructor_LoadsAvailableRooms()
        {
            // Arrange & Act
            var viewModel = new NewBookingViewModel();

            // Assert
            Assert.IsNotNull(viewModel.NewBookingRoomList);
            Assert.IsTrue(viewModel.NewBookingRoomList.Count > 0);
        }

        [TestMethod]
        public void CreateBooking_WithValidData_CreatesBooking()
        {
            // Arrange
            var viewModel = new NewBookingViewModel();
            viewModel.CheckInDate = DateTime.Today.AddDays(1);
            viewModel.CheckOutDate = DateTime.Today.AddDays(4);
            viewModel.FirstName = "John";
            viewModel.LastName = "Doe";
            viewModel.Email = "john@example.com";
            viewModel.PhoneNumber = "+4512345678";
            viewModel.Country = "Denmark";
            viewModel.PassportNumber = "DK123456";
            viewModel.SelectedRoom = viewModel.NewBookingRoomList.First();

            // Act
            viewModel.ConfirmBookingCommand.Execute(null);

            // Assert - Just check success message, not repo count
            Assert.IsTrue(viewModel.ErrorMessage.Contains("✅"));
            Assert.IsTrue(viewModel.ErrorMessage.Contains("Booking #"));
        }

        [TestMethod]
        public void CreateBooking_WithoutCheckInDate_ShowsError()
        {
            // Arrange
            var viewModel = new NewBookingViewModel();
            viewModel.CheckOutDate = DateTime.Today.AddDays(4);
            viewModel.FirstName = "John";
            viewModel.LastName = "Doe";
            viewModel.Email = "john@example.com";
            viewModel.PhoneNumber = "+4512345678";
            viewModel.Country = "Denmark";
            viewModel.PassportNumber = "DK123456";
            viewModel.SelectedRoom = viewModel.NewBookingRoomList.First();

            // Act
            viewModel.ConfirmBookingCommand.Execute(null);

            // Assert
            Assert.AreEqual("Check-in date is required", viewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateBooking_WithoutCheckOutDate_ShowsError()
        {
            // Arrange
            var viewModel = new NewBookingViewModel();
            viewModel.CheckInDate = DateTime.Today.AddDays(1);
            viewModel.FirstName = "John";
            viewModel.LastName = "Doe";
            viewModel.Email = "john@example.com";
            viewModel.PhoneNumber = "+4512345678";
            viewModel.Country = "Denmark";
            viewModel.PassportNumber = "DK123456";
            viewModel.SelectedRoom = viewModel.NewBookingRoomList.First();

            // Act
            viewModel.ConfirmBookingCommand.Execute(null);

            // Assert
            Assert.AreEqual("Check-out date is required", viewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateBooking_WithCheckOutBeforeCheckIn_ShowsError()
        {
            // Arrange
            var viewModel = new NewBookingViewModel();
            viewModel.CheckInDate = DateTime.Today.AddDays(5);
            viewModel.CheckOutDate = DateTime.Today.AddDays(2);
            viewModel.FirstName = "John";
            viewModel.LastName = "Doe";
            viewModel.Email = "john@example.com";
            viewModel.PhoneNumber = "+4512345678";
            viewModel.Country = "Denmark";
            viewModel.PassportNumber = "DK123456";
            viewModel.SelectedRoom = viewModel.NewBookingRoomList.First();

            // Act
            viewModel.ConfirmBookingCommand.Execute(null);

            // Assert
            Assert.AreEqual("Check-out date must be after check-in date", viewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateBooking_WithCheckInInPast_ShowsError()
        {
            // Arrange
            var viewModel = new NewBookingViewModel();
            viewModel.CheckInDate = DateTime.Today.AddDays(-2);
            viewModel.CheckOutDate = DateTime.Today.AddDays(2);
            viewModel.FirstName = "John";
            viewModel.LastName = "Doe";
            viewModel.Email = "john@example.com";
            viewModel.PhoneNumber = "+4512345678";
            viewModel.Country = "Denmark";
            viewModel.PassportNumber = "DK123456";
            viewModel.SelectedRoom = viewModel.NewBookingRoomList.First();

            // Act
            viewModel.ConfirmBookingCommand.Execute(null);

            // Assert
            Assert.AreEqual("Check-in date cannot be in the past", viewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateBooking_WithoutRoom_ShowsError()
        {
            // Arrange
            var viewModel = new NewBookingViewModel();
            viewModel.CheckInDate = DateTime.Today.AddDays(1);
            viewModel.CheckOutDate = DateTime.Today.AddDays(4);
            viewModel.FirstName = "John";
            viewModel.LastName = "Doe";
            viewModel.Email = "john@example.com";
            viewModel.PhoneNumber = "+4512345678";
            viewModel.Country = "Denmark";
            viewModel.PassportNumber = "DK123456";
            // No room selected

            // Act
            viewModel.ConfirmBookingCommand.Execute(null);

            // Assert
            Assert.AreEqual("Please select a room", viewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateBooking_WithInvalidGuestEmail_ShowsError()
        {
            // Arrange
            var viewModel = new NewBookingViewModel();
            viewModel.CheckInDate = DateTime.Today.AddDays(1);
            viewModel.CheckOutDate = DateTime.Today.AddDays(4);
            viewModel.FirstName = "John";
            viewModel.LastName = "Doe";
            viewModel.Email = "invalid-email";  // Invalid email
            viewModel.PhoneNumber = "+4512345678";
            viewModel.Country = "Denmark";
            viewModel.PassportNumber = "DK123456";
            viewModel.SelectedRoom = viewModel.NewBookingRoomList.First();

            // Act
            viewModel.ConfirmBookingCommand.Execute(null);

            // Assert
            Assert.IsTrue(viewModel.ErrorMessage.Contains("email") || viewModel.ErrorMessage.Contains("Email"));
        }

        [TestMethod]
        public void ClearForm_ResetsAllFields()
        {
            // Arrange
            var viewModel = new NewBookingViewModel();
            viewModel.CheckInDate = DateTime.Today.AddDays(1);
            viewModel.CheckOutDate = DateTime.Today.AddDays(4);
            viewModel.FirstName = "John";
            viewModel.LastName = "Doe";
            viewModel.Email = "john@example.com";
            viewModel.PhoneNumber = "+4512345678";
            viewModel.Country = "Denmark";
            viewModel.PassportNumber = "DK123456";
            viewModel.SelectedRoom = viewModel.NewBookingRoomList.First();

            // Act - Create booking which calls ClearForm on success
            viewModel.ConfirmBookingCommand.Execute(null);

            // Assert - Fields should be cleared after successful booking
            Assert.IsNull(viewModel.CheckInDate);
            Assert.IsNull(viewModel.CheckOutDate);
            Assert.AreEqual(string.Empty, viewModel.FirstName);
            Assert.AreEqual(string.Empty, viewModel.LastName);
            Assert.AreEqual(string.Empty, viewModel.Email);
            Assert.IsNull(viewModel.SelectedRoom);
        }
    }
}