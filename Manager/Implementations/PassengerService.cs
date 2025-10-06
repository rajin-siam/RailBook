using Mapster;
using RailBook.DataAccess.Interfaces;
using RailBook.Dtos.Passenger;
using RailBook.Manager.Interfaces;

namespace RailBook.Manager.Implementations
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepository _passengerRepository;

        public PassengerService(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<List<PassengerDto>> GetAllPassengersAsync()
        {
            var passengerList =  await _passengerRepository.GetAllAsync();
            return passengerList.Adapt<List<PassengerDto>>();
        }

        public async Task<PassengerDto?> GetPassengerByIdAsync(int id)
        {
            return (await _passengerRepository.GetByIdAsync(id)).Adapt<PassengerDto>();
        }

        public async Task AddPassengerAsync(PassengerDto passengerDto)
        {
            var passenger = passengerDto.Adapt<Passenger>();
            await _passengerRepository.AddAsync(passenger);
        }

        public async Task UpdatePassengerAsync(PassengerDto passenger)
        {
            await _passengerRepository.UpdateAsync(passenger.Adapt<Passenger>());
        }

        public async Task DeletePassengerAsync(int id)
        {
            await _passengerRepository.DeleteAsync(id);
        }


    }
}
