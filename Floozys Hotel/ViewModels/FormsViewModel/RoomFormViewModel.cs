using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Floozys_Hotel.Core;
using Floozys_Hotel.Repositories;
using Floozys_Hotel.Models;
using System.Windows.Input;
using Floozys_Hotel.Commands;


namespace Floozys_Hotel.ViewModels.FormsViewModel
{
    public class RoomFormViewModel : ObservableObject
    {
        private readonly RoomRepo _roomRepo;
        private readonly Action _onSaved;
        private readonly Action _onCancel;

        public string Title { get; }
        public string ButtonText { get; }

        public Room Room { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public RoomFormViewModel(RoomRepo roomRepo, Action onSaved, Action onCancel, Room existingRoom)
        {
            _roomRepo = roomRepo;
            _onSaved = onSaved;
            _onCancel = onCancel;

            if (existingRoom == null)
            {
                Title = "Create Room";
                ButtonText = "Create";
                Room = new Room
                {
                    Status = RoomStatus.Available
                };
            }
            else
            {
                Title = "Edit Room";
                ButtonText = "Save";
                Room = existingRoom;
            }

            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => _onCancel());

        }

        private void Save()
        {
            var errors = Room.Validate();
            if (Room.RoomId == 0)
            {
                _roomRepo.CreateRoom(Room);
            }
            else
            {
                _roomRepo.UpdateRoom(Room);
            }

            _onSaved();
        }
    }
}
