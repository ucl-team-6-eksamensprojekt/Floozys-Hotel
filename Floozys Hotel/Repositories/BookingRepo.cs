using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories.Interfaces;

namespace Floozys_Hotel.Repositories
{
    public class BookingRepo : IBooking
    {
        private readonly string _connectionString;

        public BookingRepo()
        {
            _connectionString = Database.DatabaseConfig.ConnectionString;
        }

        // CREATE
        public void Create(Booking booking)
        {
            if (booking == null) throw new ArgumentNullException(nameof(booking));

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    INSERT INTO BOOKING (StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID) 
                    VALUES (@StartDate, @EndDate, @CheckInTime, @CheckOutTime, @Status, @RoomID, @GuestID);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@StartDate", booking.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", booking.EndDate);
                    cmd.Parameters.AddWithValue("@CheckInTime", (object?)booking.CheckInTime ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CheckOutTime", (object?)booking.CheckOutTime ?? DBNull.Value); 
                    cmd.Parameters.AddWithValue("@Status", (int)booking.Status);
                    cmd.Parameters.AddWithValue("@RoomID", (object?)booking.Room?.RoomId ?? (object?)booking.RoomID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GuestID", (object?)booking.Guest?.GuestID ?? (object?)booking.GuestID ?? DBNull.Value);

                    int newId = (int)cmd.ExecuteScalar();
                    booking.BookingID = newId;
                }
            }
        }

        // READ
        public List<Booking> GetAll()
        {
            var bookings = new List<Booking>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    SELECT 
                        b.BookingID, b.StartDate, b.EndDate, b.CheckInTime, b.CheckOutTime, b.Status,
                        r.RoomID, r.RoomNumber, r.Floor, r.RoomSize, r.Capacity, r.Status as RoomStatus,
                        g.GuestID, g.FirstName, g.LastName, g.PassportNumber, g.Email, g.Country, g.PhoneNumber
                    FROM BOOKING b
                    INNER JOIN ROOM r ON b.RoomID = r.RoomID
                    INNER JOIN GUEST g ON b.GuestID = g.GuestID";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var booking = new Booking
                            {
                                BookingID = reader.GetInt32(reader.GetOrdinal("BookingID")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                CheckInTime = reader.IsDBNull(reader.GetOrdinal("CheckInTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CheckInTime")),
                                CheckOutTime = reader.IsDBNull(reader.GetOrdinal("CheckOutTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CheckOutTime")),
                                Status = (BookingStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                                
                                Room = new Room
                                {
                                    RoomId = reader.GetInt32(reader.GetOrdinal("RoomID")),
                                    RoomNumber = reader.GetString(reader.GetOrdinal("RoomNumber")),
                                    Floor = reader.GetInt32(reader.GetOrdinal("Floor")),
                                    RoomSize = reader.GetString(reader.GetOrdinal("RoomSize")),
                                    Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")),
                                    Status = (RoomStatus)reader.GetInt32(reader.GetOrdinal("RoomStatus"))
                                },
                                Guest = new Guest
                                {
                                    GuestID = reader.GetInt32(reader.GetOrdinal("GuestID")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    PassportNumber = reader.IsDBNull(reader.GetOrdinal("PassportNumber")) ? null : reader.GetString(reader.GetOrdinal("PassportNumber")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Country = reader.GetString(reader.GetOrdinal("Country")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"))
                                }
                            };
                            
                            booking.RoomID = booking.Room.RoomId;
                            booking.GuestID = booking.Guest.GuestID;

                            bookings.Add(booking);
                        }
                    }
                }
            }
            return bookings;
        }

        public Booking? GetById(int bookingID)
        {
             using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    SELECT 
                        b.BookingID, b.StartDate, b.EndDate, b.CheckInTime, b.CheckOutTime, b.Status,
                        r.RoomID, r.RoomNumber, r.Floor, r.RoomSize, r.Capacity, r.Status as RoomStatus,
                        g.GuestID, g.FirstName, g.LastName, g.PassportNumber, g.Email, g.Country, g.PhoneNumber
                    FROM BOOKING b
                    INNER JOIN ROOM r ON b.RoomID = r.RoomID
                    INNER JOIN GUEST g ON b.GuestID = g.GuestID
                    WHERE b.BookingID = @BookingID";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@BookingID", bookingID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                             var booking = new Booking
                            {
                                BookingID = reader.GetInt32(reader.GetOrdinal("BookingID")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                CheckInTime = reader.IsDBNull(reader.GetOrdinal("CheckInTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CheckInTime")),
                                CheckOutTime = reader.IsDBNull(reader.GetOrdinal("CheckOutTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CheckOutTime")),
                                Status = (BookingStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                                
                                Room = new Room
                                {
                                    RoomId = reader.GetInt32(reader.GetOrdinal("RoomID")),
                                    RoomNumber = reader.GetString(reader.GetOrdinal("RoomNumber")),
                                    Floor = reader.GetInt32(reader.GetOrdinal("Floor")),
                                    RoomSize = reader.GetString(reader.GetOrdinal("RoomSize")),
                                    Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")),
                                    Status = (RoomStatus)reader.GetInt32(reader.GetOrdinal("RoomStatus"))
                                },
                                Guest = new Guest
                                {
                                    GuestID = reader.GetInt32(reader.GetOrdinal("GuestID")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    PassportNumber = reader.IsDBNull(reader.GetOrdinal("PassportNumber")) ? null : reader.GetString(reader.GetOrdinal("PassportNumber")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Country = reader.GetString(reader.GetOrdinal("Country")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"))
                                }
                            };
                            booking.RoomID = booking.Room.RoomId;
                            booking.GuestID = booking.Guest.GuestID;
                            return booking;
                        }
                    }
                }
            }
            return null;
        }

        public List<Booking> GetByStatus(BookingStatus status)
        {
            var all = GetAll();
            return all.FindAll(b => b.Status == status);
        }

        public List<Booking> GetByRoomID(int roomID)
        {
            var all = GetAll();
            return all.FindAll(b => b.RoomID == roomID);
        }

        public List<Booking> GetByGuestID(int guestID)
        {
            var all = GetAll();
            return all.FindAll(b => b.GuestID == guestID);
        }

        // UPDATE
        public void Update(Booking booking)
        {
            if (booking == null) throw new ArgumentNullException(nameof(booking));

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    UPDATE BOOKING 
                    SET StartDate = @StartDate, 
                        EndDate = @EndDate, 
                        CheckInTime = @CheckInTime, 
                        CheckOutTime = @CheckOutTime, 
                        Status = @Status, 
                        RoomID = @RoomID, 
                        GuestID = @GuestID
                    WHERE BookingID = @BookingID";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@BookingID", booking.BookingID);
                    cmd.Parameters.AddWithValue("@StartDate", booking.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", booking.EndDate);
                    cmd.Parameters.AddWithValue("@CheckInTime", (object?)booking.CheckInTime ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CheckOutTime", (object?)booking.CheckOutTime ?? DBNull.Value); 
                    cmd.Parameters.AddWithValue("@Status", (int)booking.Status);
                    cmd.Parameters.AddWithValue("@RoomID", (object?)booking.Room?.RoomId ?? (object?)booking.RoomID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GuestID", (object?)booking.Guest?.GuestID ?? (object?)booking.GuestID ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // DELETE
        public void Delete(int bookingID)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM BOOKING WHERE BookingID = @BookingID";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@BookingID", bookingID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
