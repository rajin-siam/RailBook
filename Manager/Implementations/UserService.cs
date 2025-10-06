using Mapster;
using RailBook.DataAccess.Interfaces;
using RailBook.Domain.Entities;
using RailBook.Dtos.User;
using RailBook.Manager.Interfaces;


namespace RailBook.Manager.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var userList = await _userRepository.GetAllAsync();
            return userList.Adapt<List<UserDto>>();
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            return (await _userRepository.GetByIdAsync(id)).Adapt<UserDto?>();
        }

        public async Task AddUserAsync(CreateUserDto userDto)
        {
            await _userRepository.AddAsync(userDto.Adapt<User>());
        }

        public async Task UpdateUserAsync(UserDto userDto)
        {
            await _userRepository.UpdateAsync(userDto.Adapt<User>());
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}
