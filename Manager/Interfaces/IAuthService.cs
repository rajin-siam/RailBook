using RailBook.ApiModels;
using RailBook.Dtos.Auth;

namespace RailBook.Manager.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto);
        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto);
    }
}
