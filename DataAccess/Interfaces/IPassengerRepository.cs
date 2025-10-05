namespace RailBook.DataAccess.Interfaces
{
    public interface IPassengerRepository
    {
        Task<List<Passenger>> GetAllAsync();
        Task<Passenger?> GetByIdAsync(int id);
        Task AddAsync(Passenger passenger);
        Task UpdateAsync(Passenger passenger);
        Task DeleteAsync(int id);
    }
}
