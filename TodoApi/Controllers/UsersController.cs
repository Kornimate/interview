﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    /// <summary>
    /// Controller to handle CRUD operations for Users
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        // GET: api/Users
        /// <summary>
        /// Action to handle GET request for users list
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                return Ok(await _service.GetUsers());
            }
            catch (ArgumentNullException ex)
            {
                return Problem(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET: api/User/5
        /// <summary>
        /// Action to handle GET request for specific user, specified by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Specific user</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long? id)
        {
            try
            {
                return Ok(await _service.GetUserById(id));
            }
            catch (ArgumentNullException ex)
            {
                return Problem(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PUT: api/TodoItems/5
        /// <summary>
        /// Action to handle PUT request for existing user to update data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns>Success if update is successful, otherwise error status</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(long id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                await _service.UpdateUser(user);
            }
            catch (ArgumentNullException ex)
            {
                return Problem(ex.Message);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return await CheckIfUserExistsWithIdAndReturnError(id, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return NoContent();
        }

        // POST: api/TodoItems
        /// <summary>
        /// Action to handle POST request to create new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Success if update is successful, otherwise error status</returns>
        [HttpPost]
        public async Task<ActionResult<User>> PostUsers(User user)
        {
            try
            {
                await _service.CreateNewUser(user);
            }
            catch (ArgumentNullException ex)
            {
                return Problem(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return StatusCode(StatusCodes.Status201Created, user);
        }

        // DELETE: api/TodoItems/5
        /// <summary>
        /// Action to handle DELETE request to delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success if update is successful, otherwise error status</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            try
            {
                await _service.DeleteUser(id);
            }
            catch (ArgumentNullException ex)
            {
                return Problem(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Method to decide if user exists with specific id then return error status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="outerEx"></param>
        /// <returns>Error status</returns>
        [NonAction]
        private async Task<IActionResult> CheckIfUserExistsWithIdAndReturnError(long id, Exception outerEx)
        {
            try
            {
                if (!await _service.UserExistsWithId(id))
                    return NotFound();

                return StatusCode(StatusCodes.Status500InternalServerError, outerEx.Message);
            }
            catch (ArgumentNullException ex)
            {
                return Problem(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
