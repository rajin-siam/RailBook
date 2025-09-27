using RailBook.Core.Domain.Entities;


namespace RailBook.Core.Domain.Repositories
{
    public interface IServiceRepository
    {
        Task<List<Service>> GetAllAsync();
        Task<Service?> GetByIdAsync(int id);
        Task AddAsync(Service service);
        Task UpdateAsync(Service service);
        Task DeleteAsync(int id);
    }
}
