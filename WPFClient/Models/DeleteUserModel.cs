namespace WPFClient.Models
{
    /// <summary>
    /// Model class for Delete User Page
    /// </summary>
    public class DeleteUserModel : ModelBase
    {
        public DeleteUserModel() : base() { }

        /// <summary>
        /// Method to call API delete endpoint for user deletion
        /// </summary>
        public void DeleteUser()
        {
            try
            {
                var response = _client.DeleteAsync($"/api/Users/{User!.Id}").GetAwaiter().GetResult();

                CallApiQueryFinished(response.IsSuccessStatusCode);
            }
            catch (Exception)
            {
                CallApiQueryFinished(false);
            }
        }
    }
}
