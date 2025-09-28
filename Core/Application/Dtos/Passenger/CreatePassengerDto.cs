namespace RailBook.Core.Application.Dtos.Passenger
{
    public class CreatePassengerDto
    {
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int CreatedBy { get; set; }
    }
}
