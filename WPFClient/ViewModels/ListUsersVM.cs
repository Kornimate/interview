using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFClient.DTOs;
using WPFClient.Models;
using WPFClient.Views;

namespace WPFClient.ViewModels
{
    /// <summary>
    /// ViewModel for List Users Page
    /// </summary>
    public class ListUsersVM : ObservableObject
    {
        private static readonly string LOADING_TEXT = "Loading...";

        private ListUsersModel _model;
        public ObservableCollection<UserDTO> Users { get; private set; }

        public event EventHandler<string>? ShowErrorMessageBox;

        #region Commands
        public ICommand OpenDeleteUserPageCommand { get; set; }
        public ICommand OpenUpdateUserPageCommand { get; set; }

        #endregion
        public ListUsersVM()
        {
            Users = [];

            _model = new ListUsersModel();
            _model.ApiQueryFinished += SetUIUsersData;

            OpenDeleteUserPageCommand = new RelayCommand<long>(OpenDeleteUserPage);
            OpenUpdateUserPageCommand = new RelayCommand<long>(OpenUpdateUserPage);
        }

        /// <summary>
        /// Method to open Update User Page and refresh the page data in neccessary
        /// </summary>
        /// <param name="userId"></param>
        private void OpenUpdateUserPage(long userId)
        {
            var window = new UpdateUser(userId);
            var succesfulAction = window.ShowDialog();

            if (succesfulAction == false)
                return;

            InitializeUsers();
        }

        /// <summary>
        /// Method to open Delete User Page and refresh the page data if neccessary
        /// </summary>
        /// <param name="userId"></param>
        private void OpenDeleteUserPage(long userId)
        {
            var window = new DeleteUser(userId);
            var succesfulAction = window.ShowDialog();

            if (succesfulAction == false)
                return;

            InitializeUsers();
        }

        /// <summary>
        /// Method to load users data
        /// </summary>
        public void InitializeUsers()
        {
            Users.Clear();

            _model.GetUsers();
        }

        /// <summary>
        /// Method to print users data on UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="resultSuccess"></param>
        private void SetUIUsersData(object? sender, bool resultSuccess)
        {
            Users.Clear();

            if (!resultSuccess)
            {
                ShowErrorMessageBox?.Invoke(this, "Error while retrieving users data!");
                return;
            }

            foreach (UserDTO user in _model.Users!)
            {
                Users.Add(user);
            }
        }
    }
}
