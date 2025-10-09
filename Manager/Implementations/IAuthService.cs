using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RailBook.ApiModels;
using RailBook.Configuration;
using RailBook.DataAccess.Database;
using RailBook.Domain.Entities;
using RailBook.Dtos.Auth;
using RailBook.Manager.Interfaces;

namespace RailBook.Manager.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            ApplicationDbContext context,
            IJwtTokenService jwtTokenService,
            IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
            _jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// Registers a new user and returns JWT token
        /// </summary>
        public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto)
        {
            try
            {
                // 1. Check if user already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == dto.Email);

                if (existingUser != null)
                {
                    return new ApiResponse<AuthResponseDto>
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Success = false,
                        Message = "User with this email already exists",
                        Errors = new List<string> { "Email already registered" },
                        Timestamp = DateTime.UtcNow
                    };
                }

                // 2. Hash password (using BCrypt - install BCrypt.Net-Next package)
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                // 3. Create new user
                var user = new User
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = hashedPassword,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = 1
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // 4. Generate JWT token
                string token = _jwtTokenService.GenerateToken(user);

                // 5. Return response
                return new ApiResponse<AuthResponseDto>
                {
                    StatusCode = StatusCodes.Status201Created,
                    Success = true,
                    Message = "User registered successfully",
                    Data = new AuthResponseDto
                    {
                        UserId = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Token = token,
                        ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes)
                    },
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthResponseDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Success = false,
                    Message = "Registration failed",
                    Errors = new List<string> { ex.Message },
                    Timestamp = DateTime.UtcNow
                };
            }
        }



        /// <summary>
        /// Authenticates user and returns JWT token
        /// </summary>
        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto)
        {
            try
            {
                // 1. Find user by email
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == dto.Email);

                if (user == null)
                {
                    return new ApiResponse<AuthResponseDto>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Success = false,
                        Message = "Invalid credentials",
                        Errors = new List<string> { "Email or password is incorrect" },
                        Timestamp = DateTime.UtcNow
                    };
                }

                // 2. Verify password
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

                if (!isPasswordValid)
                {
                    return new ApiResponse<AuthResponseDto>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Success = false,
                        Message = "Invalid credentials",
                        Errors = new List<string> { "Email or password is incorrect" },
                        Timestamp = DateTime.UtcNow
                    };
                }

                // 3. Generate JWT token
                string token = _jwtTokenService.GenerateToken(user);

                // 4. Return response
                return new ApiResponse<AuthResponseDto>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Login successful",
                    Data = new AuthResponseDto
                    {
                        UserId = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Token = token,
                        ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes)
                    },
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthResponseDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Success = false,
                    Message = "Login failed",
                    Errors = new List<string> { ex.Message },
                    Timestamp = DateTime.UtcNow
                };
            }
        }
    }
}
