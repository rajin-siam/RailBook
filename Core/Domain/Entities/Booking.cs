namespace RailBook.Core.Domain.Entities
{
    public class Booking
    {
        public int BookingId { get; set; }    // Primary Key
        public int PassengerId { get; set; }  // FK -> Passenger.PassengerId
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = "Pending";

        public int InvoiceId { get; set; }    // FK -> Invoice.InvoiceId
        public int CreatedBy { get; set; }    // FK -> User.Id
    }
}
