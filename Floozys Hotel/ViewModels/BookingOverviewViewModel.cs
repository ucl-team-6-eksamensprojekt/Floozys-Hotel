using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Floozys_Hotel.Commands;
using Floozys_Hotel.Core;
using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories;
using Floozys_Hotel.Views;

namespace Floozys_Hotel.ViewModels
{
    class BookingOverviewViewModel : ObservableObject
    {
        private readonly BookingRepo _bookingRepo;
        private readonly RoomRepo _roomRepo;
        private DateTime _currentMonth;
        private decimal _revenueThisMonth;
        private string _searchText = "";
        private string _viewDuration = "Month";
        private string _sortColumn = "";
        private bool _sortAscending = true;

        // Lister til data som Viewet binder til.
        public ObservableCollection<Room> Rooms { get; set; }
        public ObservableCollection<Booking> Bookings { get; set; }
        public ObservableCollection<Booking> FilteredBookings { get; set; }
        public ObservableCollection<int> DaysInMonth { get; set; }

        public int DayCount => DaysInMonth.Count;

        // Finder startdatoen for den viste periode (starten af ugen eller måneden).
        // Vigtigt for korrekt placering af bookinger.
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

        private Booking _selectedBooking;
        // Gemmer den valgte booking, så detaljer kan vises.
        public Booking SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                _selectedBooking = value;
                OnPropertyChanged();
            }
        }

        // Binder til søgefeltet. Opdaterer automatisk søgningen ved ændringer.
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplySearchFilter();
            }
        }

        // Styrer visningen: "Week", "Month" eller "Year". Opdaterer visningen ved ændring.
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

        // Kommandoer som knapperne i Viewet bruger.
        public RelayCommand NewBookingCommand { get; set; }
        public RelayCommand NextMonthCommand { get; set; }
        public RelayCommand PreviousMonthCommand { get; set; }
        public RelayCommand SelectBookingCommand { get; set; }
        public RelayCommand SetWeekViewCommand { get; set; }
        public RelayCommand SetMonthViewCommand { get; set; }
        public RelayCommand SetYearViewCommand { get; set; }
        public RelayCommand SortBookingsCommand { get; set; }

        // Holder styr på den aktuelle dato/måned.
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

        // Viser en læsbar overskrift, f.eks. "December 2025" eller "Week 48, 2025".
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

        // Initialiserer repositories, lister og kommandoer.
        public BookingOverviewViewModel()
        {
            _bookingRepo = new BookingRepo();
            _roomRepo = new RoomRepo();

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
            SortBookingsCommand = new RelayCommand(SortBookings);

            LoadData();
        }

        // Henter værelser og bookinger fra databasen og lægger dem i listerne.
        private void LoadData()
        {
            var rooms = _roomRepo.GetAllRooms();
            Rooms.Clear();
            foreach (var room in rooms)
            {
                Rooms.Add(room);
            }

            var bookings = _bookingRepo.GetAllBookings();
            Bookings.Clear();
            foreach (var booking in bookings)
            {
                Bookings.Add(booking);
            }

            FilterBookings();
            UpdateRevenue();
            UpdateDaysInMonth();
        }

        // Går frem eller tilbage i tid, afhængigt af visningen.
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

        // Beregner hvilke dage der skal vises i headeren.
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

        // Forbedret filtrering baseret på søgetekst. Søger på tværs af gæsteoplysninger.
        private void ApplySearchFilter()
        {
            FilteredBookings.Clear();

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // Viser alle bookinger fra perioden ved manglende søgetekst
                FilterBookings();
                return;
            }

            // Søger på tværs af gæstefelter
            var matching = Bookings.Where(b =>
                (b.Guest?.FirstName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true) ||
                (b.Guest?.LastName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true) ||
                (b.Guest?.Country?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true) ||
                (b.Guest?.Email?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true) ||
                (b.Guest?.PhoneNumber?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true) ||
                b.GuestID.ToString().Contains(SearchText) ||
                b.RoomID.ToString().Contains(SearchText)
            ).ToList();

            foreach (var booking in matching)
            {
                FilteredBookings.Add(booking);
            }

            // Anvender sortering, hvis aktiv
            ApplySorting();
        }

        // Sikrer at kun bookinger inden for den valgte periode vises.
        private void FilterBookings()
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
            else // Month
            {
                startPeriod = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 1);
                endPeriod = startPeriod.AddMonths(1).AddDays(-1);
            }

            var filtered = Bookings.Where(b => 
                b.StartDate <= endPeriod && b.EndDate >= startPeriod);

            foreach (var booking in filtered)
            {
                FilteredBookings.Add(booking);
            }
        }

        private void UpdateRevenue()
        {
            // TODO: Beregner omsætning når prissætning er implementeret
            RevenueThisMonth = 0;
        }

        // Sorterer bookinger baseret på valgt kolonne.
        private void SortBookings(object parameter)
        {
            string column = parameter as string;
            if (string.IsNullOrEmpty(column)) return;

            // Skifter sorteringsretning ved valg af samme kolonne
            if (_sortColumn == column)
                _sortAscending = !_sortAscending;
            else
            {
                _sortColumn = column;
                _sortAscending = true;
            }

            ApplySorting();
        }

        // Anvender sortering på FilteredBookings.
        private void ApplySorting()
        {
            if (string.IsNullOrEmpty(_sortColumn) || FilteredBookings.Count == 0) return;

            var sorted = FilteredBookings.ToList();

            sorted = _sortColumn switch
            {
                "Name" => _sortAscending
                    ? sorted.OrderBy(b => b.Guest?.FirstName).ThenBy(b => b.Guest?.LastName).ToList()
                    : sorted.OrderByDescending(b => b.Guest?.FirstName).ThenByDescending(b => b.Guest?.LastName).ToList(),
                "Country" => _sortAscending
                    ? sorted.OrderBy(b => b.Guest?.Country).ToList()
                    : sorted.OrderByDescending(b => b.Guest?.Country).ToList(),
                "Email" => _sortAscending
                    ? sorted.OrderBy(b => b.Guest?.Email).ToList()
                    : sorted.OrderByDescending(b => b.Guest?.Email).ToList(),
                "Phone" => _sortAscending
                    ? sorted.OrderBy(b => b.Guest?.PhoneNumber).ToList()
                    : sorted.OrderByDescending(b => b.Guest?.PhoneNumber).ToList(),
                "Room" => _sortAscending
                    ? sorted.OrderBy(b => b.RoomID).ToList()
                    : sorted.OrderByDescending(b => b.RoomID).ToList(),
                "StartDate" => _sortAscending
                    ? sorted.OrderBy(b => b.StartDate).ToList()
                    : sorted.OrderByDescending(b => b.StartDate).ToList(),
                "EndDate" => _sortAscending
                    ? sorted.OrderBy(b => b.EndDate).ToList()
                    : sorted.OrderByDescending(b => b.EndDate).ToList(),
                _ => sorted
            };

            FilteredBookings.Clear();
            foreach (var booking in sorted)
            {
                FilteredBookings.Add(booking);
            }
        }

        // Åbner vindue til ny booking og genindlæser data.
        private void OpenNewBooking()
        {
            var newBookingWindow = new NewBookingView();
            newBookingWindow.ShowDialog();
            LoadData();
        }
    }
}
