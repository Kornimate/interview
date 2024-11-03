using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WPFClient.ViewModels;

namespace WPFClient.Views
{
    /// <summary>
    /// Interaction logic for UpdateUser.xaml
    /// </summary>
    public partial class UpdateUser : Window
    {
        private readonly UpdateUserVM _vm;
        private readonly long userId;

        public UpdateUser(long userId)
        {
            _vm = new UpdateUserVM();
            _vm.ApiQueryFinished += CloseWindow;
            _vm.UserFailedToLoad += UserFailedToLoad;
            _vm.InvalidInputData += InvalidInput;

            this.userId = userId;

            InitializeComponent();

            this.DataContext = _vm;
            this.Loaded += GetUser;
        }

        /// <summary>
        /// Event handler for invalid input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="problematicProperties"></param>
        private void InvalidInput(object? sender, List<string> problematicProperties)
        {
            MessageBox.Show("Invalid input Data in the following:\n" + String.Join('\n', problematicProperties), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Event handler to load user data after page initialized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetUser(object sender, RoutedEventArgs e)
        {
            _vm.GetUser(userId);
        }

        /// <summary>
        /// Event handler to handle load failure for user data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_"></param>
        private void UserFailedToLoad(object? sender, EventArgs _)
        {
            MessageBox.Show("Error while loading user data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            this.DialogResult = false;

            this.Close();
        }

        /// <summary>
        /// Event handler to close window with specific message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="successfulApiCall"></param>
        private void CloseWindow(object? sender, bool successfulApiCall)
        {
            if (!successfulApiCall)
                MessageBox.Show("Error while updating user", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show("User successfully updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            this.DialogResult = successfulApiCall;

            this.Close();
        }

        /// <summary>
        /// Check for invalid input in TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckIfNumber(object sender, TextCompositionEventArgs e)
        {
            var regex = NumRegex();
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Static method to get regex at compile time
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex("[^0-9]+")]
        private static partial Regex NumRegex();
    }
}
