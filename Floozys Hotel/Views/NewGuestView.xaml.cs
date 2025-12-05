using System.Windows;
using Floozys_Hotel.Models;
using Floozys_Hotel.ViewModels;

namespace Floozys_Hotel.Views
{
    public partial class NewGuestView : Window
    {
        public NewGuestView(Guest guest, bool isEdit, Action<Guest> onSave)
        {
            InitializeComponent();
            DataContext = new NewGuestViewModel(this, guest, isEdit, onSave);
        }
    }
}