
namespace RailBook.DataAccess.Interfaces
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
