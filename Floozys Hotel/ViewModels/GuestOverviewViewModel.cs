using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Floozys_Hotel.Commands;
using Floozys_Hotel.Core;
using Floozys_Hotel.Models;

namespace Floozys_Hotel.ViewModels
{
    public class GuestOverviewViewModel : ObservableObject
    {
        public RelayCommand NewGuestCommand { get; set; }
        public RelayCommand EditGuestCommand { get; set; }

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

        public GuestOverviewViewModel()
        {
            // Dummy data for demonstration
            Guests = new List<Guest>
            {
                new Guest { FirstName = "Anna", LastName = "Smith", PhoneNumber = "+4512345678", Email = "anna.smith@mail.com", Country = "Denmark" },
                new Guest { FirstName = "John", LastName = "Doe", PhoneNumber = "+4598765432", Email = "john.doe@mail.com", Country = "Sweden" }
            };

            NewGuestCommand = new RelayCommand(n => CreateGuest());
            EditGuestCommand = new RelayCommand(e => EditGuest(), e => SelectedGuest != null);
        }

        private void CreateGuest()
        {
            // Open new guest window/dialog
            // Example: var window = new NewGuestView(); window.Show();
        }

        private void EditGuest()
        {
            List<string> errors = _selectedGuest.Validate();
            if (errors.Any())
            {
                string message = string.Join("\n", errors);
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // TODO: Save changes
            }

            // Open edit guest window/dialog for SelectedGuest
            // Example: var window = new EditGuestView(SelectedGuest); window.Show();
        }
    }
}
