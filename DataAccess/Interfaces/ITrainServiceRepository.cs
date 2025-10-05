namespace RailBook.DataAccess.Interfaces
{
    public interface ITrainServiceRepository
    {
        Task<List<TrainService>> GetAllAsync();
        Task<TrainService?> GetByIdAsync(int id);
        Task AddAsync(TrainService service);
        Task UpdateAsync(TrainService service);
        Task DeleteAsync(int id);
    }
}
