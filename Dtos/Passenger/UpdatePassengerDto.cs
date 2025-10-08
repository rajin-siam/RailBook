using RailBook.Dtos.Service;

namespace RailBook.Dtos.Passenger
{
    public class UpdatePassengerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public List<UpdateTrainServiceDto>? TrainServices { get; set; }
    }
}
