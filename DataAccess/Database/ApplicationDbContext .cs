using Microsoft.EntityFrameworkCore;
using RailBook.Domain.Entities;

namespace RailBook.DataAccess.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) 
        { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Passenger> Passengers => Set<Passenger>();
        public DbSet<TrainService> TrainServices => Set<TrainService>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceDetails> InvoiceDetails => Set<InvoiceDetails>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()   // <--- Enable lazy loading here
                .UseNpgsql("Host=localhost;Database=RailBookDb;Username=postgres;Password=iamdragon;");
        }

    }
}