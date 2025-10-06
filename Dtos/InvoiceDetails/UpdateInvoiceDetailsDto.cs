namespace RailBook.Dtos.InvoiceDetails
{
    public class UpdateInvoiceDetailsDto
    {
        public int? TotalPrice { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? InvoiceDescription { get; set; }
    }
}
