using RailBook.Core.Domain.Entities;
using RailBook.Core.Domain.Repositories;

namespace RailBook.Application.Services
{
    public class PassengerService
    {
        private readonly IPassengerRepository _passengerRepository;

        public PassengerService(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<List<Passenger>> GetAllPassengersAsync()
        {
            return await _passengerRepository.GetAllAsync();
        }

        public async Task<Passenger?> GetPassengerByIdAsync(int id)
        {
            return await _passengerRepository.GetByIdAsync(id);
        }

        public async Task AddPassengerAsync(Passenger passenger)
        {
            await _passengerRepository.AddAsync(passenger);
        }

        public async Task UpdatePassengerAsync(Passenger passenger)
        {
            await _passengerRepository.UpdateAsync(passenger);
        }

        public async Task DeletePassengerAsync(int id)
        {
            await _passengerRepository.DeleteAsync(id);
        }
    }
}
