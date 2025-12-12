using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Floozys_Hotel.Commands;
using Floozys_Hotel.Models;
using Floozys_Hotel.Core;
using Floozys_Hotel.Repositories;
using Floozys_Hotel.Repositories.Interfaces;

namespace Floozys_Hotel.ViewModels
{
    public class NewGuestViewModel : ObservableObject
    {
        // Private fields
        private readonly IGuestRepo _guestRepo;            // Repository as interface (best for testability)
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _phoneNumber = string.Empty;
        private string _email = string.Empty;
        private string _country = string.Empty;
        private string _passportNumber = string.Empty;


        // Public properties
       

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value ?? string.Empty; ValidateFirstName(); UpdateCanSave(); OnPropertyChanged();
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value ?? string.Empty; ValidateLastName(); UpdateCanSave(); OnPropertyChanged();
            }
        }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value ?? string.Empty; ValidatePhoneNumber(); UpdateCanSave(); OnPropertyChanged();
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value ?? string.Empty; ValidateEmail(); UpdateCanSave(); OnPropertyChanged();
            }
        }
        public string Country
        {
            get => _country;
            set
            {
                _country = value ?? string.Empty; ValidateCountry(); UpdateCanSave(); OnPropertyChanged();
            }
        }
        public string PassportNumber
        {
            get => _passportNumber;
            set
            {
                _passportNumber = value ?? string.Empty; OnPropertyChanged();
            }
        }

        // Error fields and properties for inline validation
        private string _firstNameError = string.Empty;
        public string FirstNameError
        {
            get => _firstNameError;
            set { _firstNameError = value; OnPropertyChanged(); }
        }
       
        private string _lastNameError = string.Empty;
        public string LastNameError
        {
            get => _lastNameError;
            set { _lastNameError = value; OnPropertyChanged(); }
        }

        private string _phoneNumberError = string.Empty;
        public string PhoneNumberError
        {
            get => _phoneNumberError;
            set { _phoneNumberError = value; OnPropertyChanged(); }
        }

        private string _emailError = string.Empty;
        public string EmailError
        {
            get => _emailError;
            set { _emailError = value; OnPropertyChanged(); }
        }

        private string _countryError = string.Empty;
        public string CountryError
        {
            get => _countryError;
            set { _countryError = value; OnPropertyChanged(); }
        }


       
        public Action<Guest> OnSave;
        private readonly bool _isEdit;
        private readonly int _guestId;


        private bool _canSave = false;
        public bool CanSave
        {
            get => _canSave;
            set { _canSave = value; OnPropertyChanged(); }
        }

        public string WindowTitle { get; }
        public string SaveButtonText { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // Constructor
        public NewGuestViewModel(Guest guest = null, bool isEdit = false, IGuestRepo guestRepo = null)
        {
            _guestRepo = guestRepo ?? new GuestRepo();
            
            _isEdit = isEdit;

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
                _guestId = guest.GuestID;
            }

            SaveCommand = new RelayCommand(_ => SaveGuest(), _ => CanSave);
            CancelCommand = new RelayCommand(_ => Cancel());
            ValidateAll();
            UpdateCanSave();
        }

        // EVENTS for the View (UI-related events!)
        public event Action<string, string> ShowError;
        public event Action<string, string> ShowInfo;
        public event Func<string, string, MessageBoxButton, MessageBoxResult> ShowConfirmation;
        public event Action RequestClose;

        // Validation methods
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

            if (_isEdit)
                guest.GuestID = _guestId;

            var errors = guest.Validate();
            if (errors.Any())
            {
                ShowError?.Invoke(string.Join("\n", errors), "Validation Error");
                return;
            }
            var result = ShowConfirmation?.Invoke("Are you sure you want to save the changes?", "Confirm", MessageBoxButton.YesNo) ?? MessageBoxResult.No;
            if (result != MessageBoxResult.Yes)
                return;

            if (_isEdit)
                _guestRepo.UpdateGuest(guest);
            else
                guest.GuestID = _guestRepo.AddGuest(guest);

            OnSave?.Invoke(guest);
            ShowInfo?.Invoke("Saved successfully!", "Saved");
            RequestClose?.Invoke();
        }

        private void Cancel()
        {
            RequestClose?.Invoke();
        }

    }
}