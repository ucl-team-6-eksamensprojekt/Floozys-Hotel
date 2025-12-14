using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Floozys_Hotel.Commands;
using Floozys_Hotel.Core;
using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories;

namespace Floozys_Hotel.ViewModels
{
    public class NewBookingViewModel : ObservableObject
    {
        // BACKING FIELDS
        private DateTime? _checkInDate;
        private DateTime? _checkOutDate;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _passportNumber;
        private string _phoneNumber;
        private string _country;
        private string _errorMessage;
        private Room _selectedRoom;
        private Guest _selectedGuest;

        // OBSERVABLE COLLECTIONS
        public ObservableCollection<Room> NewBookingRoomList { get; set; }

        // PROPERTIES

        public Room SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value;
                OnPropertyChanged();
            }
        }

        public DateTime? CheckInDate
        {
            get => _checkInDate;
            set
            {
                _checkInDate = value;
                OnPropertyChanged();
                UpdateAvailableRooms(); // UC01 Step 4: Filter rooms when date changes
            }
        }

        public DateTime? CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                _checkOutDate = value;
                OnPropertyChanged();
                UpdateAvailableRooms(); // UC01 Step 4: Filter rooms when date changes
            }
        }

        private int GuestID { get; set; }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string PassportNumber
        {
            get => _passportNumber;
            set
            {
                _passportNumber = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        public string Country
        {
            get => _country;
            set
            {
                _country = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        // COMMAND
        public ICommand ConfirmBookingCommand { get; }

        // CONSTRUCTOR

        public NewBookingViewModel(Guest? selectedGuest = null)
        {
            _selectedGuest = selectedGuest;
            ConfirmBookingCommand = new RelayCommand(CreateBooking);

            // Initialize ObservableCollection
            NewBookingRoomList = new ObservableCollection<Room>();

            // Load all available rooms initially (UC01 Step 2)
            LoadAllAvailableRooms();
        }

        // METHODS

        /// <summary>
        /// UC01 Step 2: Load all available rooms when window opens
        /// </summary>
        private void LoadAllAvailableRooms()
        {
            RoomRepo roomRepo = new RoomRepo();
            var rooms = roomRepo.GetAllByAvailability();

            NewBookingRoomList.Clear();
            foreach (Room room in rooms)
            {
                NewBookingRoomList.Add(room);
            }

            // Load existing guest if provided
            if (_selectedGuest != null)
            {
                LoadFromGuest(_selectedGuest);
            }
        }

        /// <summary>
        /// UC01 Step 4: Filter available rooms based on selected dates
        /// UC01 Alternative Flow A1: Show "No rooms available" message
        /// </summary>
        private void UpdateAvailableRooms()
        {
            // If no dates selected, show all available rooms
            if (!CheckInDate.HasValue || !CheckOutDate.HasValue)
            {
                LoadAllAvailableRooms();
                ErrorMessage = string.Empty;
                return;
            }

            // Validate dates
            if (CheckOutDate.Value <= CheckInDate.Value)
            {
                NewBookingRoomList.Clear();
                ErrorMessage = "Check-out date must be after check-in date";
                return;
            }

            // Get all available rooms (by status)
            RoomRepo roomRepo = new RoomRepo();
            var allAvailableRooms = roomRepo.GetAllByAvailability();

            // Get bookings that overlap with selected period
            BookingRepo bookingRepo = new BookingRepo();
            var bookingsInPeriod = bookingRepo.GetAll()
                .Where(b =>
                    b.Status != BookingStatus.Cancelled && // Ignore cancelled bookings
                    b.StartDate < CheckOutDate.Value &&    // Booking starts before our checkout
                    b.EndDate > CheckInDate.Value)         // Booking ends after our checkin
                .ToList();

            // Get list of room IDs that ARE booked in this period
            var bookedRoomIds = bookingsInPeriod.Select(b => b.RoomID).Distinct().ToList();

            // Filter out booked rooms (UC01 Step 4: Show only available rooms)
            var availableInPeriod = allAvailableRooms
                .Where(r => !bookedRoomIds.Contains(r.RoomId))
                .ToList();

            // Update ObservableCollection
            NewBookingRoomList.Clear();
            foreach (Room room in availableInPeriod)
            {
                NewBookingRoomList.Add(room);
            }

            // UC01 Alternative Flow A1: No rooms available
            if (availableInPeriod.Count == 0)
            {
                ErrorMessage = "⚠️ No rooms available for the selected period. Please try different dates.";
            }
            else
            {
                ErrorMessage = string.Empty; // Clear error if rooms found
            }
        }

        /// <summary>
        /// Load guest information into form fields
        /// </summary>
        private void LoadFromGuest(Guest guest)
        {
            GuestID = guest.GuestID;
            FirstName = guest.FirstName;
            LastName = guest.LastName;
            Email = guest.Email;
            PhoneNumber = guest.PhoneNumber;
            Country = guest.Country;
            PassportNumber = guest.PassportNumber;
        }

        /// <summary>
        /// UC01 Step 8-9: Create and save booking
        /// </summary>
        private void CreateBooking(object parameter)
        {
            try
            {
                ErrorMessage = string.Empty;

                // STEP 1: VALIDATE DATES
                if (!CheckInDate.HasValue)
                {
                    throw new ArgumentException("Check-in date is required");
                }

                if (!CheckOutDate.HasValue)
                {
                    throw new ArgumentException("Check-out date is required");
                }

                if (CheckOutDate.Value <= CheckInDate.Value)
                {
                    throw new ArgumentException("Check-out date must be after check-in date");
                }

                if (CheckInDate.Value.Date < DateTime.Now.Date)
                {
                    throw new ArgumentException("Check-in date cannot be in the past");
                }

                // STEP 2: VALIDATE ROOM SELECTION (UC01 Step 6)
                if (SelectedRoom == null)
                {
                    throw new ArgumentException("Please select a room");
                }

                // STEP 3: CREATE AND VALIDATE GUEST (UC01 Step 7)
                var guest = new Guest(
                    firstName: FirstName,
                    lastName: LastName,
                    email: Email,
                    phoneNumber: PhoneNumber,
                    country: Country,
                    passportNumber: PassportNumber
                );

                guest.GuestID = GuestID;

                var guestErrors = guest.Validate();

                if (guestErrors.Any())
                {
                    throw new ArgumentException(guestErrors.First()); // UC01 Alt Flow A2
                }

                // STEP 4: SAVE GUEST TO DATABASE FIRST (only if new guest)
                if (guest.GuestID == 0)
                {
                    GuestRepo guestRepo = new GuestRepo();
                    int guestId = guestRepo.AddGuest(guest);
                    guest.GuestID = guestId;
                }

                // STEP 5: CREATE BOOKING
                var booking = new Booking
                {
                    StartDate = CheckInDate.Value,
                    EndDate = CheckOutDate.Value,
                    Status = BookingStatus.Pending,
                    RoomID = SelectedRoom.RoomId,
                    GuestID = guest.GuestID,
                    Room = SelectedRoom,
                    Guest = guest
                };

                // STEP 6: SAVE BOOKING TO DATABASE (UC01 Step 8)
                BookingRepo bookingRepo = new BookingRepo();
                bookingRepo.Create(booking);

                // STEP 7: SUCCESS MESSAGE (UC01 Step 9)
                ErrorMessage = $"✅ Booking saved! Booking #{booking.BookingID} created for {guest.FirstName} {guest.LastName} ({booking.NumberOfNights} nights)";

                // STEP 8: Close window after 2 seconds (UC01 Step 9: Return to calendar)
                System.Threading.Tasks.Task.Delay(2000).ContinueWith(_ =>
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        CloseWindow();
                    });
                });
            }
            catch (ArgumentException ex)
            {
                ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                ErrorMessage = "An unexpected error occurred: " + ex.Message;
            }
        }

        private void CloseWindow()
        {
            // Find and close the NewBookingView window
            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
            {
                if (window is Views.NewBookingView)
                {
                    window.Close();
                    break;
                }
            }
        }

        private void ClearForm()
        {
            CheckInDate = null;
            CheckOutDate = null;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            PassportNumber = string.Empty;
            PhoneNumber = string.Empty;
            Country = string.Empty;
            SelectedRoom = null;
        }
    }
}