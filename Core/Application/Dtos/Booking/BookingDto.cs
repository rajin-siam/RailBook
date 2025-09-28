namespace RailBook.Core.Application.Dtos.Booking
{
    public class BookingDto
    {
        public int BookingId { get; set; }
        public int PassengerId { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int InvoiceId { get; set; }
        public int CreatedBy { get; set; }
    }
}
