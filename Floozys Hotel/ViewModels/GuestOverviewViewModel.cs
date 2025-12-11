using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Floozys_Hotel.Commands;
using Floozys_Hotel.Core;
using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories;
using Floozys_Hotel.Views;

namespace Floozys_Hotel.ViewModels
{
    public class GuestOverviewViewModel : ObservableObject
    {
        public RelayCommand NewGuestCommand { get; set; }
        public RelayCommand EditGuestCommand { get; set; }
        public RelayCommand SaveGuestCommand { get; set; }
        public RelayCommand ClearGuestCommand { get; set; }

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

        private List<Guest> _guests;
        public List<Guest> Guests
        {
            get => _guests;
            set { _guests = value; OnPropertyChanged(); }
        }

        private Guest _selectedGuest;
        public Guest SelectedGuest
        {
            get => _selectedGuest;
            set { _selectedGuest = value; OnPropertyChanged(); }
        }

        private readonly GuestRepo _guestRepo;
        
        public GuestOverviewViewModel()
        {
            // Dummy data for demonstration
            Guests = new List<Guest>
            {
                new Guest { FirstName = "Anna", LastName = "Smith", PhoneNumber = "+4512345678", Email = "anna.smith@mail.com", Country = "Denmark" },
                new Guest { FirstName = "John", LastName = "Doe", PhoneNumber = "+4598765432", Email = "john.doe@mail.com", Country = "Sweden" },
                new Guest { FirstName = "Sreymom", LastName = "Sok", PhoneNumber = "+85593847584", Email = "sreysok@hotmail.com", Country = "Cambodia", PassportNumber = "N83749573"}
            };

            // TODO: Use correct connection string 
            _guestRepo = new GuestRepo();

            NewGuestCommand = new RelayCommand(_ => NewGuest());
            EditGuestCommand = new RelayCommand(_ => EditGuest(), _ => SelectedGuest != null);
            ClearGuestCommand = new RelayCommand(_ => ClearGuest());
                        
            IsEditing = false;
            IsNewGuest = false;
        }

        private void NewGuest()
        {
            var window = new NewGuestView(null, false, guest =>
            {
                Guests.Add(guest);
                Guests = new List<Guest>(Guests); // For at opdatere UI
            });
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }

        private void EditGuest()
        {
            if (SelectedGuest == null)
            {
                MessageBox.Show("You must select a guest to edit.", "No guest selected", MessageBoxButton.OK, MessageBoxImage.Information);
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
            var window = new NewGuestView(guestCopy, true, guest =>
            {
                // Update SelectedGuest with new values
                SelectedGuest.FirstName = guest.FirstName;
                SelectedGuest.LastName = guest.LastName;
                SelectedGuest.Email = guest.Email;
                SelectedGuest.PhoneNumber = guest.PhoneNumber;
                SelectedGuest.Country = guest.Country;
                SelectedGuest.PassportNumber = guest.PassportNumber;
                Guests = new List<Guest>(Guests); // To update UI
            });
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }

        private void ClearGuest()
        {
            SelectedGuest = new Guest();
            IsEditing = false;
            IsNewGuest = false;
        }
    }
}

