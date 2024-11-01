using System.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using WPFClient.DTOs;

namespace WPFClient.Models
{
    /// <summary>
    /// Base model class for common methods, fields, properties
    /// </summary>
    public class ModelBase
    {
        protected readonly HttpClient _client;

        #region Event Handlers

        public event EventHandler<bool>? ApiQueryFinished;
        public event EventHandler<bool>? UserLoaded;

        #endregion

        public UserDTO? User { get; protected set; }
        public ModelBase()
        {
            _client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(5),
                BaseAddress = GetApiUrl()
            };
        }

        /// <summary>
        /// Method to get API url from config file
        /// </summary>
        /// <returns>Uri Object</returns>
        private static Uri GetApiUrl()
        {
            return new Uri(ConfigurationManager.AppSettings["WebApiUrl"]?.ToString() ?? "https://localhost:7035");
        }

        /// <summary>
        /// Method to notify listeners about API query finish
        /// </summary>
        /// <param name="success"></param>
        protected void CallApiQueryFinished(bool success)
        {
            ApiQueryFinished?.Invoke(this, success);
        }

        /// <summary>
        /// Method to notify listeners about user data loaded
        /// </summary>
        /// <param name="success"></param>
        protected void CallUserLoaded(bool success)
        {
            UserLoaded?.Invoke(this, success);
        }

        /// <summary>
        /// Method to call API get endpoint to get user data and notify listeners about event
        /// </summary>
        /// <param name="userId"></param>
        public void GetUser(long userId)
        {
            try
            {
                User = _client.GetFromJsonAsync<UserDTO>($"/api/Users/{userId}").GetAwaiter().GetResult();

                CallUserLoaded(User != null);
            }
            catch (Exception)
            {
                CallUserLoaded(false);
            }
        }
    }
}
