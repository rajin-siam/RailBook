using RailBook.DataAccess.Interfaces;

namespace RailBook.Manager.Implementations
{
    public class InvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<List<Invoice>> GetAllInvoicesAsync()
        {
            return await _invoiceRepository.GetAllAsync();
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(int id)
        {
            return await _invoiceRepository.GetByIdAsync(id);
        }

        public async Task AddInvoiceAsync(Invoice invoice)
        {
            await _invoiceRepository.AddAsync(invoice);
        }

        public async Task UpdateInvoiceAsync(Invoice invoice)
        {
            await _invoiceRepository.UpdateAsync(invoice);
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
