using RailBook.Dtos.Invoice;

namespace RailBook.Manager.Interfaces
{
    public interface IInvoiceService
    {
        Task<List<InvoiceDto>> GetAllInvoicesAsync();
        Task<InvoiceDto?> GetInvoiceByIdAsync(int id);
        Task AddInvoiceAsync(Invoice invoice);
        Task UpdateInvoiceAsync(InvoiceDto invoiceDto);
        Task DeleteInvoiceAsync(int id);
        Task<Invoice> GenerateInvoiceAsync(Booking booking);
    }
}
