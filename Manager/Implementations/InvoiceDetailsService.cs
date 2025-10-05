using RailBook.DataAccess.Interfaces;


namespace RailBook.Manager.Implementations
{
    public class InvoiceDetailsService
    {
        private readonly IInvoiceDetailsRepository _invoiceDetailsRepository;

        public InvoiceDetailsService(IInvoiceDetailsRepository invoiceDetailsRepository)
        {
            _invoiceDetailsRepository = invoiceDetailsRepository;
        }

        public async Task<List<InvoiceDetails>> GetAllAsync()
        {
            return await _invoiceDetailsRepository.GetAllAsync();
        }

        public async Task<InvoiceDetails?> GetByIdAsync(int id)
        {
            return await _invoiceDetailsRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(InvoiceDetails invoiceDetails)
        {
            await _invoiceDetailsRepository.AddAsync(invoiceDetails);
        }

        public async Task UpdateAsync(InvoiceDetails invoiceDetails)
        {
            await _invoiceDetailsRepository.UpdateAsync(invoiceDetails);
        }

        public async Task DeleteAsync(int id)
        {
            await _invoiceDetailsRepository.DeleteAsync(id);
        }
    }
}
