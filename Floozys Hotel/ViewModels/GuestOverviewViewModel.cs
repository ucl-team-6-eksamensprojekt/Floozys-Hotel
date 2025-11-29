using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Floozys_Hotel.Commands;
using Floozys_Hotel.Core;

namespace Floozys_Hotel.ViewModels
{
    public class GuestOverviewViewModel : ObservableObject
    {
        public RelayCommand NewGuestCommand { get; set; }

        public GuestOverviewViewModel() 
        {
            NewGuestCommand = new RelayCommand
                (n => OpenNewGuest());
        }

        private void OpenNewGuest()
        {
            throw new NotImplementedException();
        }
    }
}
