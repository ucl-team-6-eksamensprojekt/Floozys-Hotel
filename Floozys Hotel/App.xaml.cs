using System.Configuration;
using System.Data;
using System.Windows;

namespace Floozys_Hotel;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
    }

    private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        string errorMessage = $"[{DateTime.Now}] An unhandled exception occurred: {e.Exception.Message}\nStack Trace:\n{e.Exception.StackTrace}\n\n";
        System.IO.File.AppendAllText("crash_log.txt", errorMessage);
        e.Handled = true; 
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            string errorMessage = $"[{DateTime.Now}] Critical error: {ex.Message}\nStack Trace:\n{ex.StackTrace}\n\n";
            System.IO.File.AppendAllText("crash_log.txt", errorMessage);
        }
    }
}

