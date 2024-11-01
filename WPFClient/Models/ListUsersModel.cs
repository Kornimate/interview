using System.Net.Http.Json;
using WPFClient.DTOs;

namespace WPFClient.Models
{
    /// <summary>
    /// Model class for List Users Page
    /// </summary>
    public class ListUsersModel : ModelBase
    {
        public List<UserDTO>? Users { get; private set; }

        public ListUsersModel() : base() { }

        /// <summary>
        /// Method to call API get endpoint for listing users
        /// </summary>
        public void GetUsers()
        {
            try
            {
                Users = _client.GetFromJsonAsync<List<UserDTO>>("/api/Users").GetAwaiter().GetResult();

                if (Users == null)
                    CallApiQueryFinished(false);

                CallApiQueryFinished(true);
            }
            catch (Exception)
            {
                CallApiQueryFinished(false);
            }
        }
    }
}
