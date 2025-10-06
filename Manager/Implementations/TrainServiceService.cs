using Mapster;
using RailBook.DataAccess.Interfaces;
using RailBook.Dtos.Service;
using RailBook.Manager.Interfaces;

namespace RailBook.Manager.Implementations
{
    public class TrainServiceService : ITrainServiceService
    {
        private readonly ITrainServiceRepository _trainServiceRepository;

        public TrainServiceService(ITrainServiceRepository serviceRepository)
        {
            _trainServiceRepository = serviceRepository;
        }

        public async Task<List<TrainServiceDto>> GetAllTrainsAsync()
        {
            var trainServiceList =  await _trainServiceRepository.GetAllAsync();
            return trainServiceList.Adapt<List<TrainServiceDto>>();
        }

        public async Task<TrainServiceDto?> GetTrainByIdAsync(int id)
        {
            return (await _trainServiceRepository.GetByIdAsync(id)).Adapt<TrainServiceDto>();
        }

        public async Task AddTrainAsync(TrainServiceDto serviceDto)
        {
            var service = serviceDto.Adapt<TrainService>();
            await _trainServiceRepository.AddAsync(service);
        }

        public async Task UpdateTrainAsync(TrainServiceDto serviceDto)
        {
            await _trainServiceRepository.UpdateAsync(serviceDto.Adapt<TrainService>());
        }

        public async Task DeleteTrainAsync(int id)
        {
            await _trainServiceRepository.DeleteAsync(id);
        }

    }
}
