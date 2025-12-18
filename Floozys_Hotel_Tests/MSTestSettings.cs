using Floozys_Hotel.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Windows.Media.Media3D;

[assembly: Parallelize(Scope = ExecutionScope.MethodLevel)]

namespace Floozys_Hotel_Tests
{
    /// <summary>
    /// Global test initialization - runs once before all tests
    /// Sets up DatabaseConfig.ConnectionString from appsettings.json
    /// </summary>
    [TestClass]
    public class MSTestSettings
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Build configuration from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Set the connection string for all tests
            DatabaseConfig.ConnectionString = config.GetConnectionString("DefaultConnection");
        }
    }
}
