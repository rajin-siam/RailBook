using Microsoft.EntityFrameworkCore;
using RailBook.DataAccess.Database;
using RailBook.DataAccess.Interfaces;

namespace RailBook.DataAccess.Implementations
{
    public class TrainServiceRepository : ITrainServiceRepository
    {
        private readonly ApplicationDbContext _context;
        public TrainServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrainService>> GetAllAsync()
        {
            return await _context.TrainServices.ToListAsync();
        }

        public async Task<TrainService?> GetByIdAsync(int id)
        {
            return await _context.TrainServices.FindAsync(id);
        }

        public async Task AddAsync(TrainService service)
        {
            await _context.TrainServices.AddAsync(service);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TrainService service)
        {
            _context.TrainServices.Update(service);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var service = await _context.TrainServices.FindAsync(id);
            if (service != null)
            {
                _context.TrainServices.Remove(service);
                await _context.SaveChangesAsync();
            }
        }
    }
}
