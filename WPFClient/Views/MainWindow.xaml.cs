using System.Windows;
using WPFClient.ViewModels;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowVM _vm;

        public MainWindow()
        {
            _vm = new MainWindowVM();

            InitializeComponent();
            
            this.DataContext = _vm;
        }
    }
}