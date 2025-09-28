using Microsoft.EntityFrameworkCore;
using RailBook.Core.Domain.Entities;
using RailBook.Core.Domain.Repositories;
using RailBook.External.Persistence.Database;


namespace RailBook.External.Persistence.Repositories
{
    public class InvoiceDetailsRepository : IInvoiceDetailsRepository
    {
        private readonly ApplicationDbContext _context;
        public InvoiceDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InvoiceDetails>> GetAllAsync()
        {
            return await _context.InvoiceDetails.ToListAsync();
        }

        public async Task<InvoiceDetails?> GetByIdAsync(int id)
        {
            return await _context.InvoiceDetails.FindAsync(id);
        }

        public async Task AddAsync(InvoiceDetails details)
        {
            await _context.InvoiceDetails.AddAsync(details);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(InvoiceDetails details)
        {
            _context.InvoiceDetails.Update(details);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var details = await _context.InvoiceDetails.FindAsync(id);
            if (details != null)
            {
                _context.InvoiceDetails.Remove(details);
                await _context.SaveChangesAsync();
            }
        }
    }
}
