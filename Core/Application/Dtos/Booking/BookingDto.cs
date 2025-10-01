using RailBook.Core.Application.Dtos.Invoice;
using RailBook.Core.Application.Dtos.Passenger;

namespace RailBook.Core.Application.Dtos.Booking
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