namespace RailBook.Core.Application.Dtos.Service
{
    public class TrainServiceDto
    {
        public int ServiceId { get; set; }
        public string TrainName { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int PassengerId { get; set; }
        public int CreatedBy { get; set; }
    }
}
