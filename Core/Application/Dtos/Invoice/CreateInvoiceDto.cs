namespace RailBook.Core.Application.Dtos.Invoice
{
    public class CreateInvoiceDto
    {
        public int PassengerId { get; set; }
        public int UserId { get; set; }
        public int CreatedBy { get; set; }
    }
}
