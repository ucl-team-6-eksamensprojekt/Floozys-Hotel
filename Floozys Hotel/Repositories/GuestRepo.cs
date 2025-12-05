using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Floozys_Hotel.Models;
using Floozys_Hotel.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;

namespace Floozys_Hotel.Repositories
{
    public class GuestRepo : IGuestRepo
    {
        // TODO: Store connection strings securely(e.g., in appsettings.json or Azure Key Vault), and inject them via configuration or dependency injection.
        
        private readonly string _cs;
        public GuestRepo(string connectionString)
        {
            _cs = connectionString; // TODO: Correct the connection to the Database / Azure SQL
        }

        // CREATE
        public void CreateGuest(Guest guest)
        {
            // TODO: Check if this is the right way to insert data into the Database

            using (var conn = new SqlConnection(_cs))
            {
                conn.Open();
                var sql = "INSERT INTO GUEST (FirstName, LastName, Email, PhoneNumber, Country, PassportNumber) VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Country, @PassportNumber)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", guest.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", guest.LastName);
                    cmd.Parameters.AddWithValue("@Email", guest.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", guest.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Country", guest.Country);
                    cmd.Parameters.AddWithValue("@PassportNumber", guest.PassportNumber);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // READ
        public List<Guest> GetAll()
        {
            // TODO: Check if this is the right way to READ data from the Database

            var list = new List<Guest>();
            const string sql = "SELECT GuestID, FirstName, LastName, Email, PhoneNumber, Country, PassportNumber FROM GUEST ORDER BY GuestID";

            using var conn = new SqlConnection(_cs);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Guest
                {
                    GuestID = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    PhoneNumber = reader.GetString(4),
                    Country = reader.GetString(5),
                    PassportNumber = reader.GetString(6)
                });
            }
            return list;

        }

        public List<Guest> GetAllByName(string name)
        {
            // TODO: Check if this is the right way to READ data from the Database

            var list = new List<Guest>();
            // Search on both first and last name, case-insensitive, partial match
            const string sql = @"
                SELECT GuestID, FirstName, LastName, Email, PhoneNumber, Country, PassportNumber
                FROM GUEST
                WHERE FirstName LIKE @Name OR LastName LIKE @Name
                ORDER BY GuestID";

            using var conn = new SqlConnection(_cs);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", $"%{name}%");
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Guest
                {
                    GuestID = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    PhoneNumber = reader.GetString(4),
                    Country = reader.GetString(5),
                    PassportNumber = reader.GetString(6)
                });
            }
            return list;
        }

        public Guest GetByID(int id)
        {
            // TODO: Check if this is the right way to READ data from the Database

            using (var conn = new SqlConnection(_cs))
            {
                conn.Open();
                var sql = "SELECT GuestID, FirstName, LastName, Email, PhoneNumber, Country, PassportNumber FROM GUEST WHERE GuestID = @GuestID";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@GuestID", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Guest
                            {
                                GuestID = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Email = reader.GetString(3),
                                PhoneNumber = reader.GetString(4),
                                Country = reader.GetString(5),
                                PassportNumber = reader.GetString(6)
                            };
                        }
                    }
                }
            }
            return null;
        }

        // UPDATE
        public void UpdateGuest(Guest guest)
        {
            // TODO: Check if this is the right way to UPDATE data in the Database
            using (var conn = new SqlConnection(_cs))
            {
                conn.Open();
                var sql = "UPDATE GUEST SET FirstName = @FirstName, LastName = @LastName, Email = @Email, PhoneNumber = @PhoneNumber, Country = @Country, PassportNumber = @PassportNumber WHERE GuestID = @GuestID";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", guest.GuestID);
                    cmd.Parameters.AddWithValue("@FirstName", guest.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", guest.LastName);
                    cmd.Parameters.AddWithValue("@Email", guest.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", guest.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Country", guest.Country);
                    cmd.Parameters.AddWithValue("@PassportNumber", guest.PassportNumber);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // DELETE
        public void DeleteGuest(int id)
        {
            // TODO: Check if this is the right way to DELETE data in the Database
            using (var conn = new SqlConnection(_cs))
            {
                conn.Open();
                var sql = "DELETE FROM GUEST WHERE GuestID = @GuestID";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@GuestID", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
