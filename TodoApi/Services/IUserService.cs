
using TodoApi.Models;

namespace TodoApi.Services
{
    /// <summary>
    /// Interface for users table interactions service
    /// </summary>
    public interface IUserService
    {
        Task CreateNewUser(User user);
        Task DeleteUser(long? id);
        Task<User> GetUserById(long? id);
        Task<IEnumerable<User>> GetUsers();
        Task UpdateUser(User user);
        Task<bool> UserExistsWithId(long id);
    }
}
