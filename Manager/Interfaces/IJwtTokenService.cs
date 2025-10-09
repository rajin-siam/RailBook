using RailBook.Domain.Entities;
using System.Security.Claims;

namespace RailBook.Manager.Interfaces
{
    /// Service responsible for generating and validating JWT tokens
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
