using System.Windows;
using WPFClient.ViewModels;

namespace WPFClient.Views
{
    /// <summary>
    /// Interaction logic for DeleteUser.xaml
    /// </summary>
    public partial class DeleteUser : Window
    {
        private readonly DeleteUserVM _vm;
        private readonly long userId;
        public DeleteUser(long userId)
        {
            _vm = new DeleteUserVM();
            _vm.ApiQueryFinished += CloseWindow;
            _vm.UserFailedToLoad += UserLoadedHandler;

            this.userId = userId;

            InitializeComponent();

            this.DataContext = _vm;
            this.Loaded += GetUser;
        }

        /// <summary>
        /// Event handler for loading user data after page loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetUser(object sender, RoutedEventArgs e)
        {
            _vm.GetUser(userId);
        }

        /// <summary>
        /// Event handler for user data loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_"></param>
        private void UserLoadedHandler(object? sender, EventArgs _)
        {
            MessageBox.Show("Error while loading user data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            this.DialogResult = false;

            this.Close();
        }

        /// <summary>
        /// Event handler for closing window with specified message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="successfulApiCall"></param>
        private void CloseWindow(object? sender, bool successfulApiCall)
        {
            if (!successfulApiCall)
                MessageBox.Show("Error while deleting user", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show("User successfully deleted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            this.DialogResult = successfulApiCall;

            this.Close();
        }
    }
}
