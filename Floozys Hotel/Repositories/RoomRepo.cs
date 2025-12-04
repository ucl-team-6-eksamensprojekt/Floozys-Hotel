using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using static Azure.Core.HttpHeader;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Floozys_Hotel.Repositories
{
    public class RoomRepo : IRoom
    {
        private List<Room> _rooms;  // In-memory storage simulates database

        public RoomRepo()
        {
            _rooms = new List<Room>
            {
                new Room { RoomId = 1, RoomNumber = "101", Floor = 1, RoomSize = "Small", Capacity = 2, Status = RoomStatus.Available },
                new Room { RoomId = 2, RoomNumber = "102", Floor = 1, RoomSize = "Large", Capacity = 4, Status = RoomStatus.Available },
                new Room { RoomId = 3, RoomNumber = "103", Floor = 1, RoomSize = "Small", Capacity = 2, Status = RoomStatus.Available },
                new Room { RoomId = 4, RoomNumber = "201", Floor = 2, RoomSize = "Large", Capacity = 4, Status = RoomStatus.Available },
                new Room { RoomId = 5, RoomNumber = "202", Floor = 2, RoomSize = "Small", Capacity = 2, Status = RoomStatus.Available },
            };
        }

        // CREATE
        public void CreateRoom(Room room)
        {
            if (room == null)
            {
                throw new ArgumentNullException(nameof(room), "Room cannot be null");
            }

            _rooms.Add(room);
        }

        // READ
        public List<Room> GetAll()  // ✅ Implement interface method
        {
            return new List<Room>(_rooms);  // Return copy to prevent external modification
        }

        public Room GetById(int roomId)
        {
            return _rooms.FirstOrDefault(r => r.RoomId == roomId);
        }

        public List<Room> GetRoomsFromCriteria(int? floor, string roomSize, RoomStatus? status)
        {
            return _rooms.Where(r =>
                (!floor.HasValue || r.Floor == floor.Value) &&
                (string.IsNullOrEmpty(roomSize) || r.RoomSize == roomSize) &&
                (!status.HasValue || r.Status == status.Value)
            ).ToList();
        }

        // UPDATE
        public void UpdateRoom(Room room)
        {
            if (room == null)
            {
                throw new ArgumentNullException(nameof(room), "Room cannot be null");
            }

            var existingRoom = _rooms.FirstOrDefault(r => r.RoomId == room.RoomId);

            if (existingRoom == null)
            {
                throw new ArgumentException($"Room with ID {room.RoomId} not found");
            }

            existingRoom.RoomNumber = room.RoomNumber;
            existingRoom.Floor = room.Floor;
            existingRoom.RoomSize = room.RoomSize;
            existingRoom.Capacity = room.Capacity;
            existingRoom.Status = room.Status;
        }

        // DELETE
        public bool DeleteRoom(int roomId)
        {
            var room = _rooms.FirstOrDefault(r => r.RoomId == roomId);

            if (room == null)
            {
                return false;
            }

            _rooms.Remove(room);
            return true;
        }
    }
}