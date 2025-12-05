using Microsoft.Data.SqlClient;
using Floozys_Hotel.Models;
using System;
using System.Collections.Generic;

namespace Floozys_Hotel.Repositories
{
    public class GuestRepo
    {
        private readonly string _connectionString;

        public GuestRepo()
        {
            _connectionString = Database.DatabaseConfig.ConnectionString;
        }

        /// <summary>
        /// Henter alle gæster fra databasen.
        /// </summary>
        public List<Guest> GetAllGuests()
        {
            var guests = new List<Guest>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                string query = "SELECT GuestID, FirstName, LastName, PassportNumber, Email, Country, PhoneNumber FROM GUEST";
                
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        guests.Add(new Guest
                        {
                            GuestID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            // PassportNumber er nullable, konverterer DBNull til null.
                            PassportNumber = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Email = reader.GetString(4),
                            Country = reader.GetString(5),
                            PhoneNumber = reader.GetString(6)
                        });
                    }
                }
            }

            return guests;
        }

        /// <summary>
        /// Henter en enkelt gæst baseret på GuestID.
        /// </summary>
        public Guest? GetGuestById(int guestId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                string query = "SELECT GuestID, FirstName, LastName, PassportNumber, Email, Country, PhoneNumber FROM GUEST WHERE GuestID = @GuestID";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GuestID", guestId);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Guest
                            {
                                GuestID = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                PassportNumber = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Email = reader.GetString(4),
                                Country = reader.GetString(5),
                                PhoneNumber = reader.GetString(6)
                            };
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Tilføjer en ny gæst til databasen.
        /// </summary>
        public int AddGuest(Guest guest)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                string query = @"INSERT INTO GUEST (FirstName, LastName, PassportNumber, Email, Country, PhoneNumber) 
                                VALUES (@FirstName, @LastName, @PassportNumber, @Email, @Country, @PhoneNumber);
                                SELECT CAST(SCOPE_IDENTITY() as int);";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", guest.FirstName);
                    command.Parameters.AddWithValue("@LastName", guest.LastName);
                    // Håndterer nullable PassportNumber ved at konvertere til DBNull hvis null.
                    command.Parameters.AddWithValue("@PassportNumber", (object?)guest.PassportNumber ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", guest.Email);
                    command.Parameters.AddWithValue("@Country", guest.Country);
                    command.Parameters.AddWithValue("@PhoneNumber", guest.PhoneNumber);
                    
                    return (int)command.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Opdaterer en eksisterende gæst.
        /// </summary>
        public void UpdateGuest(Guest guest)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                string query = @"UPDATE GUEST 
                                SET FirstName = @FirstName, LastName = @LastName, PassportNumber = @PassportNumber, 
                                    Email = @Email, Country = @Country, PhoneNumber = @PhoneNumber
                                WHERE GuestID = @GuestID";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GuestID", guest.GuestID);
                    command.Parameters.AddWithValue("@FirstName", guest.FirstName);
                    command.Parameters.AddWithValue("@LastName", guest.LastName);
                    command.Parameters.AddWithValue("@PassportNumber", (object?)guest.PassportNumber ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", guest.Email);
                    command.Parameters.AddWithValue("@Country", guest.Country);
                    command.Parameters.AddWithValue("@PhoneNumber", guest.PhoneNumber);
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Sletter en gæst fra databasen.
        /// </summary>
        public void DeleteGuest(int guestId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                string query = "DELETE FROM GUEST WHERE GuestID = @GuestID";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GuestID", guestId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
