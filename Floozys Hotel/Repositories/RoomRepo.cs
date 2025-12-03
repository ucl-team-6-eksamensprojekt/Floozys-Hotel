using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Floozys_Hotel.Models;

namespace Floozys_Hotel.Repositories
{
    public class RoomRepo
    {
        private List<Room> _rooms;

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

        public List<Room> GetAllRooms()
        {
            return _rooms;
        }
    }
}
