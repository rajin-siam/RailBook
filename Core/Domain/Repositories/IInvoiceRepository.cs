using RailBook.Core.Domain.Entities;

namespace RailBook.Core.Domain.Repositories
{
    public interface IInvoiceRepository
    {
        Task<List<Invoice>> GetAllAsync();
        Task<Invoice?> GetByIdAsync(int id);
        Task AddAsync(Invoice invoice);
        Task UpdateAsync(Invoice invoice);
        Task DeleteAsync(int id);
    }
}
