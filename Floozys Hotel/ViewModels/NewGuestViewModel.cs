using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Floozys_Hotel.Commands;
using Floozys_Hotel.Models;
using Floozys_Hotel.Core;

namespace Floozys_Hotel.ViewModels
{
    public class NewGuestViewModel : ObservableObject
    {
        public string WindowTitle { get; }
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

        // Inline error properties
        public string FirstNameError { get => _firstNameError; set { _firstNameError = value; OnPropertyChanged(); } }
        public string LastNameError { get => _lastNameError; set { _lastNameError = value; OnPropertyChanged(); } }
        public string PhoneNumberError { get => _phoneNumberError; set { _phoneNumberError = value; OnPropertyChanged(); } }
        public string EmailError { get => _emailError; set { _emailError = value; OnPropertyChanged(); } }
        public string CountryError { get => _countryError; set { _countryError = value; OnPropertyChanged(); } }

        public bool CanSave { get => _canSave; set { _canSave = value; OnPropertyChanged(); } }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private string _firstName = string.Empty, _lastName = string.Empty, _phoneNumber = string.Empty, _email = string.Empty, _country = string.Empty, _passportNumber = string.Empty;
        private string _firstNameError = string.Empty, _lastNameError = string.Empty, _phoneNumberError = string.Empty, _emailError = string.Empty, _countryError = string.Empty;
        private bool _canSave = false;
        private readonly Action<Guest> _onSave;
        private readonly Window _window;

        public NewGuestViewModel(Window window, Guest guest, bool isEdit, Action<Guest> onSave)
        {
            _window = window;
            _onSave = onSave;
            WindowTitle = isEdit ? "Edit Guest" : "New Guest";

            if (guest != null)
            {
                FirstName = guest.FirstName;
                LastName = guest.LastName;
                PhoneNumber = guest.PhoneNumber;
                Email = guest.Email;
                Country = guest.Country;
                PassportNumber = guest.PassportNumber;
            }

            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave);
            CancelCommand = new RelayCommand(_ => Cancel());
            ValidateAll();
            UpdateCanSave();
        }

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

        private void Save()
        {
            var guest = new Guest(FirstName, LastName, Email, PhoneNumber, Country, PassportNumber);
            if (MessageBox.Show("Are you sure you want to save the changes??", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _onSave?.Invoke(guest);
                MessageBox.Show("Succes", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                _window.DialogResult = true;
                _window.Close();
            }
        }

        private void Cancel()
        {
            _window.DialogResult = false;
            _window.Close();
        }
    }
}