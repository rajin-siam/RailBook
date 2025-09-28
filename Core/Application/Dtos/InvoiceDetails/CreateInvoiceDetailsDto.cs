namespace RailBook.Core.Application.Dtos.InvoiceDetails
{
    public class CreateInvoiceDetailsDto
    {
        public int InvoiceId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int CreatedBy { get; set; }
    }
}
