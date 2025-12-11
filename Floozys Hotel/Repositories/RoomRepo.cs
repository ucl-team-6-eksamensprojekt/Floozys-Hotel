using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Floozys_Hotel.Repositories
{
    public class RoomRepo : IRoomRepo
    {
        private readonly string _connectionString;

        public RoomRepo()
        {
            _connectionString = Database.DatabaseConfig.ConnectionString;
        }


        // CREATE
        public void CreateRoom(Room room)
        {
            var errors = room.Validate();
            if (errors.Any())
                throw new ArgumentException(string.Join(Environment.NewLine, errors));

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var sql = "uspCreateRoom";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                    cmd.Parameters.AddWithValue("@Floor", room.Floor);
                    cmd.Parameters.AddWithValue("@RoomSize", room.RoomSize);
                    cmd.Parameters.AddWithValue("@Capacity", room.Capacity);
                    cmd.Parameters.AddWithValue("@Status", (int)room.Status);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // READ
        public List<Room> GetAll()  // ✅ Implement interface method
        {
            var rooms = new List<Room>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var sql = "uspGetAllRooms";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var room = new Room
                            {
                                RoomId = reader.GetInt32(reader.GetOrdinal("RoomID")),
                                RoomNumber = reader.GetString(reader.GetOrdinal("RoomNumber")),
                                Floor = reader.GetInt32(reader.GetOrdinal("Floor")),
                                RoomSize = reader.GetString(reader.GetOrdinal("RoomSize")),
                                Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")),
                                Status = (RoomStatus)reader.GetInt32(reader.GetOrdinal("Status"))
                            };

                            rooms.Add(room);
                        }
                    }
                }
            }

            return rooms;
        }



        public Room? GetById(int roomId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var sql = "uspGetRoomById";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoomID", roomId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        
                            return null;

                        var room = new Room
                        {
                            RoomId = reader.GetInt32(reader.GetOrdinal("RoomID")),
                            RoomNumber = reader.GetString(reader.GetOrdinal("RoomNumber")),
                            Floor = reader.GetInt32(reader.GetOrdinal("Floor")),
                            RoomSize = reader.GetString(reader.GetOrdinal("RoomSize")),
                            Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")),
                            Status = (RoomStatus)reader.GetInt32(reader.GetOrdinal("Status"))
                        };

                        return room;

                    }
                }
            }
        }

        public List<Room> GetRoomsFromCriteria(int? floor, string roomSize, RoomStatus? status)
        {
            var rooms = new List<Room>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var sql = "uspGetRoomsFromCriteria";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Floor", floor.HasValue ? (object)floor.Value : DBNull.Value);

                    cmd.Parameters.AddWithValue("@RoomSize", !string.IsNullOrWhiteSpace(roomSize) ? (object)roomSize : DBNull.Value);

                    cmd.Parameters.AddWithValue("@Status", status.HasValue ? (object)(int)status.Value : DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var room = new Room
                            {
                                RoomId = reader.GetInt32(reader.GetOrdinal("RoomID")),
                                RoomNumber = reader.GetString(reader.GetOrdinal("RoomNumber")),
                                Floor = reader.GetInt32(reader.GetOrdinal("Floor")),
                                RoomSize = reader.GetString(reader.GetOrdinal("RoomSize")),
                                Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")),
                                Status = (RoomStatus)reader.GetInt32(reader.GetOrdinal("Status"))
                            };

                            rooms.Add(room);
                        }
                    }
                }
            }
            return rooms;
        }

        // UPDATE
        public void UpdateRoom(Room room)
        {
            var errors = room.Validate();
            if (errors.Any())
                throw new ArgumentException(string.Join(Environment.NewLine, errors));

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var sql = "uspUpdateRoom";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RoomID", room.RoomId);
                    cmd.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                    cmd.Parameters.AddWithValue("@Floor", room.Floor);
                    cmd.Parameters.AddWithValue("@RoomSize", room.RoomSize);
                    cmd.Parameters.AddWithValue("@Capacity", room.Capacity);
                    cmd.Parameters.AddWithValue("@Status", (int)room.Status);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // DELETE
        public void DeleteRoom(int roomId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var sql = "uspDeleteRoom";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoomID", roomId);

                    cmd.ExecuteNonQuery();
                }
            }

        }


        // Method to get all rooms by availability
        public List<Room> GetAllByAvailability()
        {
            var allRooms = GetAll();
            return allRooms.Where(r => r.Status == RoomStatus.Available).ToList();
        }

    }
}