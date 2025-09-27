namespace RailBook.Core.Domain.Entities
{
    public class Service
    {
        public int ServiceId { get; set; }    // Primary Key
        public string TrainName { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public int PassengerId { get; set; }  // FK -> Passenger.PassengerId
        public int CreatedBy { get; set; }    // FK -> User.Id
    }
}
