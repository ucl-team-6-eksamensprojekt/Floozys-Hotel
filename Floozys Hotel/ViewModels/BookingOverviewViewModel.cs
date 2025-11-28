using Floozys_Hotel.Commands;
using Floozys_Hotel.Core;
using Floozys_Hotel.Views;

namespace Floozys_Hotel.ViewModels
{
    class BookingOverviewViewModel : ObservableObject
    {

        public RelayCommand NewBookingCommand { get; set; }

        public BookingOverviewViewModel()
        {
            NewBookingCommand = new RelayCommand
                (n => OpenNewBooking());
        }

        private void OpenNewBooking() 
        {
            var window = new NewBookingView();
            window.Show();
        }

    }
}
