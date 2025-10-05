using RailBook.Domain.Dtos.Invoice;
using RailBook.Domain.Dtos.Passenger;

namespace RailBook.Domain.Dtos.Booking
{
    public class BookingDto
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public int PerTicketPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PassengerDto> Passengers { get; set; } = [];
        public InvoiceDto? Invoice { get; set; }
    }
}