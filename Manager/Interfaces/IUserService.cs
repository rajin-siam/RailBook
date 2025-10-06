using RailBook.Domain.Entities;
using RailBook.Dtos.User;

namespace RailBook.Manager.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task AddUserAsync(CreateUserDto userDto);
        Task UpdateUserAsync(UserDto userDto);
        Task DeleteUserAsync(int id);
    }
}
