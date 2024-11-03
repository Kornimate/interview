using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class UsersService : IUserService
    {
        private readonly WebAppContext _context;
        private readonly ILogger<UsersService> _logger;

        public UsersService(WebAppContext context, ILogger<UsersService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Method to create new user in DB
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Task to be awaited</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task CreateNewUserAsync(User user)
        {
            if (_context.Users == null)
                throw new ArgumentNullException(nameof(_context.Users), "no users table");


            if (!user.IsValid)
                throw new InvalidOperationException("not valid user data");

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"user added to DB ({user.Name})");
        }

        /// <summary>
        /// Method to delete user from DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task to be awaited</returns>
        public async Task DeleteUserAsync(long? id)
        {
            User user = await GetUserByIdAsync(id);

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"user deleted from DB ({user.Name})");
        }

        /// <summary>
        /// Method to get specific user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Specific user</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<User> GetUserByIdAsync(long? id)
        {
            if (id == null)
                throw new InvalidOperationException("id can not be null");

            if (_context.Users == null)
                throw new ArgumentNullException(nameof(_context.Users), "no users table");

            return await _context.Users.FirstAsync(u => u.Id == id);
        }

        /// <summary>
        /// Method to get full list of users from DB
        /// </summary>
        /// <returns>List of users</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            if (_context.Users == null)
                throw new ArgumentNullException(nameof(_context.Users), "no users table");

            return await _context.Users.ToArrayAsync();
        }

        /// <summary>
        /// Method to update user data in DB
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Task to be awaited</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task UpdateUserAsync(User user)
        {
            if (_context.Users == null)
                throw new ArgumentNullException(nameof(_context.Users), "no users table");

            _context.Entry<User>(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"user updated in DB ({user.Name})");
        }

        /// <summary>
        /// Method to check if user exists in DB with given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean value indication if the user exists in DB</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> UserExistsWithIdAsync(long id)
        {
            if (_context.Users == null)
                throw new ArgumentNullException(nameof(_context.Users), "no users table");

            return await _context.Users.AnyAsync(x => x.Id == id);
        }
    }
}
