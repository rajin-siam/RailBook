using RailBook.Core.Domain.Entities;
using RailBook.Core.Domain.Repositories;

namespace RailBook.Core.Application.Services
{
    public class TrainService
    {
        private readonly IServiceRepository _serviceRepository;

        public TrainService(IServiceRepository trainRepository)
        {
            _serviceRepository = trainRepository;
        }

        public async Task<List<Service>> GetAllTrainsAsync()
        {
            return await _serviceRepository.GetAllAsync();
        }

        public async Task<Service?> GetTrainByIdAsync(int id)
        {
            return await _serviceRepository.GetByIdAsync(id);
        }

        public async Task AddTrainAsync(Service service)
        {
            await _serviceRepository.AddAsync(service);
        }

        public async Task UpdateTrainAsync(Service service)
        {
            await _serviceRepository.UpdateAsync(service);
        }

        public async Task DeleteTrainAsync(int id)
        {
            await _serviceRepository.DeleteAsync(id);
        }
    }
}
