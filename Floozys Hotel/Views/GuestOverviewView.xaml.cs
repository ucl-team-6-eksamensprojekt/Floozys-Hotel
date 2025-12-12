using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Floozys_Hotel.Models;
using Floozys_Hotel.ViewModels;

namespace Floozys_Hotel.Views
{
    /// <summary>
    /// Interaction logic for GuestOverviewView.xaml
    /// </summary>
    public partial class GuestOverviewView : UserControl
    {
        private GuestOverviewViewModel _viewModel;
        public GuestOverviewView()
        {
            InitializeComponent();
            _viewModel = new GuestOverviewViewModel();

            // Event hooks 
            _viewModel.NewGuestRequested += ShowNewGuestDialog;
            _viewModel.EditGuestRequested += ShowEditGuestDialog;
            _viewModel.ShowInfoDialog += (msg, title) =>
                MessageBox.Show(Window.GetWindow(this), msg, title, MessageBoxButton.OK, MessageBoxImage.Information);

            DataContext = _viewModel;
        }

        // Open window with no guest to create new guest
        private void ShowNewGuestDialog()
        {
            var win = new NewGuestView(null, false, guest =>
            {
                _viewModel.AddGuestToOverview(guest);
            });
            win.Owner = Window.GetWindow(this); // Set owner to window
            win.ShowDialog();
        }

        // Open window with existing guest to edit 
        private void ShowEditGuestDialog(Guest guestCopy)
        {
            var win = new NewGuestView(guestCopy, true, editedGuest =>
            {
                _viewModel.UpdateSelectedGuest(editedGuest);
            });
            win.Owner = Window.GetWindow(this);
            win.ShowDialog();
        }
    }
}
