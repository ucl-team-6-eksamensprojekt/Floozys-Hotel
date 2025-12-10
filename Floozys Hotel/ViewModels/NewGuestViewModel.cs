using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Floozys_Hotel.Commands;
using Floozys_Hotel.Models;
using Floozys_Hotel.Core;
using Floozys_Hotel.Repositories;

namespace Floozys_Hotel.ViewModels
{
    public class NewGuestViewModel : ObservableObject
    {
        // Private fields
        private readonly GuestRepo _guestRepo = new GuestRepo();
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _phoneNumber = string.Empty;
        private string _email = string.Empty;
        private string _country = string.Empty;
        private string _passportNumber = string.Empty;

        private string _firstNameError = string.Empty;
        private string _lastNameError = string.Empty;
        private string _phoneNumberError = string.Empty;
        private string _emailError = string.Empty;
        private string _countryError = string.Empty;

        private bool _canSave = false;
        private readonly Action<Guest> _onSave;
        private readonly Window _window;

        // Public properties
        public string WindowTitle { get; }
        public string SaveButtonText { get; }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value ?? string.Empty;
                ValidateFirstName();
                UpdateCanSave();
                OnPropertyChanged();
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value ?? string.Empty;
                ValidateLastName();
                UpdateCanSave();
                OnPropertyChanged();
            }
        }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value ?? string.Empty;
                ValidatePhoneNumber();
                UpdateCanSave();
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value ?? string.Empty;
                ValidateEmail();
                UpdateCanSave();
                OnPropertyChanged();
            }
        }
        public string Country
        {
            get => _country;
            set
            {
                _country = value ?? string.Empty;
                ValidateCountry();
                UpdateCanSave();
                OnPropertyChanged();
            }
        }
        public string PassportNumber
        {
            get => _passportNumber;
            set
            {
                _passportNumber = value ?? string.Empty;
                OnPropertyChanged();
            }
        }

        // Error properties for inline validation
        public string FirstNameError
        {
            get => _firstNameError;
            set { _firstNameError = value; OnPropertyChanged(); }
        }
        public string LastNameError
        {
            get => _lastNameError;
            set { _lastNameError = value; OnPropertyChanged(); }
        }
        public string PhoneNumberError
        {
            get => _phoneNumberError;
            set { _phoneNumberError = value; OnPropertyChanged(); }
        }
        public string EmailError
        {
            get => _emailError;
            set { _emailError = value; OnPropertyChanged(); }
        }
        public string CountryError
        {
            get => _countryError;
            set { _countryError = value; OnPropertyChanged(); }
        }

        public bool CanSave
        {
            get => _canSave;
            set { _canSave = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // Constructor
        public NewGuestViewModel(Window window, Guest guest, bool isEdit, Action<Guest> onSave)
        {
            _window = window;
            _onSave = onSave;
            WindowTitle = isEdit ? "Edit Guest" : "New Guest";
            SaveButtonText = isEdit ? "Save" : "Create";

            if (guest != null)
            {
                FirstName = guest.FirstName;
                LastName = guest.LastName;
                PhoneNumber = guest.PhoneNumber;
                Email = guest.Email;
                Country = guest.Country;
                PassportNumber = guest.PassportNumber;
            }

            SaveCommand = new RelayCommand(_ => SaveGuest(), _ => CanSave);
            CancelCommand = new RelayCommand(_ => Cancel());
            ValidateAll();
            UpdateCanSave();
        }

        // Methods
        private void ValidateAll()
        {
            ValidateFirstName();
            ValidateLastName();
            ValidatePhoneNumber();
            ValidateEmail();
            ValidateCountry();
        }

        private void ValidateFirstName()
        {
            FirstNameError = string.IsNullOrWhiteSpace(FirstName) ? "First name is required." : string.Empty;
        }
        private void ValidateLastName()
        {
            LastNameError = string.IsNullOrWhiteSpace(LastName) ? "Last name is required." : string.Empty;
        }
        private void ValidatePhoneNumber()
        {
            PhoneNumberError = string.IsNullOrWhiteSpace(PhoneNumber) ? "Phone number is required." : string.Empty;
        }
        private void ValidateEmail()
        {
            EmailError = (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@") || !Email.Contains("."))
                ? "A valid email is required." : string.Empty;
        }
        private void ValidateCountry()
        {
            CountryError = string.IsNullOrWhiteSpace(Country) ? "Country is required." : string.Empty;
        }

        private void UpdateCanSave()
        {
            CanSave = string.IsNullOrEmpty(FirstNameError)
                && string.IsNullOrEmpty(LastNameError)
                && string.IsNullOrEmpty(PhoneNumberError)
                && string.IsNullOrEmpty(EmailError)
                && string.IsNullOrEmpty(CountryError);
        }

        private void SaveGuest()
        {
            var guest = new Guest(FirstName, LastName, Email, PhoneNumber, Country, PassportNumber);

            // Validate all fields via the model class's Validate method
            var errors = guest.Validate();
            if (errors.Any())
            {
                MessageBox.Show(string.Join("\n", errors), "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Confirm with the user before saving
            var result = MessageBox.Show("Are you sure you want to save the changes??", "Confirm", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
                return;

            // Save in the Database
            if (guest.GuestID == 0)
            {
                guest.GuestID = _guestRepo.AddGuest(guest);
            }
            else
            {
                _guestRepo.UpdateGuest(guest);
            }

            _onSave?.Invoke(guest);
            MessageBox.Show("Succes", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            _window.DialogResult = true;
            _window.Close();
        }

        private void Cancel()
        {
            _window.DialogResult = false;
            _window.Close();
        }
    }
}