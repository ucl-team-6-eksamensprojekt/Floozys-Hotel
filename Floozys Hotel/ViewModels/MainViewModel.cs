using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Floozys_Hotel.Commands;
using Floozys_Hotel.Core;

namespace Floozys_Hotel.ViewModels
{
    class MainViewModel : ObservableObject
    {

        public RelayCommand BookingOverviewViewCommand { get; set; }
        public RelayCommand GuestOverviewViewCommand { get; set; }
        public RelayCommand RoomOverviewViewCommand { get; set; }
        public RelayCommand SalesOverviewViewCommand { get; set; }
        public RelayCommand GuestPolicyViewCommand { get; set; }



        public BookingOverviewViewModel BookingVM { get; set; }
        public GuestOverviewViewModel GuestVM { get; set; }
        public RoomOverviewViewModel RoomVM { get; set; }
        public SalesOverviewViewModel SalesVM { get; set; }
        public GuestPolicyViewModel PolicyVM { get; set; }


        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            BookingVM = new BookingOverviewViewModel();
            GuestVM = new GuestOverviewViewModel();
            RoomVM = new RoomOverviewViewModel();
            SalesVM = new SalesOverviewViewModel();
            PolicyVM = new GuestPolicyViewModel();
            CurrentView = BookingVM;

            BookingOverviewViewCommand = new RelayCommand(o =>
            {
                CurrentView = BookingVM;
            });

            GuestOverviewViewCommand = new RelayCommand(o =>
            {
                CurrentView = GuestVM;
            });

            RoomOverviewViewCommand = new RelayCommand(o =>
            {
                CurrentView = RoomVM;
            });

            SalesOverviewViewCommand = new RelayCommand(o =>
            {
                CurrentView = SalesVM;
            });

            GuestPolicyViewCommand = new RelayCommand(o =>
            {
                CurrentView = PolicyVM;
            });
        }
    }
}
