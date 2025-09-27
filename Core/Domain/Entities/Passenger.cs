namespace RailBook.Core.Domain.Entities
{
    public class Passenger
    {
        public int PassengerId { get; set; }   // Primary Key
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        public int CreatedBy { get; set; }     // FK -> User.Id
    }
}
