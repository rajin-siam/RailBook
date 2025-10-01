
// ==================== PASSENGER ====================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RailBook.Core.Domain.Entities;

public class Passenger
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
    public int BookingId { get; set; } // FK to Booking (FIXED: was string)
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedById { get; set; }
    public DateTime? ModifiedAt { get; set; }

    // Navigation properties
    public virtual Booking? Booking { get; set; }
    public virtual List<TrainService> TrainServices { get; set; } = [];

    public class Configuration : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> entity)
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).ValueGeneratedOnAdd();

            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Gender).HasMaxLength(10);
            entity.Property(p => p.Age).HasColumnType("integer");

            entity.Property(p => p.CreatedAt).IsRequired();
            entity.Property(p => p.ModifiedById).IsRequired(false);
            entity.Property(p => p.ModifiedAt).IsRequired(false);

            // Relationship: Passenger belongs to Booking
            entity.HasOne(p => p.Booking)
                  .WithMany(b => b.Passengers)
                  .HasForeignKey(p => p.BookingId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Audit relationships
            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(p => p.CreatedBy)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(p => p.ModifiedById)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
