using RailBook.Dtos.Booking;
using RailBook.Dtos.Passenger;

namespace RailBook.Manager.Interfaces
{
    public interface IPassengerService
    {
        Task<List<PassengerDto>> GetAllPassengersAsync();
        Task<PassengerDto?> GetPassengerByIdAsync(int id);
        Task AddPassengerAsync(PassengerDto passengerDto);
        Task UpdatePassengerAsync(Booking existingBooking, UpdateBookingDto updatedBookingDto);
        Task DeletePassengerAsync(int id);
    }
}
