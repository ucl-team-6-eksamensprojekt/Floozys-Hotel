using System.Collections.ObjectModel;
using Floozys_Hotel.Commands;
using Floozys_Hotel.Core;
using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories;
using Floozys_Hotel.Repositories.Interfaces;
using Floozys_Hotel.Views;

namespace Floozys_Hotel.ViewModels
{
    public class BookingOverviewViewModel : ObservableObject
    {
        // REPOSITORIES
        private readonly IBookingRepo _bookingRepo;  // Depends on interface
        private readonly IRoomRepo _roomRepo;  // Depends on interface

        // BACKING FIELDS
        private DateTime _currentMonth;
        private decimal _revenueThisMonth;
        private string _searchText = "";
        private string _viewDuration = "Month";
        private Booking _selectedBooking;

        // COLLECTIONS

        public ObservableCollection<Room> Rooms { get; set; }
        public ObservableCollection<Booking> Bookings { get; set; }
        public ObservableCollection<Booking> FilteredBookings { get; set; }
        public ObservableCollection<int> DaysInMonth { get; set; }

        // PROPERTIES

        public int DayCount => DaysInMonth.Count;

        public DateTime ViewStartDate
        {
            get
            {
                if (ViewDuration == "Week")
                {
                    return CurrentMonth.AddDays(-(int)CurrentMonth.DayOfWeek);
                }
                else
                {
                    return new DateTime(CurrentMonth.Year, CurrentMonth.Month, 1);
                }
            }
        }

        public Booking SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                _selectedBooking = value;
                OnPropertyChanged();
                // Notify commands that CanExecute may have changed
                CheckInBookingCommand?.RaiseCanExecuteChanged();
                CheckOutBookingCommand?.RaiseCanExecuteChanged();
            }
        }

        public string SearchText  // Bound to search field, triggers filtering
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                if (!string.IsNullOrWhiteSpace(_searchText) && ViewDuration != "Year")
                {
                    ViewDuration = "Year";
                }
                FilterBookings();
            }
        }

        public string ViewDuration  // Controls display mode: Week, Month, or Year
        {
            get => _viewDuration;
            set
            {
                _viewDuration = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentMonthDisplay));
                OnPropertyChanged(nameof(DayCount));
                OnPropertyChanged(nameof(ViewStartDate));
                UpdateDaysInMonth();
                FilterBookings();
            }
        }

        public DateTime CurrentMonth
        {
            get => _currentMonth;
            set
            {
                _currentMonth = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentMonthDisplay));
                OnPropertyChanged(nameof(ViewStartDate));
                FilterBookings();
                UpdateRevenue();
            }
        }

        public string CurrentMonthDisplay  // Displays readable header like "December 2025" or "Week 48, 2025"
        {
            get
            {
                if (ViewDuration == "Week")
                {
                    var culture = System.Globalization.CultureInfo.CurrentCulture;
                    var calendar = culture.Calendar;
                    var weekRule = culture.DateTimeFormat.CalendarWeekRule;
                    var firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;
                    int weekNum = calendar.GetWeekOfYear(_currentMonth, weekRule, firstDayOfWeek);
                    return $"Week {weekNum}, {_currentMonth.Year}";
                }
                else if (ViewDuration == "Year")
                {
                    return $"{_currentMonth.Year}";
                }
                else
                {
                    return _currentMonth.ToString("MMMM yyyy");
                }
            }
        }

        public decimal RevenueThisMonth
        {
            get => _revenueThisMonth;
            set
            {
                _revenueThisMonth = value;
                OnPropertyChanged();
            }
        }

        // COMMANDS

        public RelayCommand NewBookingCommand { get; set; }
        public RelayCommand NextMonthCommand { get; set; }
        public RelayCommand PreviousMonthCommand { get; set; }
        public RelayCommand SelectBookingCommand { get; set; }
        public RelayCommand CheckInBookingCommand { get; set; }
        public RelayCommand CheckOutBookingCommand { get; set; }
        public RelayCommand SetWeekViewCommand { get; set; }
        public RelayCommand SetMonthViewCommand { get; set; }
        public RelayCommand SetYearViewCommand { get; set; }

        // CONSTRUCTORS

        public BookingOverviewViewModel() : this(new BookingRepo(), new RoomRepo())  // Parameterless for XAML - calls overloaded constructor
        {
        }

        public BookingOverviewViewModel(IBookingRepo bookingRepo, IRoomRepo roomRepo)
        {
            _bookingRepo = bookingRepo;
            _roomRepo = roomRepo;

            Rooms = new ObservableCollection<Room>();
            Bookings = new ObservableCollection<Booking>();
            FilteredBookings = new ObservableCollection<Booking>();
            DaysInMonth = new ObservableCollection<int>();

            CurrentMonth = DateTime.Today;

            NewBookingCommand = new RelayCommand(n => OpenNewBooking());
            NextMonthCommand = new RelayCommand(n => ChangeMonth(1));
            PreviousMonthCommand = new RelayCommand(n => ChangeMonth(-1));
            SelectBookingCommand = new RelayCommand(b => SelectedBooking = (Booking)b);
            SetWeekViewCommand = new RelayCommand(w => ViewDuration = "Week");
            SetMonthViewCommand = new RelayCommand(m => ViewDuration = "Month");
            SetYearViewCommand = new RelayCommand(y => ViewDuration = "Year");
            SortCommand = new RelayCommand(SortBookings);

            CheckInBookingCommand = new RelayCommand(
                execute: _ => CheckInBooking(),
                canExecute: _ => CanExecuteCheckIn()
            );

            CheckOutBookingCommand = new RelayCommand(
                execute: _ => CheckOutBooking(),
                canExecute: _ => CanExecuteCheckOut()
            );

            LoadData();
        }

        // METHODS

        private void LoadData()  // Fetches data from repositories
        {
            var rooms = _roomRepo.GetAll();
            Rooms.Clear();
            foreach (var room in rooms)
            {
                Rooms.Add(room);
            }

            var bookings = _bookingRepo.GetAll();
            Bookings.Clear();
            foreach (var booking in bookings)
            {
                Bookings.Add(booking);
            }

            FilterBookings();
            UpdateRevenue();
            UpdateDaysInMonth();
        }

        /// <summary>
        /// UC01 Step 9: Refresh calendar data to show new booking
        /// </summary>
        public void RefreshData()
        {
            LoadData();
        }

        private void ChangeMonth(int months)  // Navigates forward/backward in time
        {
            if (ViewDuration == "Week")
            {
                CurrentMonth = CurrentMonth.AddDays(months * 7);
            }
            else if (ViewDuration == "Year")
            {
                CurrentMonth = CurrentMonth.AddYears(months);
            }
            else
            {
                CurrentMonth = CurrentMonth.AddMonths(months);
            }
            UpdateDaysInMonth();
        }

        private void UpdateDaysInMonth()  // Calculates which days to display in header
        {
            DaysInMonth.Clear();

            if (ViewDuration == "Week")
            {
                var startOfWeek = CurrentMonth.AddDays(-(int)CurrentMonth.DayOfWeek);
                for (int i = 0; i < 7; i++)
                {
                    DaysInMonth.Add(startOfWeek.AddDays(i).Day);
                }
            }
            else if (ViewDuration == "Month")
            {
                int days = DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.Month);
                for (int i = 1; i <= days; i++)
                {
                    DaysInMonth.Add(i);
                }
            }
            else if (ViewDuration == "Year")
            {
                for (int i = 1; i <= 12; i++)
                {
                    DaysInMonth.Add(i);
                }
            }

            OnPropertyChanged(nameof(DayCount));
        }

        private bool MatchesSearch(Booking b)
        {
            if (b.Guest == null || b.Room == null) return false;
            string search = SearchText.Trim();
            return (b.Guest.FirstName + " " + b.Guest.LastName).Contains(search, StringComparison.OrdinalIgnoreCase) ||
                   (b.Guest.Country ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                   (b.Guest.Email ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                   b.Room.RoomNumber.ToString().Contains(search, StringComparison.OrdinalIgnoreCase);
        }

        private void FilterBookings()  // Ensures only bookings within selected period are shown
        {
            FilteredBookings.Clear();

            DateTime startPeriod;
            DateTime endPeriod;

            if (ViewDuration == "Year")
            {
                startPeriod = new DateTime(CurrentMonth.Year, 1, 1);
                endPeriod = new DateTime(CurrentMonth.Year, 12, 31);
            }
            else if (ViewDuration == "Week")
            {
                startPeriod = CurrentMonth.AddDays(-(int)CurrentMonth.DayOfWeek);
                endPeriod = startPeriod.AddDays(6);
            }
            else
            {
                startPeriod = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 1);
                endPeriod = startPeriod.AddMonths(1).AddDays(-1);
            }

            var filtered = Bookings.Where(b =>
                b.StartDate <= endPeriod && 
                b.EndDate >= startPeriod &&
                b.Status != BookingStatus.Cancelled);

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(b => MatchesSearch(b));
            }

            foreach (var booking in filtered)
            {
                FilteredBookings.Add(booking);
            }
        }

        private void UpdateRevenue()
        {
            // TODO: Calculate revenue when pricing is implemented
            RevenueThisMonth = 0;
        }

        private string _sortColumn;
        private bool _isAscending;

        public RelayCommand SortCommand { get; set; }

        private void OpenNewBooking()
        {
            var newBookingWindow = new NewBookingView();
            newBookingWindow.ShowDialog(); // Blocks until window closes

            // UC01 Step 9: Reload calendar data so new booking appears
            LoadData();
        }

        private void SortBookings(object parameter)
        {
            var column = parameter as string;
            if (string.IsNullOrEmpty(column)) return;

            if (_sortColumn == column)
            {
                // Toggle direction
                _isAscending = !_isAscending;
            }
            else
            {
                // New column, default to ascending
                _sortColumn = column;
                _isAscending = true;
            }

            // Sort the FilteredBookings collection
            var sorted = _isAscending 
                ? FilteredBookings.OrderBy(b => GetPropValue(b, _sortColumn)).ToList()
                : FilteredBookings.OrderByDescending(b => GetPropValue(b, _sortColumn)).ToList();

            FilteredBookings.Clear();
            foreach (var b in sorted)
            {
                FilteredBookings.Add(b);
            }
        }

        private object GetPropValue(object src, string propName)
        {
            if (src == null) return null;
            if (propName.Contains("."))
            {
                var split = propName.Split(new[] { '.' }, 2);
                return GetPropValue(GetPropValue(src, split[0]), split[1]);
            }
            var prop = src.GetType().GetProperty(propName);
            return prop != null ? prop.GetValue(src, null) : null;
        }

        /// <summary>
        /// UC03: Check-in guest
        /// </summary>
        private void CheckInBooking()
        {
            if (SelectedBooking == null) return;

            try
            {
                _bookingRepo.CheckIn(SelectedBooking.BookingID);

                // Refresh data to show updated status
                LoadData();

                // TODO: Show success message to user
            }
            catch (Exception ex)
            {
                // TODO: Show error message to user
                System.Diagnostics.Debug.WriteLine($"Check-in failed: {ex.Message}");
            }
        }

        /// <summary>
        /// UC04: Check-out guest
        /// </summary>
        private void CheckOutBooking()
        {
            if (SelectedBooking == null) return;

            try
            {
                _bookingRepo.CheckOut(SelectedBooking.BookingID);

                // Refresh data to show updated status
                LoadData();

                // TODO: Show success message to user
            }
            catch (Exception ex)
            {
                // TODO: Show error message to user
                System.Diagnostics.Debug.WriteLine($"Check-out failed: {ex.Message}");
            }
        }

        // Check-in/out eligibility helpers
        private bool CanExecuteCheckIn()
        {
            return SelectedBooking != null && SelectedBooking.CanCheckIn();
        }

        private bool CanExecuteCheckOut()
        {
            return SelectedBooking != null && SelectedBooking.CanCheckOut();
        }

        // UC03: Guest check-in
        private void CheckInBooking()
        {
            if (SelectedBooking == null) return;

            try
            {
                _bookingRepo.CheckIn(SelectedBooking.BookingID);
                LoadData();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Check-in failed: {ex.Message}");
            }
        }

        // UC04: Guest check-out
        private void CheckOutBooking()
        {
            if (SelectedBooking == null) return;

            try
            {
                _bookingRepo.CheckOut(SelectedBooking.BookingID);
                LoadData();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Check-out failed: {ex.Message}");
            }
        }
    }
}