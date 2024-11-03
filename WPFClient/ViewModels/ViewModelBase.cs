using CommunityToolkit.Mvvm.ComponentModel;

namespace WPFClient.ViewModels
{
    /// <summary>
    /// Base ViewModel class for common methods, fields and properties
    /// </summary>
    public class ViewModelBase : ObservableObject
    {
        #region Fields

        private string? _id;
        private string? _name;
        private string? _zipCode;
        private string? _country;
        private string? _city;
        private string? _street;
        private string? _houseNumber;

        #endregion

        #region Properties
        public string? Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string? ZipCode
        {
            get => _zipCode;
            set => SetProperty(ref _zipCode, value);
        }
        public string? Country
        {
            get => _country;
            set => SetProperty(ref _country, value);
        }
        public string? City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }
        public string? Street
        {
            get => _street;
            set => SetProperty(ref _street, value);
        }
        public string? HouseNumber
        {
            get => _houseNumber;
            set => SetProperty(ref _houseNumber, value);
        }

        #endregion

        #region Event Handlers

        public event EventHandler<bool>? ApiQueryFinished;
        public event EventHandler? UserFailedToLoad;
        public event EventHandler<List<string>>? InvalidInputData;

        #endregion

        /// <summary>
        /// Method to notify listener windows about API query finish
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="successfulApiCall"></param>
        protected void CloseWindow(object? sender, bool successfulApiCall)
        {
            ApiQueryFinished?.Invoke(sender, successfulApiCall);
        }

        /// <summary>
        /// Method to notify listener windows about user data load fail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CallUserFailedToLoad(object? sender, EventArgs e)
        {
            UserFailedToLoad?.Invoke(sender, e);
        }

        /// <summary>
        /// Method to notify user about passing invalid input data for user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="problems"></param>
        protected void CallInvalidinputUserData(object? sender, List<string> problems)
        {
            InvalidInputData?.Invoke(sender, problems);
        }

        /// <summary>
        /// Method to see if string property value is valid
        /// </summary>
        /// <param name="property"></param>
        /// <param name="problems"></param>
        protected void ValidateStringProperty(string? property, string propertyName, ref List<string> problems)
        {
            if (string.IsNullOrWhiteSpace(property) || property == String.Empty) //check for faulty inputs
                problems.Add(propertyName);
        }
    }
}
