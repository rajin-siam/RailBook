using RailBook.Core.Domain.Entities;


namespace RailBook.Core.Domain.Repositories
{
    public interface IInvoiceDetailsRepository
    {
        Task<List<InvoiceDetails>> GetAllAsync();
        Task<InvoiceDetails?> GetByIdAsync(int id);
        Task AddAsync(InvoiceDetails invoiceDetails);
        Task UpdateAsync(InvoiceDetails invoiceDetails);
        Task DeleteAsync(int id);
    }
}
