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

            int passengerCount = booking.Passengers.Count;
            int baseFare = booking.PerTicketPrice * passengerCount;
            int serviceCost = booking.Passengers
                .SelectMany(p => p.TrainServices)
                .Sum(s => s.Price);
            int totalAmount = baseFare + serviceCost;

            var invoice = booking.Invoice; // get existing invoice if present

            if (invoice == null)
            {
                invoice = new Invoice
                {
                    BookingId = booking.Id,
                    CreatedBy = 1,
                    CreatedAt = DateTime.UtcNow,
                    TotalAmount = totalAmount
                };
                return invoice;
            }

            // Update values
            invoice.TotalAmount = totalAmount;
            invoice.ModifiedById = 1;
            invoice.ModifiedAt = DateTime.UtcNow;

            return invoice;
        }


    }
}
