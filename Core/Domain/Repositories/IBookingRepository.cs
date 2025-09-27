using RailBook.Core.Domain.Entities;

namespace RailBook.Core.Domain.Repositories
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(int id);
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(int id);
    }
}
