using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Floozys_Hotel.Core;


namespace Floozys_Hotel.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = "";
        public int Floor { get; set; }
        public string RoomSize { get; set; } = "";
        public int Capacity { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Available;

        public Room()
        {

        }

        public Room(string roomNumber, int floor, string roomSize, int capacity, RoomStatus status)
        {
            RoomNumber = roomNumber;
            Floor = floor;
            RoomSize = roomSize;
            Capacity = capacity;
            Status = status;
        }

        public List<string> Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(RoomNumber))
                errors.Add("Room number is required.");

            if (Floor <= 0)
                errors.Add("Floor must be greater than 0.");

            if (string.IsNullOrWhiteSpace(RoomSize))
                errors.Add("Room size is required.");

            if (Capacity <= 0)
                errors.Add("Capacity must be greater than 0.");

            return errors;
        }

    }

}
