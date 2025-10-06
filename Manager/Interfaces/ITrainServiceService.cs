using RailBook.Dtos.Service;

namespace RailBook.Manager.Interfaces
{
    public interface ITrainServiceService
    {
        Task<List<TrainServiceDto>> GetAllTrainsAsync();
        Task<TrainServiceDto?> GetTrainByIdAsync(int id);
        Task AddTrainAsync(TrainServiceDto serviceDto);
        Task UpdateTrainAsync(TrainServiceDto serviceDto);
        Task DeleteTrainAsync(int id);
    }
}
