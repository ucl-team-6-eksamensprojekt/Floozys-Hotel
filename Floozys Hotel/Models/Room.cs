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
    public class Room : ObservableObject
    {
        private int _roomId { get; set; }
        private string _roomNumber { get; set; } = "";
        private int _floor { get; set; }
        private string _roomSize { get; set; } = "";
        private int _capacity { get; set; }
        private RoomStatus _status = RoomStatus.Available;


        /* 
            Using => instead of return is just simpler
        */

        public int RoomId
        {
            get => _roomId;
            set { _roomId = value; OnPropertyChanged(); }
        }

        public string RoomNumber
        {
            get => _roomNumber;
            set { _roomNumber = value; OnPropertyChanged(); }
        }

        public int Floor
        {
            get => _floor;
            set { _floor = value; OnPropertyChanged(); }
        }

        public string RoomSize
        {
            get => _roomSize;
            set { _roomSize = value; OnPropertyChanged(); }
        }

        public int Capacity
        {
            get => _capacity;
            set { _capacity = value; OnPropertyChanged(); }
        }
        
        public RoomStatus Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public void ValidateForCreate()
        {
            if (Floor < 0)
                throw new ArgumentException("Floor must be over 0");

            if (string.IsNullOrWhiteSpace(RoomSize))
                throw new ArgumentException("RoomSize is required");

            if (Capacity <= 0)
                throw new ArgumentException("Capacity must be > 0");
        }

        public void ValidateForUpdate() => ValidateForCreate();

    }

}
