namespace RailBook.Manager.Interfaces
{
    public interface ITrainServiceService
    {
        Task<List<TrainService>> GetAllTrainsAsync();
        Task<TrainService?> GetTrainByIdAsync(int id);
        Task AddTrainAsync(TrainService service);
        Task UpdateTrainAsync(TrainService service);
        Task DeleteTrainAsync(int id);
    }
}
