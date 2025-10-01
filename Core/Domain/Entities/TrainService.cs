
// ==================== TRAIN SERVICE ====================
// Note: This needs review - unclear how it relates to bookings
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RailBook.Core.Domain.Entities;

public class TrainService
{
    public int Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public int Price { get; set; }
    public int PassengerId { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedById { get; set; }
    public DateTime? ModifiedAt { get; set; }

    public virtual Passenger Passenger { get; set; } = null!;

    public class Configuration : IEntityTypeConfiguration<TrainService>
    {
        public void Configure(EntityTypeBuilder<TrainService> entity)
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).ValueGeneratedOnAdd();

            entity.Property(s => s.ServiceName).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Price).IsRequired().HasColumnType("integer");

            entity.Property(s => s.CreatedBy).IsRequired();
            entity.Property(s => s.CreatedAt).IsRequired();
            entity.Property(s => s.ModifiedById).IsRequired(false);
            entity.Property(s => s.ModifiedAt).IsRequired(false);

            // Relationship: TrainService belongs to Passenger
            entity.HasOne(s => s.Passenger)
                .WithMany(p => p.TrainServices)  // ✓ Reference the navigation property
                .HasForeignKey(s => s.PassengerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Audit relationships
            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(s => s.CreatedBy)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(s => s.ModifiedById)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
