using Microsoft.AspNetCore.Mvc;
using PoolBrackets_backend_dotnet.Models;
using PoolBrackets_backend_dotnet.Interfaces;
using PoolBrackets_backend_dotnet.DTOs;

namespace PoolBrackets_backend_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            await _userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            await _userRepository.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userRepository.DeleteUserAsync(id);
            return NoContent();
        }
        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginRequestModel userInfo)
        {
            {
                try
                {
                    var loginInfo = await _userRepository.LogginAsync(userInfo);
                    return Ok(loginInfo);
                }
                catch (UnauthorizedAccessException)
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "An error occurred", details = ex.Message });
                }
            }
        }
    }
}
