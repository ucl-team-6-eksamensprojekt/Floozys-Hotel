using System;
using System.Linq;
using System.Windows.Input;
using Floozys_Hotel.Commands;
using Floozys_Hotel.Core;
using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories;
using Floozys_Hotel.Repositories.Interfaces;

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

        // REPOSITORY
        private readonly IBooking _bookingRepo;

        // PROPERTIES

        public DateTime? CheckInDate
        {
            get => _checkInDate;
            set
            {
                _checkInDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime? CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                _checkOutDate = value;
                OnPropertyChanged();
            }
        }

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

        // CONSTRUCTORS

        public NewBookingViewModel() : this(new BookingRepo())
        {
        }

        public NewBookingViewModel(IBooking bookingRepo)
        {
            _bookingRepo = bookingRepo;
            ConfirmBookingCommand = new RelayCommand(CreateBooking);
        }

        // METHODS

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

                // STEP 2: CREATE AND VALIDATE GUEST
                var guest = new Guest(
                    firstName: FirstName,
                    lastName: LastName,
                    email: Email,
                    phoneNumber: PhoneNumber,
                    country: Country,
                    passportNumber: PassportNumber
                );

                var guestErrors = guest.Validate();

                if (guestErrors.Any())
                {
                    throw new ArgumentException(guestErrors.First());
                }

                // STEP 3: VALIDATE ROOM SELECTION
                int selectedRoomID = 1;

                // STEP 4: CREATE BOOKING
                var booking = new Booking
                {
                    StartDate = CheckInDate.Value,
                    EndDate = CheckOutDate.Value,
                    Status = BookingStatus.Pending,
                    RoomID = selectedRoomID,
                    GuestID = 1
                };

                // STEP 5: SAVE TO REPOSITORY
                _bookingRepo.Create(booking);

                // STEP 6: SUCCESS - Display message
                ErrorMessage = $"✅ Booking #{booking.BookingID} created successfully for {guest.FirstName} {guest.LastName}!";

                ClearForm();
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
        }
    }
}