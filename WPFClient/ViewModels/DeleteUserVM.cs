using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using WPFClient.Models;

namespace WPFClient.ViewModels
{
    /// <summary>
    /// ViewModel for Delete User Page
    /// </summary>
    public class DeleteUserVM : ViewModelBase
    {
        private readonly DeleteUserModel _model;
        public ICommand DeleteUserCommand { get; set; }

        public DeleteUserVM()
        {
            _model = new DeleteUserModel();
            _model.ApiQueryFinished += CloseWindow;
            _model.UserLoaded += UserLoaded;

            DeleteUserCommand = new RelayCommand(DeleteUser);
        }

        /// <summary>
        /// Method to call the model to get user data
        /// </summary>
        /// <param name="userId"></param>
        public void GetUser(long userId)
        {
            _model.GetUser(userId);
        }

        /// <summary>
        /// Method to call the model to delete user
        /// </summary>
        private void DeleteUser()
        {
            try
            {
                _model.DeleteUser();
            }
            catch (Exception)
            {
                CallUserFailedToLoad(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Method to print loaded user data on UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="success"></param>
        private void UserLoaded(object? sender, bool success)
        {
            if (!success)
            {
                CallUserFailedToLoad(sender, EventArgs.Empty);
                return;
            }

            Id = _model.User!.Id.ToString();
            Name = _model.User!.Name;
            ZipCode = _model.User!.ZipCode.ToString();
            Country = _model.User!.Country;
            City = _model.User!.City;
            Street = _model.User!.Street;
            HouseNumber = _model.User!.HouseNumber.ToString();
        }
    }
}
