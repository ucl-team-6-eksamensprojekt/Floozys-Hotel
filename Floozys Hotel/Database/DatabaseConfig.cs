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
        private static string? _connectionString;

        /// <summary>
        /// Henter connection string fra App.config.
        /// Hvis ikke fundet, returneres en default connection string (kun til udvikling).
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    // Forsøger at hente fra App.config
                    _connectionString = ConfigurationManager.ConnectionStrings["HotelBooking"]?.ConnectionString;

                    // Anvender environment variable, hvis ikke fundet i config (sikrest til produktion)
                    if (string.IsNullOrEmpty(_connectionString))
                    {
                        _connectionString = Environment.GetEnvironmentVariable("HOTEL_BOOKING_CONN_STRING");
                    }

                    // Fallback: Hvis hverken config eller environment variable findes
                    if (string.IsNullOrEmpty(_connectionString))
                    {
                        throw new InvalidOperationException(
                            "Database connection string not found. " +
                            "Please add 'HotelBooking' connection string to App.config or set HOTEL_BOOKING_CONN_STRING environment variable.");
                    }
                }

                return _connectionString;
            }
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
