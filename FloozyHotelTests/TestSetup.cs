using System;
using System.Configuration;
using System.Linq;
using Floozys_Hotel.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FloozyHotelTests
{
    [TestClass]
    public class TestSetup
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();

            DatabaseConfig.ConnectionString = config.GetConnectionString("DefaultConnection");
        }
    }
}
