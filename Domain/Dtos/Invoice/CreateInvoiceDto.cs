using RailBook.Domain.Dtos.InvoiceDetails;

namespace RailBook.Domain.Dtos.Invoice
{
    public class CreateInvoiceDto
    {
        public int BookingId { get; set; }
        public int TotalAmount { get; set; }
        public CreateInvoiceDetailsDto? InvoiceDetails { get; set; }
    }
}
