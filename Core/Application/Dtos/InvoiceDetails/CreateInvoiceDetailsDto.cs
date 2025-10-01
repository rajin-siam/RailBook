namespace RailBook.Core.Application.Dtos.InvoiceDetails
{
    public class CreateInvoiceDetailsDto
    {
        public int TotalPrice { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string? InvoiceDescription { get; set; }
    }
}
