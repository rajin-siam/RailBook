using Microsoft.EntityFrameworkCore;
using RailBook.Core.Domain.Entities;

namespace RailBook.External.Persistence.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Passenger> Passengers => Set<Passenger>();
        public DbSet<TrainService> TrainServices => Set<TrainService>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceDetails> InvoiceDetails => Set<InvoiceDetails>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------- User ----------------
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Password).IsRequired();
            });

            // ---------------- Passenger ----------------
            modelBuilder.Entity<Passenger>(entity =>
            {
                entity.HasKey(p => p.PassengerId);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Gender).HasMaxLength(10);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(p => p.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------------- Service (Train) ----------------
            modelBuilder.Entity<TrainService>(entity =>
            {
                entity.HasKey(s => s.ServiceId);
                entity.Property(s => s.TrainName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.From).IsRequired().HasMaxLength(50);
                entity.Property(s => s.To).IsRequired().HasMaxLength(50);
                entity.Property(s => s.Price).IsRequired().HasColumnType("decimal(18,2)");

                entity.HasOne<Passenger>()
                      .WithMany()
                      .HasForeignKey(s => s.PassengerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(s => s.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------------- Booking ----------------
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.BookingId);
                entity.Property(b => b.Status).HasMaxLength(50).IsRequired();
                entity.Property(b => b.BookingDate).IsRequired();

                entity.HasOne<Passenger>()
                      .WithMany()
                      .HasForeignKey(b => b.PassengerId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<Invoice>()
                      .WithOne()
                      .HasForeignKey<Booking>(b => b.InvoiceId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(b => b.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------------- Invoice ----------------
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(i => i.InvoiceId);

                entity.HasOne<Passenger>()
                      .WithMany()
                      .HasForeignKey(i => i.PassengerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(i => i.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(i => i.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------------- InvoiceDetails ----------------
            modelBuilder.Entity<InvoiceDetails>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.TotalPrice).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(d => d.InvoiceDate).IsRequired();

                entity.HasOne<Invoice>()
                      .WithOne()
                      .HasForeignKey<InvoiceDetails>(d => d.InvoiceId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(d => d.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
