using RailBook.Core.Application.Dtos.InvoiceDetails;

namespace RailBook.Core.Application.Dtos.Invoice
{
    public class CreateInvoiceDto
    {
        public int BookingId { get; set; }
        public int TotalAmount { get; set; }
        public CreateInvoiceDetailsDto? InvoiceDetails { get; set; }
    }
}
