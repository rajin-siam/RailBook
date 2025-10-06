using Mapster;
using RailBook.DataAccess.Interfaces;
using RailBook.Dtos.InvoiceDetails;
using RailBook.Manager.Interfaces;


namespace RailBook.Manager.Implementations
{
    public class InvoiceDetailsService : IInvoiceDetailsService
    {
        private readonly IInvoiceDetailsRepository _invoiceDetailsRepository;

        public InvoiceDetailsService(IInvoiceDetailsRepository invoiceDetailsRepository)
        {
            _invoiceDetailsRepository = invoiceDetailsRepository;
        }

        public async Task<List<InvoiceDetailsDto>> GetAllAsync()
        {
            var invoiceDetailsList =  await _invoiceDetailsRepository.GetAllAsync();
            return invoiceDetailsList.Adapt<List<InvoiceDetailsDto>>();
        }

        public async Task<InvoiceDetailsDto?> GetByIdAsync(int id)
        {
            return (await _invoiceDetailsRepository.GetByIdAsync(id)).Adapt<InvoiceDetailsDto>();
        }

        public async Task AddAsync(InvoiceDetailsDto invoiceDetailsDto)
        {
            var invoiceDetails = invoiceDetailsDto.Adapt<InvoiceDetails>();
            await _invoiceDetailsRepository.AddAsync(invoiceDetails);
        }

        public async Task UpdateAsync(InvoiceDetailsDto invoiceDetails)
        {
            var entity = invoiceDetails.Adapt<InvoiceDetails>();
            await _invoiceDetailsRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _invoiceDetailsRepository.DeleteAsync(id);
        }
    }
}
