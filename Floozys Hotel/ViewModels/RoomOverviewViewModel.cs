using Floozys_Hotel.Commands;
using Floozys_Hotel.Core;
using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories;
using Floozys_Hotel.ViewModels.FormsViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;


namespace Floozys_Hotel.ViewModels
{
    class RoomOverviewViewModel : ObservableObject
    {
        private readonly RoomRepo _roomRepo;

        public ObservableCollection<Room> Rooms { get; set; }
        public ObservableCollection<int> Floors { get; } = new();
        public ObservableCollection<string> RoomSizes { get; } = new();
        public ObservableCollection<RoomStatus> RoomStatuses { get; } = new();

        private int? _selectedFloor;
        public int? SelectedFloor
        {
            get => _selectedFloor;
            set
            {
                if (_selectedFloor != value)
                {
                    _selectedFloor = value;
                    OnPropertyChanged();
                }
            }
        }

        private string? _selectedRoomSize;
        public string? SelectedRoomSize
        {
            get => _selectedRoomSize;
            set
            {
                if (_selectedRoomSize != value)
                {
                    _selectedRoomSize = value;
                    OnPropertyChanged();
                }
            }
        }

        private RoomStatus? _selectedStatus;
        public RoomStatus? SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if(_selectedStatus != value)
                {
                    _selectedStatus = value;
                    OnPropertyChanged();
                }
            }
        }


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



        public ICommand ApplyFilterCommand { get; }
        public ICommand ClearFilterCommand { get; }
        public ICommand OpenCreateRoomCommand { get; }
        public ICommand OpenEditRoomCommand { get; }
        public ICommand DeleteRoomCommand { get; }


        public RoomOverviewViewModel()
        {
            _roomRepo = new RoomRepo();
            Rooms = new ObservableCollection<Room>();
            LoadRooms();
            LoadFilterOptions();

            ApplyFilterCommand = new RelayCommand(_ => ApplyFilter());
            ClearFilterCommand = new RelayCommand(_ => ClearFilter());
            OpenCreateRoomCommand = new RelayCommand(_ => OpenCreateRoom());
            OpenEditRoomCommand = new RelayCommand(r => OpenEditRoom(r as Room));
            DeleteRoomCommand = new RelayCommand(r => DeleteRoom(r as Room));

        }

        private void LoadRooms()
        {
            var rooms = _roomRepo.GetAll();
            Rooms.Clear();
            foreach (var room in rooms)
            {
                Rooms.Add(room);
            }
        }

        private void LoadFilterOptions()
        {
            Floors.Clear();
            RoomSizes.Clear();
            RoomStatuses.Clear();

            var rooms = _roomRepo.GetAll();

            foreach (var floor in rooms.Select(r => r.Floor).Distinct().OrderBy(f => f))
                Floors.Add(floor);

            foreach (var roomSize in rooms.Select(r => r.RoomSize).Distinct().OrderBy(f => f))
                RoomSizes.Add(roomSize);

            foreach (var roomStatus in rooms.Select(r => r.Status).Distinct().OrderBy(f => f))
                RoomStatuses.Add(roomStatus);
        }

        public void ApplyFilter()
        {
            var rooms = _roomRepo.GetRoomsFromCriteria
                (
                SelectedFloor,
                string.IsNullOrWhiteSpace(SelectedRoomSize) ? null : SelectedRoomSize,
                SelectedStatus
                );

            Rooms.Clear();
            foreach (var room in rooms)
                Rooms.Add(room);
            
        }

        private void ClearFilter()
        {
            SelectedFloor = null;
            SelectedRoomSize = null;
            SelectedStatus = null;

            LoadRooms();
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

            var result = System.Windows.MessageBox.Show($"Are you sure you want to delete room {room.RoomNumber}?\n\n" + "This can disrupt logged bookings ", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                _roomRepo.DeleteRoom(room.RoomId);
                Rooms.Remove(room);
            }
            catch (InvalidOperationException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Unable to delete room", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }

        }

        private void OnRoomSaved()
        {
            LoadRooms();
            LoadFilterOptions();
            ClosePanel();
        }

        private void ClosePanel()
        {
            IsRoomFormOpen = false;
            CurrentSideContent = null;
        }
    }
}
