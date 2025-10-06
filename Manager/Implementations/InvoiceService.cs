using Mapster;
using RailBook.DataAccess.Interfaces;
using RailBook.Dtos.Invoice;
using RailBook.Manager.Interfaces;

namespace RailBook.Manager.Implementations
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<List<InvoiceDto>> GetAllInvoicesAsync()
        {
            var invoice = await _invoiceRepository.GetAllAsync();
            return invoice.Adapt<List<InvoiceDto>>();
        }

        public async Task<InvoiceDto?> GetInvoiceByIdAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            return invoice?.Adapt<InvoiceDto>();
        }

        public async Task AddInvoiceAsync(Invoice invoice)
        {
            await _invoiceRepository.AddAsync(invoice);
        }

        public async Task UpdateInvoiceAsync(InvoiceDto invoiceDto)
        {
            await _invoiceRepository.UpdateAsync(invoiceDto.Adapt<Invoice>());
        }

        public async Task DeleteInvoiceAsync(int id)
        {
            await _invoiceRepository.DeleteAsync(id);
        }

        public async Task<Invoice> GenerateInvoiceAsync(Booking booking)
        {
            if (booking == null)
                throw new Exception("Booking not found");

            // Step 1: Base fare = ticket price × number of passengers
            int passengerCount = booking.Passengers.Count;
            int baseFare = booking.PerTicketPrice * passengerCount;

            // Step 2: Extra services cost (sum for all passengers)
            int serviceCost = booking.Passengers
                .SelectMany(p => p.TrainServices)
                .Sum(s => s.Price);   // assumes TrainService has a Price field

            int totalAmount = baseFare + serviceCost;



            // Step 3: Create Invoice
            var invoice = new Invoice
            {
                BookingId = booking.Id,
                TotalAmount = totalAmount,
                CreatedBy = 1,
                CreatedAt = DateTime.UtcNow
            };
            //await AddInvoiceAsync(invoice);
            return invoice; 
        }

    }
}
