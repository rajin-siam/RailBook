using RailBook.Dtos.Service;

namespace RailBook.Dtos.Passenger
{
    public class CreatePassengerDto
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public List<CreateTrainServiceDto>? TrainServices { get; set; }
    }
}
