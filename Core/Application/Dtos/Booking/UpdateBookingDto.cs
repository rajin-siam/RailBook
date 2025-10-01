namespace RailBook.Core.Application.Dtos.Booking
{
    public class UpdateBookingDto
    {
        public DateTime? BookingDate { get; set; }
        public string? Status { get; set; }
        public string? Source { get; set; }
        public string? Destination { get; set; }
        public int? PerTicketPrice { get; set; }
    }
}


