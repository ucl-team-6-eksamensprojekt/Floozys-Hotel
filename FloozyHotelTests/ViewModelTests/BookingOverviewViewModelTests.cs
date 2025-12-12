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
    public class BookingOverviewViewModelTests
    {
        [TestMethod]
        public void Constructor_LoadsRoomsAndBookings()
        {
            // Arrange & Act
            var viewModel = new BookingOverviewViewModel();

            // Assert
            Assert.IsNotNull(viewModel.Rooms);
            Assert.IsTrue(viewModel.Rooms.Count > 0);
            Assert.IsNotNull(viewModel.Bookings);
            Assert.IsTrue(viewModel.Bookings.Count > 0);
        }

        [TestMethod]
        public void Constructor_InitializesCurrentMonthToToday()
        {
            // Arrange & Act
            var viewModel = new BookingOverviewViewModel();

            // Assert
            Assert.AreEqual(DateTime.Today.Year, viewModel.CurrentMonth.Year);
            Assert.AreEqual(DateTime.Today.Month, viewModel.CurrentMonth.Month);
        }

        [TestMethod]
        public void Constructor_DefaultViewDurationIsMonth()
        {
            // Arrange & Act
            var viewModel = new BookingOverviewViewModel();

            // Assert
            Assert.AreEqual("Month", viewModel.ViewDuration);
        }

        [TestMethod]
        public void ViewDuration_Month_DisplaysCorrectDayCount()
        {
            // Arrange
            var viewModel = new BookingOverviewViewModel();
            viewModel.CurrentMonth = new DateTime(2025, 12, 1);

            // Act
            viewModel.ViewDuration = "Month";

            // Assert
            Assert.AreEqual(31, viewModel.DayCount); // December has 31 days
        }

        [TestMethod]
        public void ViewDuration_Week_DisplaysSevenDays()
        {
            // Arrange
            var viewModel = new BookingOverviewViewModel();

            // Act
            viewModel.ViewDuration = "Week";

            // Assert
            Assert.AreEqual(7, viewModel.DayCount);
        }

        [TestMethod]
        public void ViewDuration_Year_DisplaysTwelveMonths()
        {
            // Arrange
            var viewModel = new BookingOverviewViewModel();

            // Act
            viewModel.ViewDuration = "Year";

            // Assert
            Assert.AreEqual(12, viewModel.DayCount);
        }

        [TestMethod]
        public void NextMonth_IncreasesMonthByOne()
        {
            // Arrange
            var viewModel = new BookingOverviewViewModel();
            viewModel.CurrentMonth = new DateTime(2025, 6, 15);

            // Act
            viewModel.NextMonthCommand.Execute(null);

            // Assert
            Assert.AreEqual(7, viewModel.CurrentMonth.Month);
            Assert.AreEqual(2025, viewModel.CurrentMonth.Year);
        }

        [TestMethod]
        public void PreviousMonth_DecreasesMonthByOne()
        {
            // Arrange
            var viewModel = new BookingOverviewViewModel();
            viewModel.CurrentMonth = new DateTime(2025, 6, 15);

            // Act
            viewModel.PreviousMonthCommand.Execute(null);

            // Assert
            Assert.AreEqual(5, viewModel.CurrentMonth.Month);
            Assert.AreEqual(2025, viewModel.CurrentMonth.Year);
        }

        [TestMethod]
        public void NextMonth_InWeekView_AddsSevenDays()
        {
            // Arrange
            var viewModel = new BookingOverviewViewModel();
            viewModel.ViewDuration = "Week";
            var startDate = new DateTime(2025, 6, 15);
            viewModel.CurrentMonth = startDate;

            // Act
            viewModel.NextMonthCommand.Execute(null);

            // Assert
            Assert.AreEqual(startDate.AddDays(7), viewModel.CurrentMonth);
        }

        [TestMethod]
        public void NextMonth_InYearView_AddsOneYear()
        {
            // Arrange
            var viewModel = new BookingOverviewViewModel();
            viewModel.ViewDuration = "Year";
            viewModel.CurrentMonth = new DateTime(2025, 6, 15);

            // Act
            viewModel.NextMonthCommand.Execute(null);

            // Assert
            Assert.AreEqual(2026, viewModel.CurrentMonth.Year);
        }

        [TestMethod]
        public void FilteredBookings_ContainsOnlyBookingsInCurrentMonth()
        {
            // Arrange
            var viewModel = new BookingOverviewViewModel();
            viewModel.CurrentMonth = DateTime.Today;
            viewModel.ViewDuration = "Month";

            // Act
            var startPeriod = new DateTime(viewModel.CurrentMonth.Year, viewModel.CurrentMonth.Month, 1);
            var endPeriod = startPeriod.AddMonths(1).AddDays(-1);

            // Assert
            foreach (var booking in viewModel.FilteredBookings)
            {
                bool overlaps = booking.StartDate <= endPeriod && booking.EndDate >= startPeriod;
                Assert.IsTrue(overlaps, $"Booking {booking.BookingID} should overlap with current month");
            }
        }

        [TestMethod]
        public void CurrentMonthDisplay_ShowsCorrectFormat()
        {
            // Arrange
            var viewModel = new BookingOverviewViewModel();
            viewModel.CurrentMonth = new DateTime(2025, 12, 15);
            viewModel.ViewDuration = "Month";

            // Act
            var display = viewModel.CurrentMonthDisplay;

            // Assert
            Assert.IsTrue(display.Contains("December") || display.Contains("december"));
            Assert.IsTrue(display.Contains("2025"));
        }

        [TestMethod]
        public void SelectBooking_SetsSelectedBooking()
        {
            // Arrange
            var viewModel = new BookingOverviewViewModel();
            var booking = viewModel.Bookings.First();

            // Act
            viewModel.SelectBookingCommand.Execute(booking);

            // Assert
            Assert.AreEqual(booking, viewModel.SelectedBooking);
        }
    }
}