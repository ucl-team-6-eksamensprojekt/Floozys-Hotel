using Floozys_Hotel.Core;
using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories;
using System.Collections.ObjectModel;

namespace Floozys_Hotel.ViewModels
{
    class RoomOverviewViewModel : ObservableObject
    {
        private readonly RoomRepo _roomRepo;

        public ObservableCollection<Room> Rooms { get; set; }

        public RoomOverviewViewModel()
        {
            _roomRepo = new RoomRepo();
            Rooms = new ObservableCollection<Room>();
            LoadRooms();
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
    }
}
