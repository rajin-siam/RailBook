namespace RailBook.Core.Application.Dtos.InvoiceDetails
{
    public class InvoiceDetailsDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int CreatedBy { get; set; }
    }
}
