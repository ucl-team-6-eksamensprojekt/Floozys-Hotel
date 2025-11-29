using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Floozys_Hotel.Commands;
using Floozys_Hotel.Core;
using Floozys_Hotel.Models;

namespace Floozys_Hotel.ViewModels
{
    public class GuestOverviewViewModel : ObservableObject
    {
        public RelayCommand NewGuestCommand { get; set; }
        public RelayCommand EditGuestCommand { get; set; }

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
            set { _selectedGuest = value; OnPropertyChanged(); }
        }

        public GuestOverviewViewModel()
        {
            // Dummy data for demonstration
            Guests = new ObservableCollection<Guest>
            {
                new Guest { FirstName = "Anna", LastName = "Smith", Phone = "+4512345678", Email = "anna.smith@mail.com", Country = "Denmark" },
                new Guest { FirstName = "John", LastName = "Doe", Phone = "+4598765432", Email = "john.doe@mail.com", Country = "Sweden" }
            };

            NewGuestCommand = new RelayCommand(n => OpenNewGuest());
            EditGuestCommand = new RelayCommand(e => OpenEditGuest(), e => SelectedGuest != null);
        }

        private void OpenNewGuest()
        {
            // Open new guest window/dialog
            // Example: var window = new NewGuestView(); window.Show();
        }

        private void OpenEditGuest()
        {
            // Open edit guest window/dialog for SelectedGuest
            // Example: var window = new EditGuestView(SelectedGuest); window.Show();
        }
    }
}
