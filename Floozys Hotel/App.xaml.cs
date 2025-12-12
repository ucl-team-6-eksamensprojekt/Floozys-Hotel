using System.Configuration;
using System.Data;
using System.Windows;
using Floozys_Hotel.Database;
using Floozys_Hotel.ViewModels;
using Microsoft.Extensions.Configuration;

namespace Floozys_Hotel;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
              
        DatabaseConfig.ConnectionString = config.GetConnectionString("DefaultConnection");
    }
}

