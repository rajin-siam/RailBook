using RailBook.Dtos.Passenger;

namespace RailBook.Manager.Interfaces
{
    public interface IPassengerService
    {
        Task<List<PassengerDto>> GetAllPassengersAsync();
        Task<PassengerDto?> GetPassengerByIdAsync(int id);
        Task AddPassengerAsync(PassengerDto passengerDto);
        Task UpdatePassengerAsync(PassengerDto passenger);
        Task DeletePassengerAsync(int id);
    }
}
