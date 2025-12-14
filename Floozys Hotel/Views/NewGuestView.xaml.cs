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
            var vm = new NewGuestViewModel(guest, isEdit);

            vm.ShowError += (msg, title) => MessageBox.Show(this, msg, title, MessageBoxButton.OK, MessageBoxImage.Error);
            vm.ShowInfo += (msg, title) => MessageBox.Show(this, msg, title, MessageBoxButton.OK, MessageBoxImage.Information);
            vm.ShowConfirmation += (msg, title, buttons) =>
                MessageBox.Show(this, msg, title, buttons, MessageBoxImage.Question);
            vm.RequestClose += () => this.Close();
            vm.OnSave += onSave;

            DataContext = vm;
        }
    }
}
