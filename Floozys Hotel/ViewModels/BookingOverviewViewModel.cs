using System.Collections.ObjectModel;
using Floozys_Hotel.Commands;
using Floozys_Hotel.Core;
using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories;
using Floozys_Hotel.Repositories.Interfaces;
using Floozys_Hotel.Views;
using System.Linq;

namespace Floozys_Hotel.ViewModels
{
    public class BookingOverviewViewModel : ObservableObject
    {
        // REPOSITORIES
        private readonly IBookingRepo _bookingRepo;
        private readonly IRoomRepo _roomRepo;

        // BACKING FIELDS
        private DateTime _currentMonth;
        private decimal _revenueThisMonth;
        private string _searchText = "";
        private string _viewDuration = "Month";
        private Booking _selectedBooking;
        private bool _isEditMode;  

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
                CheckInBookingCommand?.RaiseCanExecuteChanged();
                CheckOutBookingCommand?.RaiseCanExecuteChanged();
                CancelBookingCommand?.RaiseCanExecuteChanged();
                EditBookingCommand?.RaiseCanExecuteChanged();
            }
        }

        public string SearchText
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

        public string ViewDuration
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

        public string CurrentMonthDisplay
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

        // UC03: Edit mode properties
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsViewMode));
                EditBookingCommand?.RaiseCanExecuteChanged();
                SaveEditCommand?.RaiseCanExecuteChanged();
                CancelEditCommand?.RaiseCanExecuteChanged();
            }
        }

        public bool IsViewMode => !IsEditMode;

        // Edit mode - temporary values
        private Guest _editGuest;
        private Room _editRoom;
        private DateTime _editCheckInDate;
        private DateTime _editCheckOutDate;

        public Guest EditGuest
        {
            get => _editGuest;
            set { _editGuest = value; OnPropertyChanged(); }
        }

        public Room EditRoom
        {
            get => _editRoom;
            set { _editRoom = value; OnPropertyChanged(); }
        }

        public DateTime EditCheckInDate
        {
            get => _editCheckInDate;
            set { _editCheckInDate = value; OnPropertyChanged(); }
        }

        public DateTime EditCheckOutDate
        {
            get => _editCheckOutDate;
            set { _editCheckOutDate = value; OnPropertyChanged(); }
        }

        // Collections for edit dropdowns
        public ObservableCollection<Guest> AllGuests { get; set; }
        public ObservableCollection<Room> AllRooms { get; set; }

        // COMMANDS

        public RelayCommand NewBookingCommand { get; set; }
        public RelayCommand NextMonthCommand { get; set; }
        public RelayCommand PreviousMonthCommand { get; set; }
        public RelayCommand SelectBookingCommand { get; set; }
        public RelayCommand CheckInBookingCommand { get; set; }
        public RelayCommand CheckOutBookingCommand { get; set; }
        public RelayCommand CancelBookingCommand { get; set; }
        public RelayCommand EditBookingCommand { get; set; }  
        public RelayCommand SaveEditCommand { get; set; }    
        public RelayCommand CancelEditCommand { get; set; }   
        public RelayCommand SetWeekViewCommand { get; set; }
        public RelayCommand SetMonthViewCommand { get; set; }
        public RelayCommand SetYearViewCommand { get; set; }
        public RelayCommand SortCommand { get; set; }

        // CONSTRUCTORS

        public BookingOverviewViewModel() : this(new BookingRepo(), new RoomRepo())
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

            CancelBookingCommand = new RelayCommand(  // ← NY!
                execute: _ => CancelBooking(),
                canExecute: _ => CanExecuteCancel()
            );

            // UC03: Edit booking commands
            EditBookingCommand = new RelayCommand(
                execute: _ => EnterEditMode(),
                canExecute: _ => CanExecuteEdit()
            );

            SaveEditCommand = new RelayCommand(
                execute: _ => SaveEdit(),
                canExecute: _ => IsEditMode
            );

            CancelEditCommand = new RelayCommand(
                execute: _ => CancelEdit(),
                canExecute: _ => IsEditMode
            );

            // Initialize collections for edit dropdowns
            AllGuests = new ObservableCollection<Guest>();
            AllRooms = new ObservableCollection<Room>();

            LoadData();
        }

        // METHODS

        private void LoadData()
        {
            var rooms = _roomRepo.GetAll();
            Rooms.Clear();
            foreach (var room in rooms)
            {
                Rooms.Add(room);
            }

            // Load all rooms for edit dropdown
            AllRooms.Clear();
            foreach (var room in rooms)
            {
                AllRooms.Add(room);
            }

            var bookings = _bookingRepo.GetAll();
            Bookings.Clear();
            foreach (var booking in bookings)
            {
                Bookings.Add(booking);
            }

            // Load all guests for edit dropdown
            var guestRepo = new GuestRepo();
            var guests = guestRepo.GetAll();
            AllGuests.Clear();
            foreach (var guest in guests)
            {
                AllGuests.Add(guest);
            }

            FilterBookings();
            UpdateRevenue();
            UpdateDaysInMonth();
        }

        public void RefreshData()
        {
            LoadData();
        }

        private void ChangeMonth(int months)
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

        private void UpdateDaysInMonth()
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

        private void FilterBookings()
        {
            FilteredBookings.Clear();

            DateTime startPeriod;
            DateTime endPeriod;

            IEnumerable<Booking> filtered;

            if (ViewDuration == "Year")
            {
                startPeriod = new DateTime(CurrentMonth.Year, 1, 1);
                endPeriod = new DateTime(CurrentMonth.Year, 12, 31);
                
                filtered = Bookings.Where(b =>
                    b.StartDate <= endPeriod &&
                    b.EndDate >= startPeriod &&
                    b.Status != BookingStatus.Cancelled);
            }
            else 
            {
                // For Month/Week views, we do NOT filter by date range.
                // We rely on the Converters to handle visibility based on ViewStartDate.
                // This prevents issues where bookings overlapping the edge of the view might be filtered out incorrectly or cause rendering glitches.
                filtered = Bookings.Where(b => b.Status != BookingStatus.Cancelled);
            }

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
            RevenueThisMonth = 0;
        }

        private string _sortColumn;
        private bool _isAscending;

        private void OpenNewBooking()
        {
            var newBookingWindow = new NewBookingView();
            newBookingWindow.ShowDialog();
            LoadData();
        }

        private void SortBookings(object parameter)
        {
            var column = parameter as string;
            if (string.IsNullOrEmpty(column)) return;

            if (_sortColumn == column)
            {
                _isAscending = !_isAscending;
            }
            else
            {
                _sortColumn = column;
                _isAscending = true;
            }

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

        // Cancellation eligibility
        private bool CanExecuteCancel()
        {
            return SelectedBooking != null && SelectedBooking.CanCancel();
        }

        // UC04: Cancel booking
        private void CancelBooking()
        {
            if (SelectedBooking == null) return;

            var result = System.Windows.MessageBox.Show(
                "Cancel Booking Permanently From System?",
                "Confirm Cancellation",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning
            );

            if (result != System.Windows.MessageBoxResult.Yes)
                return;

            try
            {
                _bookingRepo.CancelBooking(SelectedBooking.BookingID);
                LoadData();

                System.Windows.MessageBox.Show(
                    "Booking Cancelled Successfully",
                    "Success",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Cancellation failed: {ex.Message}",
                    "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error
                );
            }
        }

        // UC03: Edit booking eligibility
        private bool CanExecuteEdit()
        {
            return SelectedBooking != null && SelectedBooking.CanEdit();
        }

        // UC03: Enter edit mode
        private void EnterEditMode()
        {
            if (SelectedBooking == null) return;

            // Store current values in edit properties
            EditGuest = AllGuests.FirstOrDefault(g => g.GuestID == SelectedBooking.GuestID);
            EditRoom = AllRooms.FirstOrDefault(r => r.RoomId == SelectedBooking.RoomID);
            EditCheckInDate = SelectedBooking.StartDate;
            EditCheckOutDate = SelectedBooking.EndDate;

            IsEditMode = true;
        }

        // UC03: Save edited booking
        private void SaveEdit()
        {
            if (SelectedBooking == null) return;

            try
            {
                _bookingRepo.EditBooking(
                    SelectedBooking.BookingID,
                    EditCheckInDate,
                    EditCheckOutDate,
                    EditRoom.RoomId,
                    SelectedBooking.GuestID
                );

                IsEditMode = false;
                LoadData();

                System.Windows.MessageBox.Show(
                    "Booking updated successfully",
                    "Success",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Failed to update booking: {ex.Message}",
                    "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error
                );
            }
        }

        // UC03: Cancel edit mode
        private void CancelEdit()
        {
            IsEditMode = false;
        }
    }
}