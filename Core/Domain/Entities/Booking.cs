
// ==================== BOOKING ====================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RailBook.Core.Domain.Entities;

public class Booking
{
    public int Id { get; set; }
    public DateTime BookingDate { get; set; }
    public string Status { get; set; } = "Pending";
    public string Source { get; set; } = "";
    public string Destination { get; set; } = "";
    public int PerTicketPrice { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedById { get; set; }
    public DateTime? ModifiedAt { get; set; }

    // Navigation properties
    public virtual List<Passenger> Passengers { get; set; } = [];
    public virtual Invoice? Invoice { get; set; }

    public class Configuration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> entity)
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Id).ValueGeneratedOnAdd();

            entity.Property(b => b.Status).IsRequired().HasMaxLength(50);
            entity.Property(b => b.BookingDate).IsRequired();
            entity.Property(b => b.Source).IsRequired();
            entity.Property(b => b.Destination).IsRequired();
            entity.Property(b => b.PerTicketPrice).IsRequired();

            entity.Property(b => b.CreatedBy).IsRequired();
            entity.Property(b => b.CreatedAt).IsRequired();
            entity.Property(b => b.ModifiedById).IsRequired(false);
            entity.Property(b => b.ModifiedAt).IsRequired(false);

            // Relationship

            // Audit relationships
            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(b => b.CreatedBy)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(b => b.ModifiedById)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
