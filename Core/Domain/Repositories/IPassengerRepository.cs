using RailBook.Core.Domain.Entities;

namespace RailBook.Core.Domain.Repositories
{
    public interface IPassengerRepository
    {
        Task<List<Passenger>> GetAllAsync();
        Task<Passenger?> GetByIdAsync(int id);
        Task AddAsync(Passenger passenger);
        Task UpdateAsync(Passenger passenger);
        Task DeleteAsync(int id);
    }
}
