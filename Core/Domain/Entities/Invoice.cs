namespace RailBook.Core.Domain.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }    // Primary Key
        public int PassengerId { get; set; }  // FK -> Passenger.PassengerId
        public int UserId { get; set; }       // FK -> User.Id
        public int CreatedBy { get; set; }    // FK -> User.Id
    }
}
