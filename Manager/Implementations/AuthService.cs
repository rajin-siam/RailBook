using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RailBook.ApiModels;
using RailBook.Configuration;
using RailBook.DataAccess.Database;
using RailBook.Domain.Entities;
using RailBook.Dtos.Auth;
using RailBook.Manager.Interfaces;
using System.Security.Claims;

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

                // 4. Generate JWT access token & refresh token
                string accessToken = _jwtTokenService.GenerateAccessToken(user);
                string refreshToken = _jwtTokenService.GenerateRefreshToken();

                DateTime accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes);
                DateTime refreshTokenExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);

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
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        AccessTokenExpiresAt = accessTokenExpiresAt,
                        RefreshTokenExpiresAt = refreshTokenExpiresAt
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
                // 1. Find and validate user
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                {
                    return new ApiResponse<AuthResponseDto>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Success = false,
                        Message = "Invalid credentials"
                    };
                }

                // 2. Generate BOTH tokens
                string accessToken = _jwtTokenService.GenerateAccessToken(user);

                string refreshToken = _jwtTokenService.GenerateRefreshToken();

                // 3. Save refresh token to database
                user.RefreshToken = refreshToken;
                
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _context.SaveChangesAsync();

                // 4. Return BOTH tokens
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
                        
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
                        RefreshTokenExpiresAt = user.RefreshTokenExpiryTime.Value
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
                    Errors = new List<string> { ex.Message }
                };
            }
        }




        /// Refresh access token using refresh token
        public async Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenDto dto)
        {
            try
            {
                // Validate the expired access token (don't check expiry)
                var principal = _jwtTokenService.ValidateToken(dto.AccessToken, true);

                if (principal == null)
                {
                    return new ApiResponse<AuthResponseDto>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Success = false,
                        Message = "Invalid access token",
                        Timestamp = DateTime.UtcNow
                    };
                }

                // Get user ID from token
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return new ApiResponse<AuthResponseDto>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Success = false,
                        Message = "Invalid token claims",
                        Timestamp = DateTime.UtcNow
                    };
                }

                // Find user
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    return new ApiResponse<AuthResponseDto>
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "User not found",
                        Timestamp = DateTime.UtcNow
                    };
                }

                // Validate refresh token
                if (user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    return new ApiResponse<AuthResponseDto>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Success = false,
                        Message = "Invalid or expired refresh token",
                        Timestamp = DateTime.UtcNow
                    };
                }

                // Generate new tokens
                var newAccessToken = _jwtTokenService.GenerateAccessToken(user);
                var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

                // Update user's refresh token
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays); // 7 days validity
                await _context.SaveChangesAsync();

                return new ApiResponse<AuthResponseDto>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Token refreshed successfully",
                    Data = new AuthResponseDto
                    {
                        AccessToken = newAccessToken,
                        RefreshToken = newRefreshToken,
                        AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
                        RefreshTokenExpiresAt = user.RefreshTokenExpiryTime.Value
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
                    Message = "Token refresh failed",
                    Errors = new List<string> { ex.Message },
                    Timestamp = DateTime.UtcNow
                };
            }
        }


        //   Logout Functionality
        public async Task<ApiResponse<string>> LogoutAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "User not found"
                    };
                }

                // Invalidate refresh token
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;

                await _context.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Success = true,
                    Message = "User logged out successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Success = false,
                    Message = "Logout failed",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

    }
}
