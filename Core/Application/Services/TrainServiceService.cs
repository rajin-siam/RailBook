using RailBook.Core.Domain.Entities;
using RailBook.Core.Domain.Repositories;

namespace RailBook.Core.Application.Services
{
    public class TrainServiceService
    {
        private readonly ITrainServiceRepository _trainServiceRepository;

        public TrainServiceService(ITrainServiceRepository serviceRepository)
        {
            _trainServiceRepository = serviceRepository;
        }

        public async Task<List<TrainService>> GetAllTrainsAsync()
        {
            return await _trainServiceRepository.GetAllAsync();
        }

        public async Task<TrainService?> GetTrainByIdAsync(int id)
        {
            return await _trainServiceRepository.GetByIdAsync(id);
        }

        public async Task AddTrainAsync(TrainService service)
        {
            await _trainServiceRepository.AddAsync(service);
        }

        public async Task UpdateTrainAsync(TrainService service)
        {
            await _trainServiceRepository.UpdateAsync(service);
        }

        public async Task DeleteTrainAsync(int id)
        {
            await _trainServiceRepository.DeleteAsync(id);
        }
    }
}
