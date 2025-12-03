using Microsoft.Data.SqlClient;
using Floozys_Hotel.Models;
using System;
using System.Collections.Generic;

namespace Floozys_Hotel.Repositories
{
    public class BookingRepo
    {
        private readonly string _connectionString;

        public BookingRepo()
        {
            _connectionString = Database.DatabaseConfig.ConnectionString;
        }

        /// <summary>
        /// Henter alle bookinger fra databasen.
        /// </summary>
        public List<Booking> GetAllBookings()
        {
            var bookings = new List<Booking>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                string query = "SELECT BookingID, StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID FROM BOOKING";
                
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bookings.Add(new Booking
                        {
                            BookingID = reader.GetInt32(0),
                            StartDate = reader.GetDateTime(1),
                            EndDate = reader.GetDateTime(2),
                            CheckInTime = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                            CheckOutTime = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                            Status = (BookingStatus)reader.GetInt32(5),
                            RoomID = reader.GetInt32(6),
                            GuestID = reader.GetInt32(7)
                        });
                    }
                }
            }

            return bookings;
        }

        /// <summary>
        /// Henter en enkelt booking baseret på BookingID.
        /// </summary>
        public Booking? GetBookingById(int bookingId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                string query = "SELECT BookingID, StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID FROM BOOKING WHERE BookingID = @BookingID";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookingID", bookingId);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Booking
                            {
                                BookingID = reader.GetInt32(0),
                                StartDate = reader.GetDateTime(1),
                                EndDate = reader.GetDateTime(2),
                                CheckInTime = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                                CheckOutTime = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                                Status = (BookingStatus)reader.GetInt32(5),
                                RoomID = reader.GetInt32(6),
                                GuestID = reader.GetInt32(7)
                            };
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Henter bookinger for et specifikt værelse.
        /// </summary>
        public List<Booking> GetBookingsByRoomId(int roomId)
        {
            var bookings = new List<Booking>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                string query = "SELECT BookingID, StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID FROM BOOKING WHERE RoomID = @RoomID";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomID", roomId);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bookings.Add(new Booking
                            {
                                BookingID = reader.GetInt32(0),
                                StartDate = reader.GetDateTime(1),
                                EndDate = reader.GetDateTime(2),
                                CheckInTime = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                                CheckOutTime = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                                Status = (BookingStatus)reader.GetInt32(5),
                                RoomID = reader.GetInt32(6),
                                GuestID = reader.GetInt32(7)
                            });
                        }
                    }
                }
            }

            return bookings;
        }

        /// <summary>
        /// Tilføjer en ny booking til databasen.
        /// </summary>
        public int AddBooking(Booking booking)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                string query = @"INSERT INTO BOOKING (StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID) 
                                VALUES (@StartDate, @EndDate, @CheckInTime, @CheckOutTime, @Status, @RoomID, @GuestID);
                                SELECT CAST(SCOPE_IDENTITY() as int);";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", booking.StartDate);
                    command.Parameters.AddWithValue("@EndDate", booking.EndDate);
                    command.Parameters.AddWithValue("@CheckInTime", (object?)booking.CheckInTime ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CheckOutTime", (object?)booking.CheckOutTime ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", (int)booking.Status);
                    command.Parameters.AddWithValue("@RoomID", booking.RoomID);
                    command.Parameters.AddWithValue("@GuestID", booking.GuestID);
                    
                    return (int)command.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Opdaterer en eksisterende booking.
        /// </summary>
        public void UpdateBooking(Booking booking)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                string query = @"UPDATE BOOKING 
                                SET StartDate = @StartDate, EndDate = @EndDate, CheckInTime = @CheckInTime, 
                                    CheckOutTime = @CheckOutTime, Status = @Status, RoomID = @RoomID, GuestID = @GuestID
                                WHERE BookingID = @BookingID";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookingID", booking.BookingID);
                    command.Parameters.AddWithValue("@StartDate", booking.StartDate);
                    command.Parameters.AddWithValue("@EndDate", booking.EndDate);
                    command.Parameters.AddWithValue("@CheckInTime", (object?)booking.CheckInTime ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CheckOutTime", (object?)booking.CheckOutTime ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", (int)booking.Status);
                    command.Parameters.AddWithValue("@RoomID", booking.RoomID);
                    command.Parameters.AddWithValue("@GuestID", booking.GuestID);
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Sletter en booking fra databasen.
        /// </summary>
        public void DeleteBooking(int bookingId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                string query = "DELETE FROM BOOKING WHERE BookingID = @BookingID";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookingID", bookingId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
