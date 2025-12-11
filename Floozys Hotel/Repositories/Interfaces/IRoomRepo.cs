using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Floozys_Hotel.Models;

namespace Floozys_Hotel.Repositories.Interfaces
{
    public interface IRoomRepo
    {
        void CreateRoom(Room room);

        Room? GetById(int roomId);
        List<Room> GetAll();

        List<Room> GetRoomsFromCriteria(int? floor, string? roomSize, RoomStatus? status);

        void UpdateRoom(Room room);

        void DeleteRoom(int roomId);
    }
}
