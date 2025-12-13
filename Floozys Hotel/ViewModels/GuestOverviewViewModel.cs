using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Floozys_Hotel.Commands;
using Floozys_Hotel.Core;
using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories;
using Floozys_Hotel.Repositories.Interfaces;
using Floozys_Hotel.Views;

namespace Floozys_Hotel.ViewModels
{
    public class GuestOverviewViewModel : ObservableObject
    {
        // Fields and properties
        private readonly IGuestRepo _guestRepo;


        private ObservableCollection<Guest> _guests;
        public ObservableCollection<Guest> Guests
        {
            get => _guests;
            set { _guests = value; OnPropertyChanged(); }
        }


        private Guest _selectedGuest;
        public Guest SelectedGuest
        {
            get => _selectedGuest;
            set { _selectedGuest = value; OnPropertyChanged(); EditGuestCommand?.RaiseCanExecuteChanged(); }
        }


        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set { _isEditing = value; OnPropertyChanged(); }
        }


        private bool _isNewGuest;
        public bool IsNewGuest
        {
            get => _isNewGuest;
            set { _isNewGuest = value; OnPropertyChanged(); }
        }


        public RelayCommand NewGuestCommand { get; set; }
        public RelayCommand EditGuestCommand { get; set; }
        public RelayCommand ClearGuestCommand { get; set; }
       

        // Events for View
        public event Action<Guest> EditGuestRequested;
        public event Action NewGuestRequested;
        public event Action<string, string> ShowInfoDialog;


        // Constructor
        public GuestOverviewViewModel()
        {
            // Dummy data for demonstration - TODO: Delete when we have guests from Database
            Guests = new ObservableCollection<Guest>
            {
                new Guest { FirstName = "Anna", LastName = "Smith", PhoneNumber = "+4512345678", Email = "anna.smith@mail.com", Country = "Denmark" },
                new Guest { FirstName = "John", LastName = "Doe", PhoneNumber = "+4598765432", Email = "john.doe@mail.com", Country = "Sweden" },
                new Guest { FirstName = "Sreymom", LastName = "Sok", PhoneNumber = "+85593847584", Email = "sreysok@hotmail.com", Country = "Cambodia", PassportNumber = "N83749573"}
            };

            _guestRepo = new GuestRepo();

            NewGuestCommand = new RelayCommand(_ => OnNewGuest());
            EditGuestCommand = new RelayCommand(_ => OnEditGuest(), _ => SelectedGuest != null);
            ClearGuestCommand = new RelayCommand(_ => ClearGuest());
                        
            IsEditing = false;
            IsNewGuest = false;
        }


        // Methods
        private void OnNewGuest()
        {
            NewGuestRequested?.Invoke();
        }


        private void OnEditGuest()
        {
            if (SelectedGuest == null)
            {
                ShowInfoDialog?.Invoke("You must select a guest to edit.", "No guest selected");
                return;
            }
            // Make a copy, so we don't change directly
            var guestCopy = new Guest(
                SelectedGuest.FirstName,
                SelectedGuest.LastName,
                SelectedGuest.Email,
                SelectedGuest.PhoneNumber,
                SelectedGuest.Country,
                SelectedGuest.PassportNumber
            );
            EditGuestRequested?.Invoke( guestCopy );
        }


        private void ClearGuest()
        {
            SelectedGuest = new Guest();
            IsEditing = false;
            IsNewGuest = false;
        }


        // Method to receive new/updated guest (called from View after dialog)
        public void AddGuestToOverview(Guest newGuest)
        {
            Guests.Add(newGuest);
        }


        public void UpdateSelectedGuest(Guest editedGuest)
        {
            SelectedGuest.FirstName = editedGuest.FirstName;
            SelectedGuest.LastName = editedGuest.LastName;
            SelectedGuest.Email = editedGuest.Email;
            SelectedGuest.PhoneNumber = editedGuest.PhoneNumber;
            SelectedGuest.Country = editedGuest.Country;
            SelectedGuest.PassportNumber = editedGuest.PassportNumber;

            OnPropertyChanged(nameof(Guests)); // Opdate in special edge cases
        }
    }
}

