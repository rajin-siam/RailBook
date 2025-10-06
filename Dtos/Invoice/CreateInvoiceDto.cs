using RailBook.Dtos.InvoiceDetails;

namespace RailBook.Dtos.Invoice
{
    public class CreateInvoiceDto
    {
        public int BookingId { get; set; }
        public int TotalAmount { get; set; }
        public CreateInvoiceDetailsDto? InvoiceDetails { get; set; }
    }
}
