﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TodoApi.Controllers;
using TodoApi.Models;
using TodoApi.Services;

namespace WebApiTest
{
    [TestClass]
    public class WebApiUsersControllerTest
    {
        private readonly UsersController _controller;
        private readonly WebAppContext _context;
        private const long USERS_IN_DB = 2;
        public WebApiUsersControllerTest()
        {
            _context = new WebAppContext(
                                new DbContextOptionsBuilder<WebAppContext>()
                                    .UseInMemoryDatabase("WebApiTestDb")
                                    .Options);

            var service = new UsersService(_context, Mock.Of<ILogger<UsersService>>());

            _controller = new UsersController(service);
        }

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
        /// Test method to get users from API
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetUsersTest()
        {
            var response = await _controller.GetUsers();

            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
        }

        /// <summary>
        /// Test method to get users from API by id
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetUserTest()
        {
            var response = await _controller.GetUser(1L);

            Assert.IsInstanceOfType<OkObjectResult>(response.Result);
        }

        /// <summary>
        /// Test method to get users from API by id fails
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [DataRow(null)]
        [DataRow(USERS_IN_DB + 1)]
        public async Task GetUserTestFail(long? id)
        {
            var response = await _controller.GetUser(id);

            Assert.IsInstanceOfType<NotFoundObjectResult>(response.Result);
        }

        /// <summary>
        /// Test method to put endpoint in API
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task PutUserTest()
        {
            long id = 1L;

            var user = _context.Users.Single(x => x.Id == id);

            user.Name = "Test";

            var response = await _controller.PutUser(id, user);

            Assert.IsInstanceOfType<NoContentResult>(response);
        }

        /// <summary>
        /// Test method to put endpoint in API fails
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [DynamicData(nameof(UserDataAndResultGenerator), DynamicDataSourceType.Method)]
        public async Task PutUserTestFail(long id, User user, Type resultType)
        {

            var response = await _controller.PutUser(id, user);

            Assert.IsInstanceOfType(response, resultType);
        }

        /// <summary>
        /// Private method to generate test data for Put test
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<object[]> UserDataAndResultGenerator()
        {
            yield return new object[]
            {
                1L,
                new User(),
                typeof(BadRequestResult)
            };

            yield return new object[]
            {
                USERS_IN_DB + 1,
                new User() { Id = USERS_IN_DB + 1 },
                typeof(NotFoundResult)
            };
        }
    }
}