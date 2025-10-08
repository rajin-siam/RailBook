using Microsoft.EntityFrameworkCore;
using RailBook.DataAccess.Database;
using RailBook.DataAccess.Interfaces;

namespace RailBook.DataAccess.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Bookings.ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            // This entity is now TRACKED by EF Core
            // Any changes to it will be automatically detected
            return await _context.Bookings.FindAsync(id);
        }

        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        // ✅ BEST APPROACH: Just save changes
        // EF Core already tracks the entity from GetByIdAsync()
        public async Task UpdateAsync(Booking booking)
        {
            // No need for _context.Bookings.Update(booking)
            // because the entity is already being tracked!

            // Just save all tracked changes
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }
    }
}