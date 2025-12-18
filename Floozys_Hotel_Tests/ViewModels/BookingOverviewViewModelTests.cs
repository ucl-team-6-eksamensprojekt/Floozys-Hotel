using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Floozys_Hotel.ViewModels;
using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace Floozys_Hotel_Tests.ViewModels
{
    /// <summary>
    /// Tests for BookingOverviewViewModel business logic
    /// Uses mocked repositories to isolate business logic from database
    /// FIRST Principles: Fast, Independent, Repeatable, Self-validating, Timely
    /// </summary>
    [TestClass]
    public class BookingOverviewViewModelTests
    {
        private Mock<IBookingRepo> _mockBookingRepo;
        private Mock<IRoomRepo> _mockRoomRepo;
        private BookingOverviewViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            // Arrange - Create mocks for each test
            _mockBookingRepo = new Mock<IBookingRepo>();
            _mockRoomRepo = new Mock<IRoomRepo>();
        }

        // ============================================
        // CONFIRM BOOKING TESTS (UC02)
        // ============================================

        [TestMethod]
        public void CanExecuteConfirm_PendingBooking_ReturnsTrue()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.Pending,
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3)
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canConfirm = _viewModel.ConfirmBookingCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canConfirm, "Should be able to confirm Pending booking");
        }

        [TestMethod]
        public void CanExecuteConfirm_ConfirmedBooking_ReturnsFalse()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.Confirmed,
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3)
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canConfirm = _viewModel.ConfirmBookingCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canConfirm, "Cannot confirm already Confirmed booking");
        }

        [TestMethod]
        public void CanExecuteConfirm_CheckedInBooking_ReturnsFalse()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.CheckedIn,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2),
                CheckInTime = DateTime.Now
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canConfirm = _viewModel.ConfirmBookingCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canConfirm, "Cannot confirm CheckedIn booking");
        }

        [TestMethod]
        public void CanExecuteConfirm_NoSelectedBooking_ReturnsFalse()
        {
            // Arrange
            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking>());
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = null;

            // Act
            var canConfirm = _viewModel.ConfirmBookingCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canConfirm, "Cannot confirm without selected booking");
        }

        // ============================================
        // CHECK-IN TESTS (UC20)
        // ============================================

        [TestMethod]
        public void CanExecuteCheckIn_PendingBookingOnStartDate_ReturnsTrue()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.Pending,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2),
                CheckInTime = null
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canCheckIn = _viewModel.CheckInBookingCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canCheckIn, "Should be able to check in Pending booking on start date");
        }

        [TestMethod]
        public void CanExecuteCheckIn_ConfirmedBookingOnStartDate_ReturnsTrue()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.Confirmed,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2),
                CheckInTime = null
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canCheckIn = _viewModel.CheckInBookingCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canCheckIn, "Should be able to check in Confirmed booking on start date");
        }

        [TestMethod]
        public void CanExecuteCheckIn_BeforeStartDate_ReturnsFalse()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.Confirmed,
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3),
                CheckInTime = null
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canCheckIn = _viewModel.CheckInBookingCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canCheckIn, "Cannot check in before start date");
        }

        [TestMethod]
        public void CanExecuteCheckIn_AlreadyCheckedIn_ReturnsFalse()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.CheckedIn,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2),
                CheckInTime = DateTime.Now
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canCheckIn = _viewModel.CheckInBookingCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canCheckIn, "Cannot check in already checked-in booking");
        }

        [TestMethod]
        public void CanExecuteCheckIn_CancelledBooking_ReturnsFalse()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.Cancelled,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2)
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canCheckIn = _viewModel.CheckInBookingCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canCheckIn, "Cannot check in cancelled booking");
        }

        // ============================================
        // CHECK-OUT TESTS (UC21)
        // ============================================

        [TestMethod]
        public void CanExecuteCheckOut_CheckedInBooking_ReturnsTrue()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.CheckedIn,
                StartDate = DateTime.Today.AddDays(-1),
                EndDate = DateTime.Today.AddDays(1),
                CheckInTime = DateTime.Now.AddDays(-1),
                CheckOutTime = null
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canCheckOut = _viewModel.CheckOutBookingCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canCheckOut, "Should be able to check out checked-in booking");
        }

        [TestMethod]
        public void CanExecuteCheckOut_NotCheckedIn_ReturnsFalse()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.Confirmed,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2),
                CheckInTime = null
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canCheckOut = _viewModel.CheckOutBookingCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canCheckOut, "Cannot check out booking that isn't checked in");
        }

        [TestMethod]
        public void CanExecuteCheckOut_AlreadyCheckedOut_ReturnsFalse()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.CheckedOut,
                StartDate = DateTime.Today.AddDays(-2),
                EndDate = DateTime.Today.AddDays(-1),
                CheckInTime = DateTime.Now.AddDays(-2),
                CheckOutTime = DateTime.Now.AddDays(-1)
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canCheckOut = _viewModel.CheckOutBookingCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canCheckOut, "Cannot check out already checked-out booking");
        }

        // ============================================
        // CANCEL BOOKING TESTS (UC04)
        // ============================================

        [TestMethod]
        public void CanExecuteCancel_PendingBooking_ReturnsTrue()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.Pending,
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3)
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canCancel = _viewModel.CancelBookingCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canCancel, "Should be able to cancel Pending booking");
        }

        [TestMethod]
        public void CanExecuteCancel_ConfirmedBooking_ReturnsTrue()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.Confirmed,
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3)
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canCancel = _viewModel.CancelBookingCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canCancel, "Should be able to cancel Confirmed booking");
        }

        [TestMethod]
        public void CanExecuteCancel_CheckedInBooking_ReturnsFalse()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.CheckedIn,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2),
                CheckInTime = DateTime.Now
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canCancel = _viewModel.CancelBookingCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canCancel, "Cannot cancel CheckedIn booking");
        }

        [TestMethod]
        public void CanExecuteCancel_AlreadyCancelled_ReturnsFalse()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.Cancelled,
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3)
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canCancel = _viewModel.CancelBookingCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canCancel, "Cannot cancel already cancelled booking");
        }

        // ============================================
        // EDIT BOOKING TESTS (UC03)
        // ============================================

        [TestMethod]
        public void CanExecuteEdit_PendingBooking_ReturnsTrue()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.Pending,
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3)
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canEdit = _viewModel.EditBookingCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canEdit, "Should be able to edit Pending booking");
        }

        [TestMethod]
        public void CanExecuteEdit_ConfirmedBooking_ReturnsTrue()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.Confirmed,
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3)
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canEdit = _viewModel.EditBookingCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canEdit, "Should be able to edit Confirmed booking");
        }

        [TestMethod]
        public void CanExecuteEdit_CheckedInBooking_ReturnsFalse()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.CheckedIn,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2),
                CheckInTime = DateTime.Now
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canEdit = _viewModel.EditBookingCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canEdit, "Cannot edit CheckedIn booking");
        }

        [TestMethod]
        public void CanExecuteEdit_CheckedOutBooking_ReturnsFalse()
        {
            // Arrange
            var booking = new Booking
            {
                BookingID = 1,
                Status = BookingStatus.CheckedOut,
                StartDate = DateTime.Today.AddDays(-2),
                EndDate = DateTime.Today.AddDays(-1),
                CheckInTime = DateTime.Now.AddDays(-2),
                CheckOutTime = DateTime.Now.AddDays(-1)
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);
            _viewModel.SelectedBooking = booking;

            // Act
            var canEdit = _viewModel.EditBookingCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canEdit, "Cannot edit CheckedOut booking");
        }

        // ============================================
        // FILTERING TESTS
        // ============================================

        [TestMethod]
        public void FilterBookings_ExcludesCancelledBookings()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking { BookingID = 1, Status = BookingStatus.Pending, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(2) },
                new Booking { BookingID = 2, Status = BookingStatus.Cancelled, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(2) },
                new Booking { BookingID = 3, Status = BookingStatus.Confirmed, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(2) }
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(bookings);
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);

            // Act
            var filteredCount = _viewModel.FilteredBookings.Count;

            // Assert
            Assert.AreEqual(2, filteredCount, "Cancelled bookings should be filtered out");
        }

        [TestMethod]
        public void SearchText_FiltersBookingsByGuestName()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking
                {
                    BookingID = 1,
                    Status = BookingStatus.Confirmed,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(2),
                    Guest = new Guest { FirstName = "John", LastName = "Smith" },
                    Room = new Room { RoomNumber = "101" }
                },
                new Booking
                {
                    BookingID = 2,
                    Status = BookingStatus.Confirmed,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(2),
                    Guest = new Guest { FirstName = "Jane", LastName = "Doe" },
                    Room = new Room { RoomNumber = "102" }
                }
            };

            _mockBookingRepo.Setup(r => r.GetAll()).Returns(bookings);
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            _viewModel = new BookingOverviewViewModel(_mockBookingRepo.Object, _mockRoomRepo.Object);

            // Act
            _viewModel.SearchText = "John";

            // Assert
            Assert.AreEqual(1, _viewModel.FilteredBookings.Count, "Should filter to John Smith booking");
            Assert.AreEqual("John", _viewModel.FilteredBookings[0].Guest.FirstName);
        }
    }
}