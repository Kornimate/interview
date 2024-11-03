using Microsoft.AspNetCore.Mvc;
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
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                return Ok(await _service.GetUsersAsync());
            }
            catch (ArgumentNullException ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET: api/Users/5
        /// <summary>
        /// Action to handle GET request for specific user, specified by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Specific user</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> GetUser(long? id)
        {
            try
            {
                return Ok(await _service.GetUserByIdAsync(id));
            }
            catch (ArgumentNullException ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
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

        // PUT: api/Users/5
        /// <summary>
        /// Action to handle PUT request for existing user to update data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns>Success if update is successful, otherwise error status</returns>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PutUser(long id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                await _service.UpdateUserAsync(user);
            }
            catch (ArgumentNullException ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
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

        // POST: api/Users
        /// <summary>
        /// Action to handle POST request to create new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Success if update is successful, otherwise error status</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                await _service.CreateNewUserAsync(user);
            }
            catch (ArgumentNullException ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Created("/api/Users", user);
        }

        // DELETE: api/Users/5
        /// <summary>
        /// Action to handle DELETE request to delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success if update is successful, otherwise error status</returns>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteUser(long? id)
        {
            try
            {
                await _service.DeleteUserAsync(id);
            }
            catch (ArgumentNullException ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
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
        private async Task<ActionResult> CheckIfUserExistsWithIdAndReturnError(long id, Exception outerEx)
        {
            try
            {
                if (!await _service.UserExistsWithIdAsync(id))
                    return NotFound();

                return StatusCode(StatusCodes.Status500InternalServerError, outerEx.Message);
            }
            catch (ArgumentNullException ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
