using System.Net.Http.Json;
using WPFClient.DTOs;

namespace WPFClient.Models
{
    /// <summary>
    /// Model class for Update User Page
    /// </summary>
    public class UpdateUserModel : ModelBase
    {
        public UpdateUserModel() : base() { }

        /// <summary>
        /// Method to call API put endpoint to update user data in DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="zipCode"></param>
        /// <param name="country"></param>
        /// <param name="city"></param>
        /// <param name="street"></param>
        /// <param name="houseNumber"></param>
        public void UpdateUser(long id, string name, long zipCode, string? country, string? city, string? street, long houseNumber)
        {
            try
            {
                var response = _client.PutAsJsonAsync($"/api/Users/{id}", new UserDTO
                {
                    Id = id,
                    Name = name,
                    ZipCode = zipCode,
                    Country = country,
                    City = city,
                    Street = street,
                    HouseNumber = houseNumber
                }).GetAwaiter().GetResult();

                CallApiQueryFinished(response.IsSuccessStatusCode);
            }
            catch (Exception)
            {
                CallApiQueryFinished(false);
            }
        }
    }
}
