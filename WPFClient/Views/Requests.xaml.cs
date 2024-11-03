using System.Windows;
using WPFClient.ViewModels;

namespace WPFClient.Views
{
    /// <summary>
    /// Interaction logic for Requests.xaml
    /// </summary>
    public partial class Requests : Window
    {
        private readonly int numOfRequests;
        private readonly RequestsVM _vm;

        public Requests(int numOfRequests, string title)
        {
            _vm = new RequestsVM();

            InitializeComponent();

            this.DataContext = _vm;
            this.Loaded += StartRequests;
            this.numOfRequests = numOfRequests;
            this.Title = title;
        }

        /// <summary>
        /// Event handler to start requests after window initialized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartRequests(object sender, RoutedEventArgs e)
        {
            _vm.StartRequests(numOfRequests);
        }
    }
}
