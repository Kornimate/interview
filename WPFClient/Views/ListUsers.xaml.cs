using System.Windows;
using WPFClient.ViewModels;

namespace WPFClient.Views
{
    /// <summary>
    /// Interaction logic for ListUsers.xaml
    /// </summary>
    public partial class ListUsers : Window
    {
        private readonly ListUsersVM _vm;
        public ListUsers()
        {
            _vm = new ListUsersVM();

            _vm.ShowErrorMessageBox += ShowErrorMessageBox;

            InitializeComponent();

            this.DataContext = _vm;
            this.Loaded += WindowLoaded;
        }

        /// <summary>
        /// Event handler for window loaded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _vm.InitializeUsers();
        }

        /// <summary>
        /// Event handler to show error message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="text"></param>
        private void ShowErrorMessageBox(object? sender, string text)
        {
            MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            this.Close();
        }
    }
}
