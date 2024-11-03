using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;
using TodoApi.Models;
using TodoApi.Services;

namespace WebApiTest
{
    [TestClass]
    public class WebApiServiceTest : IDisposable
    {
        private readonly IUserService _service;
        private readonly WebAppContext _context;
        private const long USERS_IN_DB = 2;
        public WebApiServiceTest()
        {
            var options = new DbContextOptionsBuilder<WebAppContext>()
                                .UseInMemoryDatabase("WebApiTestDb")
                                .Options;

            _context = new WebAppContext(options);

            _service = new UsersService(_context, Mock.Of<ILogger<UsersService>>());
        }

        /// <summary>
        /// Method to free resources
        /// </summary>
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        /// <summary>
        /// Method to run before every test method
        /// </summary>
        [TestInitialize]
        public void InitializeTestDbData()
        {
            TestDbSeeder.SeedDb(_context);
        }

        /// <summary>
        /// Method to run after every test method
        /// </summary>
        [TestCleanup]
        public void CleanupTestDbData()
        {
            TestDbSeeder.DeleteDb(_context);
        }

        /// <summary>
        /// Test method for user creation success
        /// </summary>
        [TestMethod]
        public async Task CreateNewUserTest()
        {
            Assert.AreEqual(USERS_IN_DB, _context.Users.Count());

            await _service.CreateNewUserAsync(new User()
            {
                Name = "Test",
                Country = "Test",
                City = "Test",
                Street = "Test",
                HouseNumber = 1L,
                ZipCode = 1L
            });

            Assert.AreEqual(USERS_IN_DB + 1, _context.Users.Count());
        }

        /// <summary>
        /// Test method to test user creation failure
        /// </summary>
        [TestMethod]
        [DataRow("", "Test", "Test", "Test")] // invalid name
        [DataRow("Test", "", "Test", "Test")] // invalid country
        [DataRow("Test", "Test", "", "Test")] // invalid city
        [DataRow("Test", "Test", "Test", "")] // invalid street
        public void CreateNewUserTestFail(string name, string country, string city, string street)
        {
            Assert.ThrowsException<InvalidOperationException>(() => _service.CreateNewUserAsync(new User()
            {
                Name = name,
                Country = country,
                City = city,
                Street = street,
                HouseNumber = 1L,
                ZipCode = 1L
            }).GetAwaiter().GetResult());
        }

        /// <summary>
        /// Test method to delete user
        /// </summary>
        /// <returns>Task</returns>
        [TestMethod]
        public async Task DeleteUserTest()
        {
            Assert.AreEqual(USERS_IN_DB, _context.Users.Count());

            await _service.DeleteUserAsync(1);

            Assert.AreEqual(USERS_IN_DB - 1, _context.Users.Count());
        }

        /// <summary>
        /// Test method to test user deletion fail
        /// </summary>
        /// <param name="id"></param>
        [TestMethod]
        [DataRow(null)]
        [DataRow(USERS_IN_DB + 1)]
        public void DeleteUserTestFail(long? id)
        {
            Assert.ThrowsException<InvalidOperationException>(() => _service.DeleteUserAsync(id)
                                                                            .GetAwaiter()
                                                                            .GetResult());
        }

        /// <summary>
        /// Test method to gert user by id 
        /// </summary>
        /// <returns>Task</returns>
        [TestMethod]
        public async Task GetUserTest()
        {
            var user = await _service.GetUserByIdAsync(1);

            Assert.IsNotNull(user);
            Assert.AreEqual(1L, user.Id);
        }

        /// <summary>
        /// Test method to test get user by id fails
        /// </summary>
        /// <param name="id"></param>
        [TestMethod]
        [DataRow(null)]
        [DataRow(USERS_IN_DB + 1)]
        public void GetUserTestFail(long? id)
        {
            Assert.ThrowsException<InvalidOperationException>(() => _service.DeleteUserAsync(id)
                                                                            .GetAwaiter()
                                                                            .GetResult());
        }

        /// <summary>
        /// Test method to get users list
        /// </summary>
        /// <returns>Task</returns>
        [TestMethod]
        public async Task GetUsersTest()
        {
            var users = await _service.GetUsersAsync();

            Assert.IsNotNull(users);
            Assert.AreEqual(USERS_IN_DB, users.Count());
        }

        /// <summary>
        /// Test method to update user
        /// </summary>
        /// <returns>Task</returns>
        [TestMethod]
        public async Task UpdateUserTest()
        {
            string newName = "Test2";

            var user = _context.Users.First();

            user.Name = newName;

            await _service.UpdateUserAsync(user);

            user = _context.Users.First();

            Assert.AreEqual(newName, user.Name);
        }

        /// <summary>
        /// Test method to update user fails
        /// </summary>
        /// <returns>Task</returns>
        [TestMethod]
        public void UpdateUserTestFail()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _service.UpdateUserAsync(null!)
                                                                    .GetAwaiter()
                                                                    .GetResult());

            Assert.ThrowsException<DbUpdateConcurrencyException>(() => _service.UpdateUserAsync(new User())
                                                                    .GetAwaiter()
                                                                    .GetResult());
        }

        [TestMethod]
        [DataRow(1L, true)]
        [DataRow(USERS_IN_DB + 1, false)]
        public async Task UserExistsWithIdTest(long id, bool actionResult)
        {
            Assert.AreEqual(actionResult, await _service.UserExistsWithIdAsync(id));
        }
    }
}