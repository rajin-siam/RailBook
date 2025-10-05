namespace RailBook.Manager.Interfaces
{
    public interface IPassengerService
    {
        Task<List<Passenger>> GetAllPassengersAsync();
        Task<Passenger?> GetPassengerByIdAsync(int id);
        Task AddPassengerAsync(Passenger passenger);
        Task UpdatePassengerAsync(Passenger passenger);
        Task DeletePassengerAsync(int id);
    }
}
