using RailBook.Core.Application.Dtos.InvoiceDetails;

namespace RailBook.Core.Application.Dtos.Invoice
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public InvoiceDetailsDto? InvoiceDetails { get; set; }
    }
}
