using Microsoft.Data.SqlClient;
using Floozys_Hotel.Models;
using System;
using System.Collections.Generic;

namespace Floozys_Hotel.Repositories
{
    public class RoomRepo
    {
        private readonly string _connectionString;

        public RoomRepo()
        {
            _connectionString = Database.DatabaseConfig.ConnectionString;
        }

        /// <summary>
        /// Henter alle værelser fra databasen.
        /// </summary>
        public List<Room> GetAllRooms()
        {
            var rooms = new List<Room>();

            using (var connection = new SqlConnection(_connectionString))
            {
               connection.Open();
                
                string query = "SELECT RoomID, RoomNumber, Floor, RoomSize, Capacity, Status FROM ROOM";
                
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rooms.Add(new Room
                        {
                            RoomId = reader.GetInt32(0),
                            RoomNumber = reader.GetInt32(1).ToString(),
                            Floor = reader.GetInt32(2),
                            RoomSize = reader.GetString(3),
                            Capacity = reader.GetInt32(4),
                            Status = (RoomStatus)reader.GetInt32(5)
                        });
                    }
                }
            }

            return rooms;
        }

        /// <summary>
        /// Henter et enkelt værelse baseret på RoomID.
        /// </summary>
        public Room? GetRoomById(int roomId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                string query = "SELECT RoomID, RoomNumber, Floor, RoomSize, Capacity, Status FROM ROOM WHERE RoomID = @RoomID";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomID", roomId);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Room
                            {
                                RoomId = reader.GetInt32(0),
                                RoomNumber = reader.GetInt32(1).ToString(),
                                Floor = reader.GetInt32(2),
                                RoomSize = reader.GetString(3),
                                Capacity = reader.GetInt32(4),
                                Status = (RoomStatus)reader.GetInt32(5)
                            };
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Tilføjer et nyt værelse til databasen.
        /// </summary>
        public int AddRoom(Room room)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                string query = @"INSERT INTO ROOM (RoomNumber, Floor, RoomSize, Capacity, Status) 
                                VALUES (@RoomNumber, @Floor, @RoomSize, @Capacity, @Status);
                                SELECT CAST(SCOPE_IDENTITY() as int);";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomNumber", int.Parse(room.RoomNumber));
                    command.Parameters.AddWithValue("@Floor", room.Floor);
                    command.Parameters.AddWithValue("@RoomSize", room.RoomSize);
                    command.Parameters.AddWithValue("@Capacity", room.Capacity);
                    command.Parameters.AddWithValue("@Status", (int)room.Status);
                    
                    return (int)command.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Opdaterer et eksisterende værelse.
        /// </summary>
        public void UpdateRoom(Room room)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                string query = @"UPDATE ROOM 
                                SET RoomNumber = @RoomNumber, Floor = @Floor, RoomSize = @RoomSize, 
                                    Capacity = @Capacity, Status = @Status
                                WHERE RoomID = @RoomID";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomID", room.RoomId);
                    command.Parameters.AddWithValue("@RoomNumber", int.Parse(room.RoomNumber));
                    command.Parameters.AddWithValue("@Floor", room.Floor);
                    command.Parameters.AddWithValue("@RoomSize", room.RoomSize);
                    command.Parameters.AddWithValue("@Capacity", room.Capacity);
                    command.Parameters.AddWithValue("@Status", (int)room.Status);
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Sletter et værelse fra databasen.
        /// </summary>
        public void DeleteRoom(int roomId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                string query = "DELETE FROM ROOM WHERE RoomID = @RoomID";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomID", roomId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
