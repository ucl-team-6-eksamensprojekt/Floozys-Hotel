using Floozys_Hotel.Commands;
using Floozys_Hotel.Core;
using Floozys_Hotel.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floozys_Hotel.ViewModels
{
    class NewBookingViewModel : ObservableObject
    {
        // Date Properties
        private DateTime? _checkInDate;
        public DateTime? CheckInDate
        {
            get => _checkInDate;
            set
            {
                _checkInDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _checkOutDate;
        public DateTime? CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                _checkOutDate = value;
                OnPropertyChanged();
            }
        }

        // Guest Information
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _passportNumber;
        public string PassportNumber
        {
            get => _passportNumber;
            set
            {
                _passportNumber = value;
                OnPropertyChanged();
            }
        }

        private string _phoneNumber;
        public string PhoneNumber  // Stored as string to preserve formatting (+45, spaces)
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                _country = value;
                OnPropertyChanged();
            }
        }

        // Error Handling
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public RelayCommand ConfirmBookingCommand { get; set; }

        // Constructor
        public NewBookingViewModel()
        {
            ConfirmBookingCommand = new RelayCommand(
                execute: o => ConfirmBooking(),
                canExecute: o => CanConfirmBooking()
            );
        }

        // Command Methods
        private bool CanConfirmBooking()
        {
            // Button is always enabled for now
            return true;
        }

        private void ConfirmBooking()
        {
            try
            {
                // TODO: Validate dates
                // TODO: Create Guest object
                // TODO: Create Booking object
                // TODO: Save to database via Repository

                ErrorMessage = "Booking functionality not yet implemented";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }

    }
}
