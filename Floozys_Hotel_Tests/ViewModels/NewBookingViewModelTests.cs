using Microsoft.VisualStudio.TestTools.UnitTesting;
using Floozys_Hotel.ViewModels;
using Floozys_Hotel.Models;
using System;
using System.Linq;

namespace Floozys_Hotel_Tests.ViewModels
{
    /// <summary>
    /// Tests for NewBookingViewModel business logic
    /// Tests room availability filtering and booking creation validation
    /// FIRST Principles: Fast, Independent, Repeatable, Self-validating, Timely
    /// </summary>
    [TestClass]
    public class NewBookingViewModelTests
    {
        private NewBookingViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            // Arrange - Create fresh ViewModel for each test
            _viewModel = new NewBookingViewModel();
        }

        // ============================================
        // PROPERTY TESTS
        // ============================================

        [TestMethod]
        public void CheckInDate_SetValue_UpdatesProperty()
        {
            // Arrange
            var date = DateTime.Today.AddDays(1);

            // Act
            _viewModel.CheckInDate = date;

            // Assert
            Assert.AreEqual(date, _viewModel.CheckInDate);
        }

        [TestMethod]
        public void CheckOutDate_SetValue_UpdatesProperty()
        {
            // Arrange
            var date = DateTime.Today.AddDays(3);

            // Act
            _viewModel.CheckOutDate = date;

            // Assert
            Assert.AreEqual(date, _viewModel.CheckOutDate);
        }

        [TestMethod]
        public void FirstName_SetValue_UpdatesProperty()
        {
            // Arrange
            var name = "John";

            // Act
            _viewModel.FirstName = name;

            // Assert
            Assert.AreEqual(name, _viewModel.FirstName);
        }

        [TestMethod]
        public void LastName_SetValue_UpdatesProperty()
        {
            // Arrange
            var name = "Smith";

            // Act
            _viewModel.LastName = name;

            // Assert
            Assert.AreEqual(name, _viewModel.LastName);
        }

        [TestMethod]
        public void Email_SetValue_UpdatesProperty()
        {
            // Arrange
            var email = "john@example.com";

            // Act
            _viewModel.Email = email;

            // Assert
            Assert.AreEqual(email, _viewModel.Email);
        }

        [TestMethod]
        public void PhoneNumber_SetValue_UpdatesProperty()
        {
            // Arrange
            var phone = "+45 12345678";

            // Act
            _viewModel.PhoneNumber = phone;

            // Assert
            Assert.AreEqual(phone, _viewModel.PhoneNumber);
        }

        [TestMethod]
        public void Country_SetValue_UpdatesProperty()
        {
            // Arrange
            var country = "Denmark";

            // Act
            _viewModel.Country = country;

            // Assert
            Assert.AreEqual(country, _viewModel.Country);
        }

        [TestMethod]
        public void PassportNumber_SetValue_UpdatesProperty()
        {
            // Arrange
            var passport = "DK123456";

            // Act
            _viewModel.PassportNumber = passport;

            // Assert
            Assert.AreEqual(passport, _viewModel.PassportNumber);
        }

        // ============================================
        // ROOM SELECTION TESTS
        // ============================================

        [TestMethod]
        public void SelectedRoom_SetValue_UpdatesProperty()
        {
            // Arrange
            var room = new Room { RoomId = 1, RoomNumber = "101" };

            // Act
            _viewModel.SelectedRoom = room;

            // Assert
            Assert.AreEqual(room, _viewModel.SelectedRoom);
        }

        [TestMethod]
        public void SelectedRoom_InitialValue_IsNull()
        {
            // Arrange & Act
            var selectedRoom = _viewModel.SelectedRoom;

            // Assert
            Assert.IsNull(selectedRoom, "Selected room should initially be null");
        }

        // ============================================
        // ERROR MESSAGE TESTS
        // ============================================

        [TestMethod]
        public void ErrorMessage_InitialValue_IsEmpty()
        {
            // Arrange & Act
            var errorMessage = _viewModel.ErrorMessage;

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Error message should initially be empty");
        }

        [TestMethod]
        public void ErrorMessage_SetValue_UpdatesProperty()
        {
            // Arrange
            var message = "Test error message";

            // Act
            _viewModel.ErrorMessage = message;

            // Assert
            Assert.AreEqual(message, _viewModel.ErrorMessage);
        }

        // ============================================
        // GUEST CONSTRUCTOR TESTS
        // ============================================

        [TestMethod]
        public void Constructor_WithGuest_LoadsGuestData()
        {
            // Arrange
            var guest = new Guest
            {
                GuestID = 1,
                FirstName = "John",
                LastName = "Smith",
                Email = "john@example.com",
                PhoneNumber = "+45 12345678",
                Country = "Denmark",
                PassportNumber = "DK123456"
            };

            // Act
            var viewModel = new NewBookingViewModel(guest);

            // Assert
            Assert.AreEqual("John", viewModel.FirstName);
            Assert.AreEqual("Smith", viewModel.LastName);
            Assert.AreEqual("john@example.com", viewModel.Email);
            Assert.AreEqual("+45 12345678", viewModel.PhoneNumber);
            Assert.AreEqual("Denmark", viewModel.Country);
            Assert.AreEqual("DK123456", viewModel.PassportNumber);
        }

        [TestMethod]
        public void Constructor_WithoutGuest_HasEmptyFields()
        {
            // Arrange & Act
            var viewModel = new NewBookingViewModel();

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(viewModel.FirstName));
            Assert.IsTrue(string.IsNullOrEmpty(viewModel.LastName));
            Assert.IsTrue(string.IsNullOrEmpty(viewModel.Email));
        }

        // ============================================
        // COMMAND TESTS
        // ============================================

        [TestMethod]
        public void SaveBookingCommand_IsNotNull()
        {
            // Arrange & Act
            var command = _viewModel.SaveBookingCommand;

            // Assert
            Assert.IsNotNull(command, "SaveBookingCommand should be initialized");
        }

        // ============================================
        // DATE VALIDATION LOGIC TESTS
        // ============================================

        [TestMethod]
        public void CheckInDate_Null_NoErrorMessage()
        {
            // Arrange
            _viewModel.CheckInDate = null;
            _viewModel.CheckOutDate = null;

            // Act
            var errorMessage = _viewModel.ErrorMessage;

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "No error when dates are null initially");
        }

        [TestMethod]
        public void CheckOutDate_BeforeCheckIn_SetsErrorMessage()
        {
            // Arrange
            _viewModel.CheckInDate = DateTime.Today.AddDays(5);
            _viewModel.CheckOutDate = DateTime.Today.AddDays(3);

            // Act
            // UpdateAvailableRooms is called automatically when dates change
            var errorMessage = _viewModel.ErrorMessage;

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(errorMessage), "Should have error message");
            Assert.IsTrue(errorMessage.Contains("after"), "Error should mention date ordering");
        }

        // ============================================
        // OBSERVABLE COLLECTION TESTS
        // ============================================

        [TestMethod]
        public void NewBookingRoomList_IsInitialized()
        {
            // Arrange & Act
            var roomList = _viewModel.NewBookingRoomList;

            // Assert
            Assert.IsNotNull(roomList, "Room list should be initialized");
        }

        // ============================================
        // INTEGRATION WITH GUEST MODEL
        // ============================================

        [TestMethod]
        public void GuestData_CreatesValidGuest_WhenAllFieldsFilled()
        {
            // Arrange
            _viewModel.FirstName = "John";
            _viewModel.LastName = "Smith";
            _viewModel.Email = "john@example.com";
            _viewModel.PhoneNumber = "+45 12345678";
            _viewModel.Country = "Denmark";
            _viewModel.PassportNumber = "DK123456";

            // Act - Create guest manually to test data
            var guest = new Guest(
                _viewModel.FirstName,
                _viewModel.LastName,
                _viewModel.Email,
                _viewModel.PhoneNumber,
                _viewModel.Country,
                _viewModel.PassportNumber
            );

            var errors = guest.Validate();

            // Assert
            Assert.AreEqual(0, errors.Count, "Guest should be valid with all fields filled");
        }

        [TestMethod]
        public void GuestData_FailsValidation_WhenFieldsMissing()
        {
            // Arrange
            _viewModel.FirstName = "John";
            _viewModel.LastName = "";
            _viewModel.Email = "invalid";
            _viewModel.PhoneNumber = "";
            _viewModel.Country = "";

            // Act - Create guest manually to test data
            var guest = new Guest(
                _viewModel.FirstName,
                _viewModel.LastName,
                _viewModel.Email,
                _viewModel.PhoneNumber,
                _viewModel.Country,
                _viewModel.PassportNumber
            );

            var errors = guest.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Guest should have validation errors");
        }
    }
}