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

            ValidateStringProperty(Name, nameof(Name), ref problems);
            ValidateStringProperty(ZipCode, nameof(ZipCode), ref problems);
            ValidateStringProperty(Country, nameof(Country), ref problems);
            ValidateStringProperty(City, nameof(City), ref problems);
            ValidateStringProperty(Street, nameof(Street), ref problems);
            ValidateStringProperty(HouseNumber, nameof(HouseNumber), ref problems);

            if (problems.Count > 0)
            {
                CallInvalidinputUserData(this, problems);
                return;
            }

            _model.CreateNewUser(Name!, long.Parse(ZipCode!), Country, City, Street, long.Parse(HouseNumber!));
        }
    }
}
