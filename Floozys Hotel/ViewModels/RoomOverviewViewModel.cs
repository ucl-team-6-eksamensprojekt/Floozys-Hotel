using Floozys_Hotel.Commands;
using Floozys_Hotel.Core;
using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories;
using Floozys_Hotel.ViewModels.FormsViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace Floozys_Hotel.ViewModels
{
    class RoomOverviewViewModel
    {
        private readonly RoomRepo _roomRepo;

        public ObservableCollection<Room> Rooms { get; set; }



        private bool _isRoomFormOpen;
        public bool IsRoomFormOpen
        {
            get => _isRoomFormOpen;
            set
            {
                if (_isRoomFormOpen != value)
                {
                    _isRoomFormOpen = value;
                    OnPropertyChanged();
                }
            
            }
        }

        private object _currentSideContent;
        public object CurrentSideContent
        {
            get => _currentSideContent;
            set 
            {
                if (_currentSideContent != value)
                {
                    _currentSideContent = value;
                    OnPropertyChanged();
                }
            }
        }




        public ICommand OpenCreateRoomCommand { get; }
        public ICommand OpenEditRoomCommand { get; }
        public ICommand DeleteRoomCommand { get; }


        public RoomOverviewViewModel()
        {
            _roomRepo = new RoomRepo();
            Rooms = new ObservableCollection<Room>();
            LoadRooms();

            OpenCreateRoomCommand = new RelayCommand(_ => OpenCreateRoom());
            OpenEditRoomCommand = new RelayCommand(r => OpenEditRoom(r as Room));
            DeleteRoomCommand = new RelayCommand(r => DeleteRoom(r as Room));

        }

        private void LoadRooms()
        {
            var rooms = _roomRepo.GetAllRooms();
            Rooms.Clear();
            foreach (var room in rooms)
            {
                Rooms.Add(room);
            }
        }

        private void OpenCreateRoom()
        {
            CurrentSideContent = new RoomFormViewModel(
                _roomRepo,
                onSaved: OnRoomSaved,
                onCancel: ClosePanel,
                existingRoom: null);

            IsRoomFormOpen = true;
        }

        private void OpenEditRoom(Room room)
        {
            if (room == null) return;

            CurrentSideContent = new RoomFormViewModel(
                _roomRepo,
                onSaved: OnRoomSaved,
                onCancel: ClosePanel,
                existingRoom: room);

            IsRoomFormOpen = true;
        }

        

        private void DeleteRoom(Room room)
        {
            if (room == null) return;

            _roomRepo.DeleteRoom(room.RoomId);
            Rooms.Remove(room);
        }

        private void OnRoomSaved()
        {
            LoadRooms();
            ClosePanel();
        }

        private void ClosePanel()
        {
            IsRoomFormOpen = false;
            CurrentSideContent = null;
        }
    }
}
