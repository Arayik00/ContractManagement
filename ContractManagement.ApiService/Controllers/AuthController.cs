using ContractManagement.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.ApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var token = await _auth.LoginAsync(request.Username, request.Password);

            if (token == null)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new { token });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
