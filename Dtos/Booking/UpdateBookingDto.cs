using RailBook.Dtos.Passenger;

namespace RailBook.Dtos.Booking
{
    public class UpdateBookingDto
    {
        public DateTime? BookingDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public int PerTicketPrice { get; set; }
        public List<UpdatePassengerDto> Passengers { get; set; } = [];
    }
}


