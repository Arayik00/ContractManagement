using Azure.Core;
using ContractManagement.BL.Interfaces;
using ContractManagement.BL.Services;
using ContractManagement.BL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.ApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        // GET api/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [Authorize]
        // GET api/users/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserById(id);
            return user == null ? NotFound() : Ok(user);
        }

        // POST api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewUser([FromBody] Users newUser)
        {
            var errors = new List<string>();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (string.IsNullOrWhiteSpace(newUser.FirstName))
                errors.Add("First name is required.");

            if (string.IsNullOrWhiteSpace(newUser.LastName))
                errors.Add("Last name is required.");

            if (newUser.Username.Length < 4)
                errors.Add("Username must be at least 4 characters.");

            if (newUser.Password.Length < 4)
                errors.Add("Password must be at least 4 characters.");

            if (errors.Any())
                return BadRequest(new { Errors = errors });

            // Check if username already exists
            var existingUser = await _userService.GetUserByUsername(newUser.Username);
            if (existingUser != null)
                return Conflict(new { message = "Username already taken" });

            var success = await _userService.CreateUserAsync(newUser);
            if (!success)
                return StatusCode(500, new { message = "Unknown error occurred" });

            return CreatedAtAction(nameof(GetById), new { id = newUser.Id },
                new { message = "User registered successfully" });
        }

        [Authorize]
        [HttpGet("contracts/{id}")]
        public async Task<IActionResult> GetAllUserContracts(int id)
        {
            var result = await _userService.GetContractsByUserIdAsync(id);
            return Ok(result);
        }


    }
}
