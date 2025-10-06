using RailBook.Dtos.InvoiceDetails;

namespace RailBook.Manager.Interfaces
{
    public interface IInvoiceDetailsService
    {
        Task<List<InvoiceDetailsDto>> GetAllAsync();
        Task<InvoiceDetailsDto?> GetByIdAsync(int id);
        Task AddAsync(InvoiceDetailsDto invoiceDetails);
        Task UpdateAsync(InvoiceDetailsDto invoiceDetails);
        Task DeleteAsync(int id);
    }
}
