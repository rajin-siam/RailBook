using Microsoft.EntityFrameworkCore;
using RailBook.Core.Domain.Entities;
using RailBook.Core.Domain.Repositories;
using RailBook.External.Persistence.Database;


namespace RailBook.External.Persistence.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly ApplicationDbContext _context;
        public PassengerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Passenger>> GetAllAsync()
        {
            return await _context.Passengers.ToListAsync();
        }

        public async Task<Passenger?> GetByIdAsync(int id)
        {
            return await _context.Passengers.FindAsync(id);
        }

        public async Task AddAsync(Passenger passenger)
        {
            await _context.Passengers.AddAsync(passenger);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Passenger passenger)
        {
            _context.Passengers.Update(passenger);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger != null)
            {
                _context.Passengers.Remove(passenger);
                await _context.SaveChangesAsync();
            }
        }
    }
}
