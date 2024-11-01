using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using WPFClient.Models;

namespace WPFClient.ViewModels
{
    /// <summary>
    /// ViewModel for Create User Page
    /// </summary>
    class CreateUserVM : ViewModelBase
    {
        private readonly CreateUserModel _model;

        public event EventHandler<List<string>>? InvalidInputData;
        public ICommand CreateUserCommand { get; set; }

        public CreateUserVM()
        {
            _model = new CreateUserModel();
            _model.ApiQueryFinished += CloseWindow;

            CreateUserCommand = new RelayCommand(CreateNewUser);
        }

        /// <summary>
        /// Method to validate input data and to call model's creation method
        /// </summary>
        private void CreateNewUser()
        {
            List<string> problems = [];

            if (string.IsNullOrWhiteSpace(Name)) //check for faulty inputs
                problems.Add(nameof(Name));

            if (string.IsNullOrWhiteSpace(ZipCode))
                problems.Add("Zip Code");

            if (string.IsNullOrWhiteSpace(Country))
                problems.Add(nameof(Country));

            if (string.IsNullOrWhiteSpace(City))
                problems.Add(nameof(City));

            if (string.IsNullOrWhiteSpace(Street))
                problems.Add(nameof(Street));

            if (string.IsNullOrWhiteSpace(HouseNumber))
                problems.Add("House Number");

            if (problems.Count > 0)
            {
                InvalidInputData?.Invoke(this, problems);
                return;
            }

            _model.CreateNewUser(Name!, long.Parse(ZipCode!), Country, City, Street, long.Parse(HouseNumber!));
        }
    }
}
