namespace RailBook.Dtos.InvoiceDetails
{
    public class InvoiceDetailsDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int TotalPrice { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string? InvoiceDescription { get; set; }
    }
}
