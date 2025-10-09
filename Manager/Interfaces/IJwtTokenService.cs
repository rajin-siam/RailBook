using RailBook.Domain.Entities;
using System.Security.Claims;

namespace RailBook.Manager.Interfaces
{
    /// Service responsible for generating and validating JWT tokens
    public interface IJwtTokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal? ValidateToken(string token, bool ignoreExpiry);
    }
}
