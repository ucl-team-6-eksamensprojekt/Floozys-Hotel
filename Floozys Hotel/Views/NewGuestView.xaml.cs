using System.Windows;
using Floozys_Hotel.Models;
using Floozys_Hotel.ViewModels;

namespace Floozys_Hotel.Views
{
    public partial class NewGuestView : Window
    {
        private NewGuestViewModel _viewModel;
        public NewGuestView(Guest guest, bool isEdit, Action<Guest> onSave)
        {
            InitializeComponent();
            _viewModel = new NewGuestViewModel(guest, isEdit);
            _viewModel.OnSave += onSave;
            _viewModel.OnSave += OnSave;
            DataContext = _viewModel;
        }

        private void OnSave(Guest guest)
        { 

        }
    }
}