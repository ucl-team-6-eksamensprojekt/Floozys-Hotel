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


        private ObservableCollection<Guest> _guests = new ObservableCollection<Guest>();
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
        public RelayCommand CreateBookingForGuestCommand { get; set; }
       

        // Events for View
        public event Action<Guest> EditGuestRequested;
        public event Action NewGuestRequested;
        public event Action<string, string> ShowInfoDialog;
        public event Action<Guest> BookingRequestedForGuest;


        // Constructor
        public GuestOverviewViewModel()
        {
            _guestRepo = new GuestRepo();
            LoadGuests();

            NewGuestCommand = new RelayCommand(_ => OnNewGuest());
            EditGuestCommand = new RelayCommand(_ => OnEditGuest(), _ => SelectedGuest != null);
            ClearGuestCommand = new RelayCommand(_ => ClearGuest());
            CreateBookingForGuestCommand = new RelayCommand(_ => OnBookingRequestedForGuest(), _ => SelectedGuest != null);

            IsEditing = false;
            IsNewGuest = false;
        }

        private void LoadGuests()
        {
            var guests = _guestRepo.GetAll();
            Guests.Clear();
            foreach (var guest in guests)
            {
                Guests.Add(guest);
            }
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
            EditGuestRequested?.Invoke(guestCopy);
        }


        private void ClearGuest()
        {
            SelectedGuest = new Guest();
            IsEditing = false;
            IsNewGuest = false;
        }

        private void OnBookingRequestedForGuest()
        {
            if (SelectedGuest != null)
            {
                BookingRequestedForGuest?.Invoke( SelectedGuest );
            }
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

