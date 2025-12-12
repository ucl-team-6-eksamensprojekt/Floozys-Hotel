using Microsoft.Data.SqlClient;
using System;
using System.Configuration;

namespace Floozys_Hotel.Database
{
    /// <summary>
    /// Håndterer database connection string og configuration.
    /// Connection string hentes fra App.config for at undgå hardcoding.
    /// </summary>
    public static class DatabaseConfig
    {
        public static string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Tester om der kan oprettes forbindelse til databasen.
        /// </summary>
        /// <returns>True hvis forbindelse lykkes, false ellers.</returns>
        public static bool TestConnection()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Tester forbindelse og kaster exception med detaljer hvis det fejler.
        /// </summary>
        public static void TestConnectionWithDetails()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                Console.WriteLine($"Connection successful to database: {connection.Database}");
                Console.WriteLine($"Server version: {connection.ServerVersion}");
            }
        }
    }
}
