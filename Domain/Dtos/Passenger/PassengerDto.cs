using RailBook.Domain.Dtos.Service;

namespace RailBook.Domain.Dtos.Passenger
{
    public class PassengerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public int BookingId { get; set; }
        public List<TrainServiceDto> TrainServices { get; set; } = [];
    }
}
