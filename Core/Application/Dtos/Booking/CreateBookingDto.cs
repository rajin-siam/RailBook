using RailBook.Core.Application.Dtos.Passenger;

namespace RailBook.Core.Application.Dtos.Booking
{
    public class CreateBookingDto
    {
        public string Source { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public int PerTicketPrice { get; set; }
        public List<CreatePassengerDto> Passengers { get; set; } = [];
    }
}
