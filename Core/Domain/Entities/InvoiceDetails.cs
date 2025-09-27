namespace RailBook.Core.Domain.Entities
{
    public class InvoiceDetails
    {
        public int Id { get; set; }           // Primary Key
        public int InvoiceId { get; set; }    // FK -> Invoice.InvoiceId
        public decimal TotalPrice { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int CreatedBy { get; set; }    // FK -> User.Id
    }
}
