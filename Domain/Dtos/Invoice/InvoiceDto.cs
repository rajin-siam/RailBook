using RailBook.Domain.Dtos.InvoiceDetails;

namespace RailBook.Domain.Dtos.Invoice
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
