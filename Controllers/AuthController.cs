using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RailBook.Dtos.Auth;
using RailBook.Manager.Interfaces;
using System.Security.Claims;

namespace RailBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// Register a new user
        /// <param name="dto">Registration details</param>
        /// Authentication response with JWT token
        [HttpPost("register")]
        [AllowAnonymous] // Anyone can access this endpoint
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var response = await _authService.RegisterAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        /// Login with existing credentials
        /// <param name="dto">Login credentials</param>
        /// <returns>Authentication response with JWT token</returns>
        [HttpPost("login")]
        [AllowAnonymous] // Anyone can access this endpoint
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var response = await _authService.LoginAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        /// Get current authenticated user's information
        /// <returns>User details from JWT token</returns>
        [HttpGet("me")]
        [Authorize] // Only authenticated users can access this
        public IActionResult GetCurrentUser()
        {
            // Extract user information from JWT token claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            return Ok(new
            {
                UserId = userId,
                Name = userName,
                Email = userEmail,
                Message = "You are authenticated!"
            });
        }

        /// Test endpoint to verify authentication is working
        [HttpGet("protected")]
        [Authorize] // Requires valid JWT token
        public IActionResult ProtectedEndpoint()
        {
            return Ok(new
            {
                Message = "This is a protected endpoint. You have access!",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}