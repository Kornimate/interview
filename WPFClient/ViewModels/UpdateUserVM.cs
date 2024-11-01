﻿using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using WPFClient.Models;

namespace WPFClient.ViewModels
{
    /// <summary>
    /// ViewModel for Update User Page
    /// </summary>
    public class UpdateUserVM : ViewModelBase
    {
        private readonly UpdateUserModel _model;
        public ICommand UpdateUserCommand { get; set; }

        public UpdateUserVM()
        {
            _model = new UpdateUserModel();
            _model.ApiQueryFinished += CloseWindow;
            _model.UserLoaded += UserLoaded;

            UpdateUserCommand = new RelayCommand(UpdateUser);
        }

        /// <summary>
        /// Method to load user data
        /// </summary>
        /// <param name="userId"></param>
        public void GetUser(long userId)
        {
            _model.GetUser(userId);
        }

        /// <summary>
        /// Method to call update user in model class
        /// </summary>
        private void UpdateUser()
        {
            try
            {
                _model.UpdateUser(long.Parse(Id!), Name!, long.Parse(ZipCode!), Country, City, Street, long.Parse(HouseNumber!));
            }
            catch (Exception)
            {
                CallUserFailedToLoad(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Method to print user data on UI after it loaded
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
