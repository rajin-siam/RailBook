namespace RailBook.Dtos.Service
{
    public class TrainServiceDto
    {
        public int Id { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public int Price { get; set; }
        public int PassengerId { get; set; }
    }
}


