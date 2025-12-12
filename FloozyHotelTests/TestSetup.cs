using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Linq;

namespace FloozyHotelTests
{
    [TestClass]
    public class TestSetup
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            // Set connection string from App.config to environment variable for all tests
            var connectionString = ConfigurationManager.ConnectionStrings["HotelBooking"]?.ConnectionString;

            // Manual fallback: Read App.config directly if ConfigurationManager fails
            if (string.IsNullOrEmpty(connectionString))
            {
                try 
                {
                    var configPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.config");
                    if (System.IO.File.Exists(configPath))
                    {
                        var doc = System.Xml.Linq.XDocument.Load(configPath);
                        connectionString = doc.Descendants("add")
                            .FirstOrDefault(x => (string)x.Attribute("name") == "HotelBooking")
                            ?.Attribute("connectionString")?.Value;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to manually load App.config: {ex.Message}");
                }
            }

            if (!string.IsNullOrEmpty(connectionString))
            {
                Environment.SetEnvironmentVariable("HOTEL_BOOKING_CONN_STRING", connectionString);
            }
        }
    }
}
