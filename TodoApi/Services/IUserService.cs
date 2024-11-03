
using TodoApi.Models;

namespace TodoApi.Services
{
    /// <summary>
    /// Interface for users table interactions service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Method to create new user in DB
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Task to be awaited</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        Task CreateNewUserAsync(User user);

        /// <summary>
        /// Method to delete user from DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task to be awaited</returns
        Task DeleteUserAsync(long? id);

        /// <summary>
        /// Method to get specific user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Specific user</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        Task<User> GetUserByIdAsync(long? id);

        /// <summary>
        /// Method to get full list of users from DB
        /// </summary>
        /// <returns>List of users</returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<IEnumerable<User>> GetUsersAsync();

        /// <summary>
        /// Method to update user data in DB
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Task to be awaited</returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task UpdateUserAsync(User user);

        /// <summary>
        /// Method to check if user exists in DB with given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean value indication if the user exists in DB</returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<bool> UserExistsWithIdAsync(long id);
    }
}
