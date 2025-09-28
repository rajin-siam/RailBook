namespace RailBook.Core.Application.Dtos.Service
{
    public class UpdateTrainServiceDto
    {
        public string TrainName { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
