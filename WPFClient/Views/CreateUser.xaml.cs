using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WPFClient.ViewModels;

namespace WPFClient.Views
{
    /// <summary>
    /// Interaction logic for CreateUser.xaml
    /// </summary>
    public partial class CreateUser : Window
    {
        private readonly CreateUserVM _vm;
        public CreateUser()
        {
            _vm = new CreateUserVM();
            _vm.ApiQueryFinished += CloseWindow;
            _vm.InvalidInputData += InvalidInput;

            InitializeComponent();

            this.DataContext = _vm;
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
        /// Event handler for closing the window with specified message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="successfulApiCall"></param>
        private void CloseWindow(object? sender, bool successfulApiCall)
        {
            if (!successfulApiCall)
                MessageBox.Show("Error while creating user", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show("User successfully created", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

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
