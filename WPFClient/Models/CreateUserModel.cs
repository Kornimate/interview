using System.Net.Http.Json;
using WPFClient.DTOs;

namespace WPFClient.Models
{
    /// <summary>
    /// Model class for Create User Page
    /// </summary>
    public class CreateUserModel : ModelBase
    {
        public CreateUserModel() : base() { }

        /// <summary>
        /// Method to call API Post endpoint for user creation
        /// </summary>
        /// <param name="name"></param>
        /// <param name="zipCode"></param>
        /// <param name="country"></param>
        /// <param name="city"></param>
        /// <param name="street"></param>
        /// <param name="houseNumber"></param>
        public void CreateNewUser(string name, long zipCode, string? country, string? city, string? street, long houseNumber)
        {
            try
            {
                var response = _client.PostAsJsonAsync("/api/Users", new UserDTO
                {
                    Id = 0,
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
